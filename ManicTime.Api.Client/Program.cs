using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.Threading.Tasks;
using ManicTime.Api.Client.Commands;
using ManicTime.Api.Client.Commands.Auth;
using ManicTime.Api.Client.Commands.Get;

namespace ManicTime.Api.Client
{
    public static class Program
    {
        static Task Main(string[] args) =>
            new CommandLineBuilder()
                .UseDefaults()
                .UseMiddleware(ErrorHandlingMiddleware.Invoke)
                .AddCommand(new AuthCommand())
                .AddCommand(new GetCommand())
                .Build()
                .InvokeAsync(args);
    }
}