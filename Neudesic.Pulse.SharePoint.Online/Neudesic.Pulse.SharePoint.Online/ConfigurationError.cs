using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neudesic.Pulse.SharePoint.Online
{
    public class ConfigurationError
    {
        public string Title
        {
            get;
            set;
        }

        public string HelpHtml
        {
            get;
            set;
        }

        public ConfigurationError(string title, string helpHtml)
        {
            Title = title;
            HelpHtml = helpHtml;
        }
    }

    public class ConfigurationErrorList : List<ConfigurationError>
    {
        public string ToHtml()
        {
            var result = new StringBuilder();

            foreach (var error in this)
            {
                result.AppendFormat("<li><b>{0}</b></li>{1}", error.Title, error.HelpHtml);
            }
            
            return result.ToString();
        }
    }
}
