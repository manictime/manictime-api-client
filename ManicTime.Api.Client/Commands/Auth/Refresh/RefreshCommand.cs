using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ManicTime.Api.Client.Commands.Auth.Refresh
{
    public class RefreshCommand : Command
    {
        public RefreshCommand() : base("refresh", "Refresh tokens [cloud only]")
        {
            AddOption(new Option<string>("--client-id", "Client ID"));
            AddOption(new Option<string>("--client-secret", "Client secret"));
            AddOption(new Option<string>("--refresh-token", "Refresh token"));
            
            AddValidator(new CommandValidator()
                .AddAllRequired("--client-id", "--client-secret", "--refresh-token")
                .Validate);
            
            Handler = CommandHandler.Create<string, string, string>(HandleAsync);
        }

        private static async Task HandleAsync(string clientId, string clientSecret, string refreshToken)
        {
            using HttpClient client = HttpClientFactory.Create();

            JsonElement configuration = await client.GetAsJsonAsync(Urls.OpenIdConnectConfiguration);
            
            var tokenEndpointPostContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", refreshToken),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret)
            });
            JsonElement tokens = await client.PostAndReadAsJsonAsync(configuration.TokenEndpoint(), tokenEndpointPostContent);

            Console.WriteLine(tokens.FormatForDisplay());
        }
    }
}
