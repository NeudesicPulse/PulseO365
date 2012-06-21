using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Neudesic.Pulse.SharePoint.Online
{
    public class EmbedParameter
    {
        public EmbedParameter(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public EmbedParameter(string key, string value, Func<string, string> customValueEncode)
        {
            Key = key;
            Value = value;
            CustomValueEncode = customValueEncode;
        }

        public string Key
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }

        public Func<string, string> CustomValueEncode
        {
            get;
            set;
        }

        public string ToQueryParameter()
        {
            return string.Format("{0}={1}", HttpUtility.UrlEncode(Key), getEncodedValue());
        }

        private string getEncodedValue()
        {
            if (Value == null)
            {
                return null;
            }
            else
            {
                return CustomValueEncode == null ? HttpUtility.UrlEncode(Value) : CustomValueEncode(Value);
            }
        }
    }

    public class EmbedParameterList : List<EmbedParameter>
    {
        public string ToQueryString(Settings settings)
        {
            List<string> items = new List<string>();
            foreach (var parameter in this)
            {
                items.Add(parameter.ToQueryParameter());
            }

            return String.Join("&", items.ToArray());
        }

        public void Set(EmbedParameter parameter)
        {
            this.RemoveAll(x => x.Key.Equals(parameter.Key, StringComparison.InvariantCultureIgnoreCase));
            this.Add(parameter);
        }

        public static EmbedParameterList ParseQuery(string query)
        {
            var result = new EmbedParameterList();

            if (!string.IsNullOrEmpty(query))
            {
                var parsedQuery = HttpUtility.ParseQueryString(query);
                foreach (var key in parsedQuery.AllKeys)
                {
                    result.Add(new EmbedParameter(key, parsedQuery[key])); 
                }
            }
            return result;
        }

        public void MergeRange(IEnumerable<EmbedParameter> parameters, bool replaceIfExists)
        {
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    if (!this.Any(x => x.Key.Equals(parameter.Key, StringComparison.InvariantCultureIgnoreCase)) || replaceIfExists)
                    {
                        this.Set(parameter);
                    }
                }
            }
        }
    }
}
