using System;
using System.Text;
using System.Text.Json.Serialization;

namespace LottieFiles.IO.LottieDOM
{
    internal class Asset
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("w")]
        public int Width { get; set; }

        [JsonPropertyName("h")]
        public int Height { get; set; }

        [JsonPropertyName("u")]
        public string u { get; set; }

        [JsonPropertyName("p")]
        public string ImagePath { get; set; }               

        [JsonPropertyName("e")]
        public int e { get; set; }
    }

    internal class ImageDataHelper
    {
        public string GetMimeType(string imageData)
        {
            var parts = imageData.Split(',');
            if(parts.Length == 0)
            {
                throw new ArgumentException($"{nameof(imageData)} is not valid");
            }

            //eg: data:image/png;base64
            var mimeData = parts[0];
            
            //eg: image/png
            var mimeType = mimeData.Replace("data:","").Split(';')[0];
            
            //eg: png
            var imageType = mimeType.Split('/')[1];
            
            //eg: base64 image data
            var data = parts[1];

            return imageType;
        }
    }
}
