namespace LottieFiles.IO
{
    static class DataUriHelpers
    {
        public static bool IsDataUri(this string filePath)
        {
            return filePath.StartsWith("data:");
        }
    }
}