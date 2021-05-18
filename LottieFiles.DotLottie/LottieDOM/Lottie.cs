using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LottieFiles.IO.LottieDOM
{
    internal class Lottie
    {
        [JsonPropertyName("v")]
        public string Version { get;  set; }

        [JsonPropertyName("meta")]
        public Meta Meta { get;  set; }

        [JsonPropertyName("fr")]
        public int FramesPerSecond { get;  set; }

        [JsonPropertyName("ip")]
        public int InPoint { get;  set; }

        [JsonPropertyName("op")]
        public int OutPoint { get;  set; }

        [JsonPropertyName("w")]
        public int Width { get;  set; }

        [JsonPropertyName("h")]
        public int Height { get;  set; }

        [JsonPropertyName("nm")]
        public string Name { get;  set; }

        [JsonPropertyName("ddd")]
        public int ddd { get;  set; }

        [JsonPropertyName("assets")]
        public List<Asset> Assets { get;  set; } = new List<Asset>();

        //Layers
        //Markers
    }

    //public class Rootobject
    //{
    //    public string v { get; set; }
    //    public Meta meta { get; set; }
    //    public int fr { get; set; }
    //    public int ip { get; set; }
    //    public int op { get; set; }
    //    public int w { get; set; }
    //    public int h { get; set; }
    //    public string nm { get; set; }
    //    public int ddd { get; set; }
    //    public Asset[] assets { get; set; }
    //    public Layer[] layers { get; set; }
    //    public object[] markers { get; set; }
    //}
}
