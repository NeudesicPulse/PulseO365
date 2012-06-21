using System;
using System.Web;
using System.ComponentModel;
using System.Collections.Specialized;
using Microsoft.SharePoint;
using System.Collections.Generic;
using Microsoft.SharePoint.Utilities;
using System.Web.UI.WebControls.WebParts;

namespace Neudesic.Pulse.SharePoint.Online
{
    [ToolboxItemAttribute(false)]
    public class ExistingFeatureWebPartBase : PulseWebPartBase
    {
        protected override string GetPulseRelativeContentUrl()
        {
            return SPUtility.ConcatUrls("/embed", PulseUtility.GetPulseRelativeUrl(PageUrl));
        }

        [WebBrowsable(true),
        WebDescription(WebPartPropertyDescription.FeatureUrl),
        WebDisplayName(WebPartPropertyName.FeatureUrl),
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

            errors.AddRange(PulseUtility.ValidateRequiredPulseUrlPropertyValue(PageUrl, WebPartPropertyName.FeatureUrl));

            return errors;
        }
    }
}