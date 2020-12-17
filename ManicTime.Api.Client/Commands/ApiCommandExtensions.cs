using System.CommandLine;

namespace ManicTime.Api.Client.Commands
{
    public static class ApiCommandExtensions
    {
        public static void AddApiAuthOptions(this Command command)
        {
            command.AddOption(new Option<string>("--server-url", "Server URL [server only, required]"));
            command.AddOption(new Option<string>("--access-token", "Access token [cloud or server with ManicTime users only, required]"));
            command.AddOption(new Option<string>("--username", "Username (server with Windows users only, optional)"));
            command.AddOption(new Option<string>("--password", "Password (server with Windows users only, optional)"));
        }
    }
}