using System;
using System.Diagnostics;

namespace ManicTime.Api.Client.Commands.Auth.Login
{
    public static class Browser
    {
        public static void OpenLoginPage(string authorizationEndpoint, string nonce, string state, string codeChallenge, string clientId, string redirectUri)
        {
            string authorizationUrl = authorizationEndpoint +
                "?response_type=code" +
                $"&nonce={Uri.EscapeDataString(nonce)}" +
                $"&state={Uri.EscapeDataString(state)}" +
                $"&code_challenge={Uri.EscapeDataString(codeChallenge)}" +
                "&code_challenge_method=S256" +
                $"&client_id={Uri.EscapeDataString(clientId)}" +
                "&scope=openid+profile+manictimeapi+offline_access" +
                $"&redirect_uri={Uri.EscapeDataString(redirectUri)}";
            Process.Start(new ProcessStartInfo { FileName = authorizationUrl, UseShellExecute = true });
        }
    }
}