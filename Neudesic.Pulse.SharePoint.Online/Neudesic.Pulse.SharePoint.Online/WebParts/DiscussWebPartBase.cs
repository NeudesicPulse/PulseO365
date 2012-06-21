using System;
using System.Web;
using System.ComponentModel;
using System.Collections.Specialized;
using Microsoft.SharePoint;
using System.Collections.Generic;
using System.Xml;
using Microsoft.SharePoint.Utilities;

namespace Neudesic.Pulse.SharePoint.Online
{
    [ToolboxItemAttribute(false)]
    public abstract class DiscussWebPartBase : FeedWebPartBase
    {
        protected override string GetPulseRelativeFeedUrl()
        {
            return "/streams/pages";
        }

        protected override string GetPulseRelativeContentUrl()
        {
            return SPUtility.ConcatUrls(GetPulseRelativeFeedUrl(), "/embed");
        }

        protected override EmbedParameterList GetEmbedParameters()
        {
            var parameters = base.GetEmbedParameters();

            var entity = GetStreamEntity();
            parameters.Set(new EmbedParameter(ParameterKey.Title, entity.Name));
            parameters.Set(new EmbedParameter(ParameterKey.Url, entity.LocalUrl));
            parameters.Set(new EmbedParameter(ParameterKey.ExternalKey, entity.ExternalKey));
            parameters.Set(new EmbedParameter(ParameterKey.EntityType, entity.Type));
            parameters.Set(new EmbedParameter(ParameterKey.Full, (DisplayMode == DisplayMode.Full).ToString()));
            parameters.Set(new EmbedParameter(ParameterKey.ParentId, Settings.ParentSystemFeedId));

            return parameters;
        }

        protected override ConfigurationErrorList Validate()
        {
            var errors = base.Validate();

            if (GetStreamEntity() == null)
            {
                errors.Add(GetCurrentContextError());
            }

            return errors;
        }

        protected virtual StreamEntity GetStreamEntity()
        {
            return PulseUtility.GetCurrentContextStreamEntity();
        }

        protected abstract ConfigurationError GetCurrentContextError();
    }
}