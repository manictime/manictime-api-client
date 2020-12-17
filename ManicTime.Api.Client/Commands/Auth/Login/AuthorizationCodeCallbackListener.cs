using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace ManicTime.Api.Client.Commands.Auth.Login
{
    public static class AuthorizationCodeCallbackListener
    {
        public static async Task<Task<string>> StartAsync(string redirectUri, string state)
        {
            TaskCompletionSource<string> authorizationCodeTcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
            IWebHost host = null;
            host = WebHost
                .CreateDefaultBuilder()
                .ConfigureLogging(config => config.ClearProviders())
                .UseUrls(redirectUri)
                .Configure(app =>
                    app.UseRouter(rb => rb.MapGet("/", context =>
                    {
                        if (context.Request.Query["state"] == state)
                        {
                            context.Response.WriteAsync("Login success");
                            string authorizationCode = context.Request.Query["code"];
                            authorizationCodeTcs.SetResult(authorizationCode);
                        }
                        else
                        {
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                            context.Response.WriteAsync("Login error");
                            authorizationCodeTcs.SetException(new InvalidOperationException("Authorization code received with invalid state"));
                        }
                        host.StopAsync().ContinueWith(_ => host.Dispose());
                        return Task.CompletedTask;
                    })))
                .Build();
            await host.StartAsync();
            return authorizationCodeTcs.Task;
        }
    }
}
