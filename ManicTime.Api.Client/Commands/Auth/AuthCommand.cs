using System.CommandLine;
using ManicTime.Api.Client.Commands.Auth.Login;
using ManicTime.Api.Client.Commands.Auth.Refresh;

namespace ManicTime.Api.Client.Commands.Auth
{
    public class AuthCommand : Command
    {
        public AuthCommand() : base("auth", "Login, get authorization tokens [cloud or server with ManicTime users only]")
        {
            AddCommand(new LoginCommand());
            AddCommand(new RefreshCommand());
        }
    }
}
