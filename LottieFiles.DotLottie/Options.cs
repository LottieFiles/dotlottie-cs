using System.Text.Json;

namespace LottieFiles.DotLottie
{
    internal static class Options
    {
        public static JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }
}
