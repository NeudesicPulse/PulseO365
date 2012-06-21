using System;
using System.Web;
using System.ComponentModel;
using System.Collections.Specialized;
using Microsoft.SharePoint;
using System.Collections.Generic;

namespace Neudesic.Pulse.SharePoint.Online
{
    [ToolboxItemAttribute(false)]
    public class DiscussSiteWebPartBase : DiscussWebPartBase
    {
        protected override StreamEntity GetStreamEntity()
        {
            return SPContext.Current.Web == null ? null : StreamEntity.CreateFrom(SPContext.Current.Web);
        }

        protected override ConfigurationError GetCurrentContextError()
        {
            return new ConfigurationError(
                    "This web part requires at least a SharePoint web context.",
                    "Ensure this web part is being used on a form that has at least a SharePoint web context.  For example, this web part could be placed on the home page of a SharePoint web.");
        }
    }
}