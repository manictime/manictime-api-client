using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Text.Json;
using System.Threading.Tasks;

namespace ManicTime.Api.Client.Commands.Get.Profile
{
    public class GetProfileCommand : Command
    {
        public GetProfileCommand() : base("profile", "Get user profile [cloud only]")
        {
            AddOption(new Option<string>("--access-token", "Access token [required]"));

            AddValidator(new CommandValidator()
                .AddAllRequired("--access-token")
                .Validate);

            Handler = CommandHandler.Create<string>(HandleAsync);
        }

        private static async Task HandleAsync(string accessToken)
        {
            using var client = HttpClientFactory.Create(accessToken: accessToken);

            JsonElement profile = await client.GetAsJsonAsync(Urls.Profile);

            Console.WriteLine(profile.FormatForDisplay());
        }
    }
}
