using System;
using System.Web;
using System.ComponentModel;
using System.Collections.Specialized;
using Microsoft.SharePoint;
using System.Collections.Generic;

namespace Neudesic.Pulse.SharePoint.Online
{
    [ToolboxItemAttribute(false)]
    public class DiscussListWebPartBase : DiscussWebPartBase
    {
        protected override StreamEntity GetStreamEntity()
        {
            var list = PulseUtility.TryGetCurrentContextList();
            return list == null ? null : StreamEntity.CreateFrom(list);
        }

        protected override ConfigurationError GetCurrentContextError()
        {
            return new ConfigurationError(
                    "This web part requires a SharePoint list context.",
                    "Ensure this web part is being used on a form that has a SharePoint list context.  For example, this web part could be placed on the default view page of a SharePoint list.");
        }
    }
}