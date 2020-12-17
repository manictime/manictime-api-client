using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace ManicTime.Api.Client.Commands.Auth.Login
{
    public class LoginCommand : Command
    {
        public LoginCommand() : base("login", "Perform login [cloud or server with ManicTime users only]")
        {
            AddOption(new Option<string>("--client-id", "Client ID [cloud only, required]"));
            AddOption(new Option<string>("--client-secret", "Client secret [cloud only, required]"));
            AddOption(new Option<string>("--callback-url",
                $"Callback URL where mtapi will listen for authorization code (default: {Urls.DefaultCallback}) [cloud only, required]"));
            AddOption(new Option<string>("--server-url", "Server URL [server only, required]"));
            AddOption(new Option<string>("--username", "Username [server only, optional]"));
            AddOption(new Option<string>("--password", "Password [server only, optional]"));

            AddValidator(new CommandValidator()
                .AddAnyRequired("--client-id", "--server-url")
                .AddConditionalRequired("--client-id", "--client-secret")
                .AddConditionalNotAllowed("--client-id", "--server-url", "--username", "--password")
                .AddConditionalNotAllowed("--server-url", "--client-secret", "--callback-url")
                .Validate);

            Handler = CommandHandler.Create<string, string, string, string, string, string>(HandleAsync);
        }

        private Task HandleAsync(string clientId, string clientSecret, string callbackUrl, string serverUrl, string username, string password)
        {
            if (serverUrl == null)
                return CloudLoginHandler.HandleAsync(clientId, clientSecret, callbackUrl);
            return ServerLoginHandler.HandleAsync(serverUrl, username, password);
        }
    }
}