using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Text.Json;
using System.Threading.Tasks;

namespace ManicTime.Api.Client.Commands.Get.Timelines
{
    public class GetTimelinesCommand : Command
    {
        public GetTimelinesCommand() : base("timelines", "Display timelines")
        {
            this.AddApiAuthOptions();
            AddOption(new Option<string>("--timeline-key", "Timeline key [optional]"));

            AddValidator(new CommandValidator()
                .AddApiAuthValidators()
                .Validate);

            Handler = CommandHandler.Create<string, string, string, string, string>(HandleAsync);
        }

        private static async Task HandleAsync(string serverUrl, string accessToken, string username, string password, string timelineKey)
        {
            using var client = HttpClientFactory.Create(username, password, accessToken);
            serverUrl ??= await client.GetCloudServerUrlAsync();

            JsonElement home = await client.GetAsJsonAsync(serverUrl, ManicTimeMediaType.V3);
            JsonElement timelines = await client.GetAsJsonAsync(Urls.Timelines(home.GetLinkHref("manictime/timelines"), timelineKey), ManicTimeMediaType.V3);

            Console.WriteLine(timelines.FormatForDisplay());
        }
    }
}
