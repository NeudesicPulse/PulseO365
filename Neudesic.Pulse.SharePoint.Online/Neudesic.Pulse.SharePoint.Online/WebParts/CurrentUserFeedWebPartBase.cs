using System;
using System.Web;
using System.ComponentModel;
using System.Collections.Specialized;
using Microsoft.SharePoint;
using System.Collections.Generic;

namespace Neudesic.Pulse.SharePoint.Online
{
    [ToolboxItemAttribute(false)]
    public class CurrentUserFeedWebPartBase : FeedWebPartBase
    {
        protected override string GetPulseRelativeFeedUrl()
        {
            return "/streams/my";
        }

        protected override EmbedParameterList GetEmbedParameters()
        {
            var parameters = base.GetEmbedParameters();

            parameters.Set(new EmbedParameter(ParameterKey.View, "personal"));
            
            return parameters;
        }
    }
}