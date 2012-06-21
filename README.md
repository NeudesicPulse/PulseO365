PulseO365
=========

Office 365 web parts for Pulse

You must change the Pulse settings values in the Neudesic.Pulse.SharePoint.Online\Constants\SettingsValue.cs file to fit your Pulse environment.

Settings Values
---------------
PulseBaseUrl - Required. The full URL to your Pulse server (ex. https://pulse.mycompany.com).
ParentSystemFeedId - Required. The feed ID of the parent system in Pulse. This URL can be found by browsing to the parent System in Pulse and copying the GUID from the URL (ex. from System URL https://pulse.mycompany.com/streams/6dcdb3a9-e857-4aa0-a8cd-5bd33050d228/activities the ParentSystemFeedId = 6dcdb3a9-e857-4aa0-a8cd-5bd33050d228).
CustomCss - Optional. The full URL to a CSS file to override Pulse styles.
