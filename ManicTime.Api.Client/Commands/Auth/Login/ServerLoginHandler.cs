using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;

namespace ManicTime.Api.Client.Commands.Auth.Login
{
    public static class ServerLoginHandler
    {
        public static async Task HandleAsync(string serverUrl, string username, string password)
        {
            using HttpClient client = new HttpClient();

            var tokenEndpointHref = await GetServerTokenEndpointUrlAsync(client, serverUrl);

            username ??= ConsoleHelper.ReadValue("Username: ");
            password ??= ConsoleHelper.ReadMaskedValue("Password: ");

            var tokenEndpointRequestContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
            });
            using HttpRequestMessage tokenEndpointRequest = new HttpRequestMessage(HttpMethod.Post, tokenEndpointHref)
            {
                Headers = { Accept = { new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json) } },
                Content = tokenEndpointRequestContent
            };
            using HttpResponseMessage tokenEndpointResponse = await client.SendAsync(tokenEndpointRequest);
            if (tokenEndpointResponse.StatusCode == HttpStatusCode.BadRequest)
            {
                JsonElement error = await tokenEndpointResponse.Content.ReadFromJsonAsync<JsonElement>();
                if (string.Equals(error.GetProperty("error").GetString(), "invalid_grant", StringComparison.OrdinalIgnoreCase))
                    throw new HttpRequestException("Invalid username or password", null, HttpStatusCode.BadRequest);
            }

            tokenEndpointResponse.EnsureSuccessStatusCode();
            JsonElement tokens = await tokenEndpointResponse.Content.ReadFromJsonAsync<JsonElement>();

            Console.WriteLine(tokens.FormatForDisplay());
        }

        private static async Task<string> GetServerTokenEndpointUrlAsync(HttpClient client, string serverUrl)
        {
            using HttpResponseMessage serverResponse = await client.GetAsync(serverUrl);

            ServerAuthCheck.AssertOAuthConfiguredOnServer(serverResponse);

            JsonElement home = await serverResponse.Content.ReadFromJsonAsync<JsonElement>();
            string tokenEndpointHref = home.GetLinkHref("manictime/token");
            if (tokenEndpointHref == null)
                throw new InvalidOperationException("Token endpoint url not found.");
            return tokenEndpointHref;
        }
    }
}
