using System;
using System.Web;
using System.ComponentModel;
using System.Collections.Specialized;
using Microsoft.SharePoint;
using System.Collections.Generic;

namespace Neudesic.Pulse.SharePoint.Online
{
    [ToolboxItemAttribute(false)]
    public class DiscussItemWebPartBase : DiscussWebPartBase
    {
        protected override StreamEntity GetStreamEntity()
        {
            var item = PulseUtility.TryGetCurrentContextListItem();
            return item == null ? null : StreamEntity.CreateFrom(item);
        }

        protected override ConfigurationError GetCurrentContextError()
        {
            return new ConfigurationError(
                    "This web part requires a SharePoint list item context.",
                    "Ensure this web part is being used on a form that has a SharePoint list item context.  For example, this web part could be placed on the display or edit form of a SharePoint list item.");
        }
    }
}