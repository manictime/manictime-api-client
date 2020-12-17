using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;

namespace ManicTime.Api.Client.Commands
{
    public static class HttpClientExtensions
    {
        public static async Task<JsonElement> GetAsJsonAsync(this HttpClient client, string url, string accept = MediaTypeNames.Application.Json)
        {
            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url)
                {Headers = {Accept = {new MediaTypeWithQualityHeaderValue(accept)}}};
            return await client.SendAndReadAsJsonAsync(request);
        }

        public static async Task<JsonElement> PostAndReadAsJsonAsync(this HttpClient client, string url, HttpContent content, string accept = MediaTypeNames.Application.Json)
        {
            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Headers = {Accept = {new MediaTypeWithQualityHeaderValue(accept)}},
                Content = content
            };
            return await client.SendAndReadAsJsonAsync(request);
        }
        
        public static async Task<JsonElement> SendAndReadAsJsonAsync(this HttpClient client, HttpRequestMessage request)
        {
            using HttpResponseMessage response = (await client.SendAsync(request)).EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<JsonElement>();
        }

        public static async Task<string> GetCloudServerUrlAsync(this HttpClient client)
        {
            JsonElement profile = await client.GetAsJsonAsync(Urls.Profile, ManicTimeMediaType.V3);
            return Urls.Home(profile.GetProperty("serverUrl").GetString());
        }
    }
}
