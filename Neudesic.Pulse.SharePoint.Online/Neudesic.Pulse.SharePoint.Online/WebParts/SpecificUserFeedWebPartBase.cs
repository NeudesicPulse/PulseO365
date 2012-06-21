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
    public class SpecificUserFeedWebPartBase : FeedWebPartBase
    {
        protected override string GetPulseRelativeFeedUrl()
        {
            string accountName = null;
            return PulseUtility.TryParseAccountName(this.Context.Request["accountname"], out accountName) ? SPUtility.ConcatUrls("/streams/r/", accountName) : "/streams/my";
        }

        protected override EmbedParameterList GetEmbedParameters()
        {
            var parameters = base.GetEmbedParameters();

            parameters.Set(new EmbedParameter(ParameterKey.View, "timeline"));

            return parameters;
        }

        protected override ConfigurationErrorList Validate()
        {
            var errors = base.Validate();

            string accountName = this.Context.Request["accountname"];
            string accountNameParsed = null;

            if (accountName != null && accountName.Trim() != string.Empty && !PulseUtility.TryParseAccountName(accountName, out accountNameParsed))
            {
                errors.Add(new ConfigurationError(
                    "The account name parameter format is not recognized and cannot be parsed.",
                    string.Format("Ensure this web part is being used on a form that is passed a valid 'accountname' query string parameter.  For example, this web part could be placed on a person's 'My Site' page.  The current unparsable account name value is '{0}'.", accountName)));
            }

            return errors;
        }
    }
}