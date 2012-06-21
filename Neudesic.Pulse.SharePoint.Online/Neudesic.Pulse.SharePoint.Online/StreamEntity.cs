using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using System.Xml;

namespace Neudesic.Pulse.SharePoint.Online
{
    public class StreamEntity
    {
        public string Name
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string LocalUrl
        {
            get;
            set;
        }

        public string ExternalKey
        {
            get;
            set;
        }

        public string Type
        {
            get;
            set;
        }

        private StreamEntity(string name, string description, string localUrl, string externalKey, string type)
        {
            Name = name;
            Description = description;
            LocalUrl = localUrl;
            ExternalKey = externalKey;
            Type = type;
        }

        public static StreamEntity CreateFrom(SPWeb web)
        {
            return new StreamEntity(web.Title, web.Description, PulseUtility.GetConsistentUrl(web), PulseUtility.GetExternalKey(web), PulseUtility.GetEntityType(web));
        }

        public static StreamEntity CreateFrom(SPList list)
        {
            return new StreamEntity(list.Title, list.Description, PulseUtility.GetConsistentUrl(list), PulseUtility.GetExternalKey(list), PulseUtility.GetEntityType(list));
        }

        public static StreamEntity CreateFrom(SPListItem item)
        {
            return new StreamEntity(PulseUtility.GetName(item), null, PulseUtility.GetConsistentUrl(item), PulseUtility.GetExternalKey(item), PulseUtility.GetEntityType(item));
        }
    }
}
