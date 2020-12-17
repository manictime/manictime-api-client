using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ManicTime.Api.Client.Commands.Auth.Login
{
    public static class ServerAuthCheck
    {
        public static void AssertOAuthConfiguredOnServer(HttpResponseMessage serverResponse)
        {
            if (serverResponse.StatusCode != HttpStatusCode.Unauthorized)
                throw new InvalidOperationException($"Invalid server response: {serverResponse.StatusCode}");

            string[] authenticateHeaders = GetAuthenticationTypes(serverResponse.Headers);

            if (IsWindowsAuthentication(authenticateHeaders))
                throw new InvalidOperationException("Server is configured with Windows authentication. Login not required.");
            if (!IsOAuthAuthentication(authenticateHeaders))
                throw new InvalidOperationException($"Unknown authentication method: {serverResponse.Headers.WwwAuthenticate}");
        }

        private static string[] GetAuthenticationTypes(HttpResponseHeaders headers)
        {
            return headers.TryGetValues("WWW-Authenticate", out var authenticateValues)
                ? authenticateValues.Select(GetAuthenticationType).ToArray()
                : new string[0];
        }

        private static string GetAuthenticationType(string wwwAuthenticateHeaderValue)
        {
            int pos = wwwAuthenticateHeaderValue.IndexOfAny(new[] { ' ', ',' });
            return pos == -1
                ? wwwAuthenticateHeaderValue.Trim().ToLowerInvariant()
                : wwwAuthenticateHeaderValue.Substring(0, pos).Trim().ToLowerInvariant();
        }

        private static bool IsWindowsAuthentication(string[] authenticateHeaders) =>
            authenticateHeaders.Any(h =>
                h.Equals("ntlm", StringComparison.OrdinalIgnoreCase) || h.Equals("negotiate", StringComparison.OrdinalIgnoreCase));

        private static bool IsOAuthAuthentication(string[] authenticateHeaders) =>
            authenticateHeaders.Any(h => h.Equals("bearer", StringComparison.OrdinalIgnoreCase));
    }
}
