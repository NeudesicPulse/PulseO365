using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Neudesic.Pulse.SharePoint.Online;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using Microsoft.SharePoint.WebPartPages;
using System.IO;
using Microsoft.SharePoint.Utilities;

namespace Neudesic.Pulse.SharePoint.Online
{
    [ToolboxItemAttribute(false)]
    public abstract class PulseWebPartBase : System.Web.UI.WebControls.WebParts.WebPart
    {
        private Settings _settings;
        public Settings Settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = PulseUtility.GetSettings();
                }
                return _settings;
            }
        }

        private EmbedParameterList _embedParameters;
        public EmbedParameterList EmbedParameters
        {
            get
            {
                if (_embedParameters == null)
                {
                    _embedParameters = GetEmbedParameters();
                }
                return _embedParameters;
            }
        }

        protected abstract string GetPulseRelativeContentUrl();

        protected virtual EmbedParameterList GetEmbedParameters()
        {
            var parameters = new EmbedParameterList();

            parameters.Set(new EmbedParameter(ParameterKey.TargetElement, Guid.NewGuid().ToString()));
            
            if (!string.IsNullOrEmpty(Settings.CustomCss))
            {
                parameters.Set(new EmbedParameter(ParameterKey.CustomCss, Settings.CustomCss));
            }

            return parameters;
        }

        protected virtual ConfigurationErrorList Validate()
        {
            var errors = new ConfigurationErrorList();

            try
            {
                if (Settings == null || !Settings.IsValid())
                {
                    errors.Add(new ConfigurationError(
                        "Your site is not configured for Pulse.", 
                        "Ensure that Neudesic Pulse settings are correctly configured."));
                }
            }
            catch(Exception ex)
            {
                errors.Add(new ConfigurationError(
                    "An error occurred while validating your Pulse configuration",
                    string.Format("{0}. Ensure that Neudesic Pulse settings are correctly configured.", ex.Message)));
            }

            return errors;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            try
            {
                if (isDesignOrEditMode())
                {
                    writer.Write("Hiding your Pulse content because you are in Design mode.");
                }
                else if (!PulseUtility.IsContextValid(this.Context))
                {
                    writer.Write("Hiding your Pulse content because the SharePoint context is not set.");
                }
                else
                {
                    var validationErrors = Validate();
                    if (validationErrors.Count > 0)
                    {
                        writer.Write(string.Format("Pulse was unable to render this content for the following reason{0}:<br/>", validationErrors.Count > 1 ? "s" : string.Empty));
                        writer.Write(validationErrors.ToHtml());
                    }
                    else
                    {
                        writer.Write(GetEmbedContent());
                    }
                }
            }
            catch (Exception ex)
            {
                writer.Write(string.Format("Pulse was unable to render this content for the following reason:<br/><li><b>{0}</b></li>", ex.ToString()));
            }
        }

        protected virtual string GetEmbedContent()
        {
            var additionalScript = "<script type='text/javascript'>setInterval(function () { if (FixRibbonAndWorkspaceDimensionsForResize) { FixRibbonAndWorkspaceDimensionsForResize(); } }, 1000);</script>"; //SharePoint bug
            return string.Format("<div id='{0}'></div><script type='text/javascript' src='{1}'></script>{2}", EmbedParameters.Find(x => x.Key.Equals(ParameterKey.TargetElement)).Value, getEmbedUrl(), additionalScript);
        }

        protected string getEmbedUrl()
        {
            var baseUri = new Uri(Settings.PulseBaseUrl);
            var embedUri = new Uri(baseUri, GetPulseRelativeContentUrl());
            var embedUriBuilder = new UriBuilder(embedUri);

            EmbedParameters.MergeRange(EmbedParameterList.ParseQuery(embedUriBuilder.Query), true);
            embedUriBuilder.Query = EmbedParameters.ToQueryString(Settings);

            return embedUriBuilder.ToString();
        }

        private bool isDesignOrEditMode()
        {
            if (SPContext.Current.IsDesignTime ||
                Page.Request.Form["MSOLayout_InDesignMode"] == "1" ||
                string.Equals(Page.Request.Form["MSOSPWebPartManager_DisplayModeName"], "Design", StringComparison.InvariantCultureIgnoreCase) ||
                string.Equals(Page.Request.Form["MSOSPWebPartManager_DisplayModeName"], "Edit", StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
            else if (this.WebPartManager != null && this.WebPartManager.DisplayMode != null)
            {
                var displayMode = this.WebPartManager.DisplayMode.Name;
                return displayMode.Equals(SPWebPartManager.DesignDisplayMode.Name) || displayMode.Equals(SPWebPartManager.EditDisplayMode.Name);
            }
            else
            {
                return false;
            }
        }
    }
}