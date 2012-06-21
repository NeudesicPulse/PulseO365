using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Xml.Serialization;

namespace Neudesic.Pulse.SharePoint.Online
{
    [XmlRoot("settings")]
    public class Settings
    {
        public Settings()
        {
        }

        public Settings(string pulseBaseUrl, string customCss, string parentSystemFeedId)
        {
            PulseBaseUrl = pulseBaseUrl;
            CustomCss = customCss;
            ParentSystemFeedId = parentSystemFeedId;
        }

        [XmlElement("pulseBaseUrl")]
        public string PulseBaseUrl
        {
            get;
            set;
        }

        [XmlElement("customCss")]
        public string CustomCss
        {
            get;
            set;
        }

        [XmlElement("parentSystemFeedId")]
        public string ParentSystemFeedId
        {
            get;
            set;
        }
        
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(PulseBaseUrl) && !string.IsNullOrEmpty(ParentSystemFeedId);
        }
    }
}
