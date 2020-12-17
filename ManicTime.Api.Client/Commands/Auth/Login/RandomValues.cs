using System.Security.Cryptography;
using Microsoft.AspNetCore.WebUtilities;

namespace ManicTime.Api.Client.Commands.Auth.Login
{
    public static class RandomValues
    {
        private static readonly RandomNumberGenerator _randomNumberGenerator = RandomNumberGenerator.Create();

        public static string CreateNonce() =>
            Base64UrlTextEncoder.Encode(CreateRandomKey(16));

        public static string CreateState() =>
            Base64UrlTextEncoder.Encode(CreateRandomKey(16));

        public static string CreateCodeVerifier() =>
            Base64UrlTextEncoder.Encode(CreateRandomKey(32));

        private static byte[] CreateRandomKey(int length)
        {
            var bytes = new byte[length];
            _randomNumberGenerator.GetBytes(bytes);
            return bytes;
        }
    }
}