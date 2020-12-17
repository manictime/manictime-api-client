using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Text.Json;
using System.Threading.Tasks;

namespace ManicTime.Api.Client.Commands.Get.Home
{
    public class GetHomeCommand : Command
    {
        public GetHomeCommand() : base("home", "Get server home")
        {
            this.AddApiAuthOptions();

            AddValidator(new CommandValidator()
                .AddApiAuthValidators()
                .Validate);

            Handler = CommandHandler.Create<string, string, string, string>(HandleAsync);
        }

        private static async Task HandleAsync(string serverUrl, string accessToken, string username, string password)
        {
            using var client = HttpClientFactory.Create(username, password, accessToken);
            serverUrl ??= await client.GetCloudServerUrlAsync();

            JsonElement home = await client.GetAsJsonAsync(serverUrl, ManicTimeMediaType.V3);

            Console.WriteLine(home.FormatForDisplay());
        }
    }
}
