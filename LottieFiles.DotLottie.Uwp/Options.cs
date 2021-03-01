using System.Text.Json;

namespace LottieFiles.IO
{
    internal static class Options
    {
        public static JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }
}
