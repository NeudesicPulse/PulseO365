using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using Microsoft.SharePoint;
using System.Security.Cryptography;
using Microsoft.SharePoint.Utilities;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using System.Web;
using System.Xml.Serialization;
using System.Reflection;
using System.Data;
using System.Security.Cryptography.X509Certificates;

namespace Neudesic.Pulse.SharePoint.Online
{
    public class PulseUtility
    {
        public static bool IsSandboxed()
        {
            return AppDomain.CurrentDomain.FriendlyName.Contains("Sandbox");
        }

        public static bool IsContextValid(HttpContext context)
        {
            return SPContext.Current != null && SPContext.Current.Web != null && context != null && context.Request != null;
        }

        public static Settings GetSettings()
        {
            return new Settings(SettingsValue.PulseBaseUrl, SettingsValue.CustomCss, SettingsValue.ParentSystemFeedId); //implement your own settings store if desired
        }
        
        public static string GetTitle(SPWeb web)
        {
            return web == null ? null : web.Title;
        }

        public static string GetTitle(SPListItem item)
        {
            string result = null;

            if (item != null && item.ID != 0)
            {
                if (item.File == null)
                {
                    result = item.Title;
                    if (string.IsNullOrEmpty(result))
                    {
                        result = item.Name;
                        if (string.IsNullOrEmpty(result))
                        {
                            result = item.DisplayName;
                            if (string.IsNullOrEmpty(result))
                            {
                                result = item.UniqueId.ToString();
                            }
                        }
                    }
                }
                else
                {
                    result = item.DisplayName;
                    if (string.IsNullOrEmpty(result))
                    {
                        result = item.UniqueId.ToString();
                    }
                }
            }

            return result;
        }

        public static string GetPulseRelativeUrl(string url)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(url))
            {
                var parts = url.Split('/');
                var validBaseParts = new string[] { "streams", "reports", "settings", "developers" };
                bool appending = false;

                foreach (var part in parts)
                {
                    var p = part;

                    if (!appending)
                    {
                        if (p.StartsWith("#!"))
                        {
                            p = p.Remove(0, 2);
                        }

                        if (validBaseParts.Contains(p, StringComparer.InvariantCultureIgnoreCase))
                        {
                            appending = true;
                        }
                    }

                    if (appending)
                    {
                        result += "/" + p;
                    }
                }
            }

