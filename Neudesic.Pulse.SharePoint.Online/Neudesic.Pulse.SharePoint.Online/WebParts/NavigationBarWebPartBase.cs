using System;
using System.Web;
using System.ComponentModel;
using System.Collections.Specialized;
using Microsoft.SharePoint;
using System.Collections.Generic;
using System.Web.UI;

namespace Neudesic.Pulse.SharePoint.Online
{
    [ToolboxItemAttribute(false)]
    public class NavigationBarWebPartBase : PulseWebPartBase
    {
        protected override string GetPulseRelativeContentUrl()
        {
            return "/embed/navbar";
        }

        protected override string GetEmbedContent()
        {
            var embedUrl = string.Format("{0}{1}", GetPulseRelativeContentUrl(), EmbedParameters.Count == 0 ? string.Empty : "?" + EmbedParameters.ToQueryString(Settings));

            return string.Format(
                "<script src=\"{0}/scripts/sdk/embed.js\" ></script>" +
                "<script type=\"text/javascript\">" +
                    "var embed = Object.create(PulseEmbed);" +
                    "embed.init({{" +
                        "baseUrl: '{0}'," +
                        "embedUrl: '{1}'," +
                        "overlay: true," +
                        "width: \"100%\"" +
                    "}});" +
                    "</script>", Settings.PulseBaseUrl, embedUrl);
        }

        protected override EmbedParameterList GetEmbedParameters()
        {
            var parameters = new EmbedParameterList();

            if (!string.IsNullOrEmpty(Settings.CustomCss))
            {
                parameters.Set(new EmbedParameter(ParameterKey.CustomCss, Settings.CustomCss));
            }

            var pulseUrl = PulseUtility.GetPulseRelativeUrl(this.Context.Request["pulseurl"]);
            if (!string.IsNullOrEmpty(pulseUrl))
            {
                parameters.Set(new EmbedParameter(ParameterKey.PulseUrl, pulseUrl, System.Uri.EscapeDataString));
            }

            return parameters;
        }
    }
}