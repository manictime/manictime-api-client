using System.CommandLine;
using ManicTime.Api.Client.Commands.Get.Activities;
using ManicTime.Api.Client.Commands.Get.Home;
using ManicTime.Api.Client.Commands.Get.Profile;
using ManicTime.Api.Client.Commands.Get.Timelines;

namespace ManicTime.Api.Client.Commands.Get
{
    public class GetCommand : Command
    {
        public GetCommand() : base("get", "Get resources")
        {
            AddCommand(new GetActivitiesCommand());
            AddCommand(new GetHomeCommand());
            AddCommand(new GetProfileCommand());
            AddCommand(new GetTimelinesCommand());
        }
    }
}