            return result;
        }

        public static string GetPulseStreamUrl(string streamExternalKey, string pulseBaseUrl)
        {
            return SPUtility.ConcatUrls(pulseBaseUrl, string.Format("streams/external.{0}", streamExternalKey));
        }

        public static ConfigurationErrorList ValidateRequiredPulseUrlPropertyValue(string propertyValue, string propertyDisplayName)
        {
            var errors = new ConfigurationErrorList();

            if (string.IsNullOrEmpty(propertyValue))
            {
                errors.Add(new ConfigurationError(
                    string.Format("The required web part property '{0}' has not been set.", propertyDisplayName),
                    string.Format("Ensure the required web part property '{0}' is set on this web part.  You may access this property by editing the web part and expanding the 'Pulse' section.", propertyDisplayName)));
            }
            else if (string.IsNullOrEmpty(PulseUtility.GetPulseRelativeUrl(propertyValue)))
            {
                errors.Add(new ConfigurationError(
                    string.Format("The value of web part property '{0}' is not recognized as a valid Pulse URL.", propertyDisplayName),
                    string.Format("Ensure the web part property '{0}' is set to a valid Pulse URL.  You may access this property by editing the web part and expanding the 'Pulse' section.", propertyDisplayName)));
            }

            return errors;
        }

        public static SPListItem TryGetCurrentContextListItem()
        {
            if (SPContext.Current.ItemId == 0)
            {
                return null;
            }

            var list = TryGetCurrentContextList();
            if (list != null)
            {
                try
                {
                    return list.GetItemById(SPContext.Current.ItemId);
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return null;
        }

        public static SPList TryGetCurrentContextList()
        {
            try
            {
                return SPContext.Current.Web.Lists[SPContext.Current.ListId];
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static StreamEntity GetCurrentContextStreamEntity()
        {
            if (SPContext.Current != null)
            {
                var item = TryGetCurrentContextListItem();
                if (item != null)
                {
                    return StreamEntity.CreateFrom(item);
                }
                else
                {
                    var list = TryGetCurrentContextList();
                    if (list != null)
                    {
                        return StreamEntity.CreateFrom(list);
                    }
                    else
                    {
                        var web = SPContext.Current.Web;
                        if (web != null)
                        {
                            return StreamEntity.CreateFrom(web);
                        }
                    }
                }
            }

            return null;
        }

        public static bool TryParseAccountName(string value, out string result)
        {
            bool parsed = true;
            result = value;

            if (result != null && result.Trim() != string.Empty)
            {
                result = result.Trim();

                if (result.Contains("|"))
                {
                    string[] accountNameParts = result.Split('|');
                    if (accountNameParts.Length == 3 && !string.IsNullOrEmpty(accountNameParts[2].Trim()))
                    {
                        result = accountNameParts[2];
                    }
                    else
                    {
                        parsed = false;
                    }
                }

                if (result.Contains("\\"))
                {
                    string[] accountNameParts = result.Split('\\');
                    if (accountNameParts.Length != 2 || string.IsNullOrEmpty(accountNameParts[1].Trim()))
                    {
                        parsed = false;
                    }
                }
            }
            else
            {
                parsed = false;
            }

            if (!parsed)
            {
                result = null;
            }

            return parsed;
        }

        public static string GetConsistentUrl(SPWeb web)
        {
            return web.Url;
        }

        public static string GetConsistentUrl(SPListItem item)
        {
            return String.Format("{0}?ID={1}", SPUtility.ConcatUrls(GetConsistentUrl(item.Web), item.ParentList.Forms.Cast<SPForm>().First(x => x.Type == PAGETYPE.PAGE_DISPLAYFORM).Url), item.ID);
        }

        public static string GetConsistentUrl(SPList list)
        {
            return SPUtility.ConcatUrls(GetConsistentUrl(list.ParentWeb), list.DefaultView.Url);
        }

        public static string GetName(SPListItem item)
        {
            string result = null;

            if (item.Fields.ContainsField("Title")) //careful some items don't have 'Title' field (ex. wiki)
            {
                result = item.Title;
            }

            if (string.IsNullOrEmpty(result))
            {
                result = item.DisplayName;
                if (string.IsNullOrEmpty(result) && item.Fields.ContainsField("Name"))
                {
                    result = item.Name;
                }
            }

            if (string.IsNullOrEmpty(result))
            {
                result = item.ContentType.Name;
            }

            return result;
        }

        public static string GetExternalKey(SPWeb web)
        {
            return web.ID.ToString();
        }

        public static string GetExternalKey(SPList list)
        {
            return list.ID.ToString();
        }

        public static string GetExternalKey(SPListItem item)
        {
            return item.UniqueId.ToString();
        }

        public static XmlDocument GetEntityTypeMappings()
        {
            var result = new XmlDocument();
            result.LoadXml(Template.EntityTypeMappings);
            return result;
        }

        public static string GetEntityType(SPWeb web)
        {
            return "web";
        }

        public static string GetEntityType(SPList list)
        {
            return "list";
        }

        public static string GetEntityType(SPListItem item)
        {
            string result = "item";

            var entityTypeMappings = GetEntityTypeMappings();
            var entityTypeNodes = entityTypeMappings.SelectNodes("entityTypes/entityType")
                .Cast<XmlNode>()
                .Where(x => x.Attributes["contentType"] != null && !string.IsNullOrEmpty(x.Attributes["contentType"].Value))
                .OrderByDescending(x => x.Attributes["contentType"].Value.Length);

            foreach (var entityTypeNode in entityTypeNodes)
            {
                var contentType = new SPContentTypeId(entityTypeNode.Attributes["contentType"].Value);
                if (item.ContentType.Id.CompareTo(contentType) == 0 || item.ContentType.Id.IsChildOf(contentType))
                {
                    result = entityTypeNode.Attributes["code"].Value;
                    break;
                }
            }

            return result;
        }
    }
}
