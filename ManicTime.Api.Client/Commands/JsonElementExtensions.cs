using System.Linq;
using System.Text.Json;

namespace ManicTime.Api.Client.Commands
{
    public static class JsonElementExtensions
    {
        public static string GetLinkHref(this JsonElement json, string rel)
        {
            if (json.TryGetProperty("links", out JsonElement links) && links.ValueKind == JsonValueKind.Array)
            {
                JsonElement link = links
                    .EnumerateArray()
                    .SingleOrDefault(l => l.TryGetProperty("rel", out JsonElement itemRel) && itemRel.ValueEquals(rel));
                if (link.ValueKind == JsonValueKind.Object)
                    return link.GetProperty("href").GetString();
            }

            return null;
        }
    }
}