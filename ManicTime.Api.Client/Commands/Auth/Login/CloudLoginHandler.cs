using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;

namespace ManicTime.Api.Client.Commands.Auth.Login
{
    public static class CloudLoginHandler
    {
        public static async Task HandleAsync(string clientId, string clientSecret, string callbackUrl)
        {
            string nonce = RandomValues.CreateNonce();
            string state = RandomValues.CreateState();
            string codeVerifier = RandomValues.CreateCodeVerifier();
            string codeChallenge = Base64UrlTextEncoder.Encode(SHA256.HashData(Encoding.UTF8.GetBytes(codeVerifier)));
            callbackUrl ??= Urls.DefaultCallback;

            using HttpClient client = HttpClientFactory.Create();

            JsonElement configuration = await client.GetAsJsonAsync(Urls.OpenIdConnectConfiguration);

            Task<string> waitForAuthorizationCodeTask = await AuthorizationCodeCallbackListener.StartAsync(callbackUrl, state);

            Browser.OpenLoginPage(configuration.AuthorizationEndpoint(), nonce, state, codeChallenge, clientId, callbackUrl);

            string authorizationCode = await waitForAuthorizationCodeTask;

            var tokenEndpointPostContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("code", authorizationCode),
                new KeyValuePair<string, string>("redirect_uri", callbackUrl),
                new KeyValuePair<string, string>("code_verifier", codeVerifier),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret)
            });
            JsonElement tokens = await client.PostAndReadAsJsonAsync(configuration.TokenEndpoint(), tokenEndpointPostContent);

            Console.WriteLine(tokens.FormatForDisplay());
        }
    }
}
