using System;
using System.Linq;

namespace ManicTime.Api.Client.Commands
{
    public static class Urls
    {
        public static readonly string DefaultCallback = "http://127.0.0.1:4040";

        public static readonly string Auth = "https://login.manictime.com";
        
        public static readonly string Profile = "https://api.manictime.com/v1/profile";

        public static readonly string OpenIdConnectConfiguration = Combine(Auth, ".well-known/openid-configuration");
        
        public static string Home(string serverUrl) =>
            Combine(serverUrl, "api");

        public static string Timelines(string timelinesHref, string timelineKey) =>
            timelineKey == null
                ? timelinesHref
                : $"{timelinesHref}?timelineKey={timelineKey}";

        public static string Activities(string activitiesHref, DateTime fromTime, DateTime toTime) => 
            $"{activitiesHref}?fromTime={fromTime:o}&toTime={toTime:o}";

        public static string Combine(params string[] urls) =>
            urls.Aggregate((combined, url) => combined == null || combined.EndsWith('/') ? string.Concat(combined, url) : string.Concat(combined, "/", url));
    }
}
