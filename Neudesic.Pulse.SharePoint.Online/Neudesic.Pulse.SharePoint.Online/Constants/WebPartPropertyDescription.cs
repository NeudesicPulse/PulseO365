using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neudesic.Pulse.SharePoint.Online
{
    public static class WebPartPropertyDescription
    {
        public const string DisplayMode = "Choose the display mode for Pulse content.  'Full' mode will render the full Pulse UI.  'Mini' mode will only render the Pulse discussion UI.";
        public const string FeatureUrl = "Enter the URL of a Pulse feature you want to embed. For example, to embed the 'Questions' feature input '/streams/questions'.  You may enter a full or relative URL.";
        public const string FeedUrl = "Feed URLs are used to segment conversations within Pulse.  Enter the URL of an existing Pulse feed you want to embed.  For example, to embed the current user's feed enter '/streams/my'.  You may enter a full or relative URL.";
    }
}
