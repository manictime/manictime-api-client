using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ManicTime.Api.Client.Commands.Get.Activities
{
    public class GetActivitiesCommand : Command
    {
        public GetActivitiesCommand() : base("activities", "Get activities")
        {
            this.AddApiAuthOptions();
            AddOption(new Option<string>("--timeline-key", "Timeline key [required]"));
            AddOption(new Option<DateTime>("--from-time", "From time [required]"));
            AddOption(new Option<DateTime>("--to-time", "To time [required]"));
            
            AddValidator(new CommandValidator()
                .AddApiAuthValidators()
                .AddAllRequired("--timeline-key", "--from-time", "--to-time")
                .Validate);
            
            Handler = CommandHandler.Create<string, string, string, string, string, DateTime, DateTime>(HandleAsync);
        }
        
        private static async Task HandleAsync(string serverUrl, string accessToken, string username, string password, string timelineKey, DateTime fromTime, DateTime toTime)
        {
            using var client = HttpClientFactory.Create(username, password, accessToken);
            serverUrl ??= await client.GetCloudServerUrlAsync();

            JsonElement home = await client.GetAsJsonAsync(serverUrl, ManicTimeMediaType.V3);
            JsonElement timelines = await client.GetAsJsonAsync(Urls.Timelines(home.GetLinkHref("manictime/timelines"), timelineKey), ManicTimeMediaType.V3);
            JsonElement timeline = timelines.GetProperty("timelines").EnumerateArray().SingleOrDefault();
            if (timeline.ValueKind == JsonValueKind.Undefined)
                throw new InvalidOperationException("Timeline not found.");
            JsonElement activities = await client.GetAsJsonAsync(Urls.Activities(timeline.GetLinkHref("manictime/activities"), fromTime, toTime), ManicTimeMediaType.V3);

            Console.WriteLine(activities.FormatForDisplay());
        }
    }
}
