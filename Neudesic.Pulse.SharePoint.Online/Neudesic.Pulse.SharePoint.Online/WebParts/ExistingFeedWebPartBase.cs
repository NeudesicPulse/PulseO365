using System;
using System.Web;
using System.ComponentModel;
using System.Collections.Specialized;
using Microsoft.SharePoint;
using System.Collections.Generic;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint.Utilities;

namespace Neudesic.Pulse.SharePoint.Online
{
    [ToolboxItemAttribute(false)]
    public class ExistingFeedWebPartBase : FeedWebPartBase
    {
        protected override string GetPulseRelativeFeedUrl()
        {
            return PulseUtility.GetPulseRelativeUrl(PageUrl);
        }

        [WebBrowsable(true),
        WebDescription(WebPartPropertyDescription.FeedUrl),
        WebDisplayName(WebPartPropertyName.FeedUrl),
        Personalizable(PersonalizationScope.Shared),
        Category("Pulse"),
        DefaultValue("/streams/my")]
        public string PageUrl
        {
            get;
            set;
        }

        protected override ConfigurationErrorList Validate()
        {
            var errors = base.Validate();

            errors.AddRange(PulseUtility.ValidateRequiredPulseUrlPropertyValue(PageUrl, WebPartPropertyName.FeedUrl));

            return errors;
        }
    }
}