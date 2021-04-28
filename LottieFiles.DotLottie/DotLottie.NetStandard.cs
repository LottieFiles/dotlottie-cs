using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace LottieFiles.IO
{
    public sealed partial class DotLottie
    {
        public static DotLottie Open(Uri uri)
        {
            if (!uri.ToString().EndsWith(".lottie"))
                throw new ArgumentException("File must be a .lottie");

            using (var client = new WebClient())
            {
                var data = client.DownloadData(uri);
                MemoryStream stream = new MemoryStream(data);
                return Open(stream);
            }
        }

        public static async Task<DotLottie> OpenAsync(Uri uri)
        {
            if (!uri.ToString().EndsWith(".lottie"))
                throw new ArgumentException("File must be a .lottie");

            using (var client = new WebClient())
            {
                //todo: there's an async downloaddata but it's a bit more tricky to use
                var data = client.DownloadData(uri);
                MemoryStream stream = new MemoryStream(data);
                return await OpenAsync(stream);
            }                       
        }        
    }
}