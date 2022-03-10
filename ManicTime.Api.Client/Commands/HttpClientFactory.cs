using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;

namespace ManicTime.Api.Client.Commands
{
    public static class HttpClientFactory
    {
        public static HttpClient Create(string username = null, string password = null, string accessToken = null)
        {
            var handler = new HttpClientHandler();

            var client = new HttpClient(handler) {Timeout = Timeout.InfiniteTimeSpan};
            if (accessToken == null)
                handler.Credentials = username == null 
                    ? CredentialCache.DefaultCredentials 
                    : new NetworkCredential(username, password ?? ConsoleHelper.ReadMaskedValue("Pasword: "));
            else
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            return client;
        }
    }
}


