using System;
using System.Web;
using System.ComponentModel;
using System.Collections.Specialized;
using Microsoft.SharePoint;
using System.Collections.Generic;
using Microsoft.SharePoint.Utilities;

namespace Neudesic.Pulse.SharePoint.Online
{
    [ToolboxItemAttribute(false)]
    public class DynamicContentWebPartBase : PulseWebPartBase
    {
        protected override string GetPulseRelativeContentUrl()
        {
            var contentUrl = PulseUtility.GetPulseRelativeUrl(this.Context.Request["pulseurl"]);
            return string.IsNullOrEmpty(contentUrl) ? "/embed/streams/my" : SPUtility.ConcatUrls("/embed", contentUrl);
        }
    }
}