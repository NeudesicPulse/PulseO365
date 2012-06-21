using System;
using System.Web;
using System.ComponentModel;
using System.Collections.Specialized;
using Microsoft.SharePoint;
using System.Collections.Generic;
using System.Xml;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint.Utilities;

namespace Neudesic.Pulse.SharePoint.Online
{
    [ToolboxItemAttribute(false)]
    public abstract class FeedWebPartBase : PulseWebPartBase
    {
        [WebBrowsable(true),
        WebDescription(WebPartPropertyDescription.DisplayMode),
        WebDisplayName(WebPartPropertyName.DisplayMode),
        Personalizable(PersonalizationScope.Shared),
        Category("Pulse"),
        DefaultValue(DisplayMode.Mini)]
        public DisplayMode DisplayMode
        {
            get;
            set;
        }

        protected abstract string GetPulseRelativeFeedUrl();

        protected override string GetPulseRelativeContentUrl()
        {
            if (DisplayMode == DisplayMode.Full)
            {
                return SPUtility.ConcatUrls("/embed", GetPulseRelativeFeedUrl());
            }
            else
            {
                return SPUtility.ConcatUrls(GetPulseRelativeFeedUrl(), "/embed");
            }
        }
    }
}