using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace ManicTime.Api.Client.Commands
{
    public static class DisplayExtensions
    {
        private static readonly JsonSerializerOptions JsonDisplayOptions = new()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public static string FormatForDisplay(this object value) =>
            Encoding.UTF8.GetString(JsonSerializer.SerializeToUtf8Bytes(value, JsonDisplayOptions));
    }
}