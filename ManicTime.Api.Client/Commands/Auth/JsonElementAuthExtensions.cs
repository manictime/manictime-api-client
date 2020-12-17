using System.Text.Json;

namespace ManicTime.Api.Client.Commands.Auth
{
    public static class JsonElementAuthExtensions
    {
        public static string AuthorizationEndpoint(this JsonElement configuration) =>
            configuration.GetProperty("authorization_endpoint").GetString();

        public static string TokenEndpoint(this JsonElement configuration) =>
            configuration.GetProperty("token_endpoint").GetString();
    }
}
