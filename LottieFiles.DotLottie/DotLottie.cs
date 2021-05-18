using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LottieFiles.IO
{
    public sealed partial class DotLottie
    {
        private DotLottie()
        {

        }

        /// <summary>
        /// The manifest
        /// </summary>
        public Manifest Manifest { get; private set; }

        /// <summary>
        /// A collection of the animations
        /// </summary>
        public Dictionary<string, MemoryStream> Animations { get; private set; } = new Dictionary<string, MemoryStream>();

        /// <summary>
        /// A collection of the images used by the animations
        /// </summary>
        public Dictionary<string, MemoryStream> Images { get; private set; } = new Dictionary<string, MemoryStream>();

        /// <summary>
        /// Open a given lottie file 
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public static DotLottie Open(Stream fileStream)
        {
            var dotLottie = new DotLottie();

            var archive = new ZipArchive(fileStream, ZipArchiveMode.Read, true);

            var manifestEntry = archive.GetEntry(Consts.Manifest);
            var animationEntries = archive.Entries.Where(x => x.FullName.StartsWith(Consts.Animations));
            var imageEntries = archive.Entries.Where(x => x.FullName.StartsWith(Consts.Images));

            if (manifestEntry != null)
            {
                using (var stream = manifestEntry.Open())
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    var json = streamReader.ReadToEnd();
                    var manifest = JsonSerializer.Deserialize<Manifest>(json, Options.JsonSerializerOptions);
                    dotLottie.Manifest = manifest;
                }
            }

            ExtractAnimations(dotLottie, animationEntries);
            ExtractImages(dotLottie, imageEntries);

            return dotLottie;
        }

        /// <summary>
        /// Open a given lottie file
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns></returns>
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

            //todo: asyncify
            ExtractAnimations(dotLottie, animationEntries);
            ExtractImages(dotLottie, imageEntries);

            return dotLottie;
        }

        /// <summary>
        /// Download and open a valid dotlottie Uri synchronously
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>DotLottie object model</returns>
        public static DotLottie Open(Uri uri)
        {
            if (!uri.ToString().EndsWith(".lottie"))
                throw new ArgumentException("File must be a .lottie");

            var openAsyncTask = Task.Run(() => OpenAsync(uri));
            return openAsyncTask.Result;
        }

        /// <summary>
        /// Download and open a valid dotlottie Uri asynchronously
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>DotLottie object model</returns>
        public static async Task<DotLottie> OpenAsync(Uri uri)
        {
            if (!uri.ToString().EndsWith(".lottie"))
                throw new ArgumentException("File must be a .lottie");

            using (var client = new HttpClient())
            {
                //todo: there's an async downloaddata but it's a bit more tricky to use
                var data = await client.GetByteArrayAsync(uri);
                MemoryStream stream = new MemoryStream(data);
                return await OpenAsync(stream);
            }
        }

        public void AddAnimation(string fileName, bool loop, float speed, string themeColor, string json)
        {
            var fileExtension = Path.GetExtension(fileName);
            var nakedFileName = Path.GetFileNameWithoutExtension(fileName);
            
            Manifest.Animations.Add(new Animation
            {
                Id = nakedFileName,
                Loop = loop,
                Speed = speed,
                ThemeColor = themeColor
            });

            var ms = new MemoryStream();
            using (var sw = new StreamWriter(ms, Encoding.UTF8, 2048, true))
            {
                sw.Write(json);
                sw.Flush();
            }
            ms.Position = 0;

            Animations.Add(fileName, ms);
        }

        private void ConvertInlineAssetToExternal(string animationId, string assetId)
        {
            throw new NotImplementedException();

            if (!Animations.TryGetValue(animationId, out MemoryStream ms))
            {
                throw new ArgumentException(nameof(animationId));
            }

            var json = new StreamReader(ms, true).ReadToEnd();

            //Parse the images from the lottie
            var dom = JsonSerializer.Deserialize<LottieDOM.Lottie>(json);

            foreach (var asset in dom.Assets)
            {
                if (asset.ImagePath.IsDataUri())
                {
                    //Extract data, filename

                    //save in folder

                    //replace entry to point at folder
                }
            }
        }

        internal void AddImage(string filename, byte[] data)
        {

        }

        //RemoveAnimation
        //RemoveImage

        /// <summary>
        /// Returns the current dotlottie as a byte array.
        /// </summary>
        /// <returns></returns>
        public byte[] GetData()
        {
            var ms = new MemoryStream();

            var archive = new ZipArchive(ms, ZipArchiveMode.Update, true);

            //Manifest
            var manifestEntry = archive.CreateEntry(Consts.Manifest);
            using (var stream = manifestEntry.Open())
            using (StreamWriter streamWriter = new StreamWriter(stream))
            {
                var json = JsonSerializer.Serialize(Manifest, Options.JsonSerializerOptions);
                streamWriter.Write(json);
            }

            //Images
            foreach (var image in Images)
            {
                var imageEntry = archive.CreateEntry($"{Consts.Images}/{image.Key}");
                using (var imageArchiveStream = imageEntry.Open())
                {
                    image.Value.CopyTo(imageArchiveStream);
                }
            }

            //Animations
            foreach (var animation in Animations)
            {
                var animationEntryName = $"{Consts.Animations}/{animation.Key}";
                var animationEntry = archive.CreateEntry(animationEntryName);
                using (var animationArchiveStream = animationEntry.Open())
                {
                    animation.Value.CopyTo(animationArchiveStream);
                }
            }

            archive.Dispose();

            var buffer = ms.ToArray();
            return buffer;
        }

        private static void ExtractAnimations(DotLottie dotLottie, IEnumerable<ZipArchiveEntry> animationEntries)
        {
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
        }

        private static void ExtractImages(DotLottie dotLottie, IEnumerable<ZipArchiveEntry> imageEntries)
        {
            foreach (var imageEntry in imageEntries)
            {
                using (var stream = imageEntry.Open())
                {
                    var ms = new MemoryStream();

                    stream.CopyTo(ms);
                    ms.Position = 0;
                    dotLottie.Images.Add(imageEntry.Name, ms);
                }
            }
        }
    }
}