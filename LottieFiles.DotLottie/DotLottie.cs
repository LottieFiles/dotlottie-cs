using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LottieFiles.DotLottie
{
    public class DotLottie
    {
        private DotLottie()
        {

        }

        //
        public Manifest Manifest { get; private set; }
        public Dictionary<string, MemoryStream> Animations { get; private set; } = new Dictionary<string, MemoryStream>();

        //todo: expose this as a helper method or override for simplicity
        public KeyValuePair<string, string> FirstAnimation()
        {
            var animationStream = Animations.First();
            var animationText = Encoding.UTF8.GetString(animationStream.Value.ToArray());

            return new KeyValuePair<string, string>(animationStream.Key, animationText);
        }

        public Dictionary<string, byte[]> Images { get; private set; } = new Dictionary<string, byte[]>();

        public static async Task<DotLottie> OpenAsync(Stream fileStream)
        {
            var dotLottie = new DotLottie();

            var archive = new ZipArchive(fileStream, ZipArchiveMode.Read, true);

            var manifestEntry = archive.GetEntry(Consts.Manifest);
            var animationEntries = archive.Entries.Where(x => x.FullName.StartsWith(Consts.Animations));
            var imageEntries = archive.Entries.Where(x => x.FullName.StartsWith(Consts.Images));

            if (manifestEntry != null)
            {
                using (var stream = manifestEntry.Open())
                {
                    var manifest = await JsonSerializer.DeserializeAsync<Manifest>(stream, Options.JsonSerializerOptions);
                    dotLottie.Manifest = manifest;
                }
            }

            foreach (var animationEntry in animationEntries)
            {
                using (var stream = animationEntry.Open())
                {
                    var ms = new MemoryStream();

                    //todo: just use the stream, don't clone it.
                    stream.CopyTo(ms);
                    ms.Position = 0;
                    dotLottie.Animations.Add(animationEntry.Name, ms);
                }
            }

            return dotLottie;
        }
    }
}