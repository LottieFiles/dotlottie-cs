using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace LottieFiles.IO.Tests
{
    public class Tests
    {
        Uri sampleDotLottie = new Uri("https://dotlottie.io/sample_files/animation.lottie");
        Uri sampleLottieInlineImage = new Uri("https://dotlottie.io/sample_files/animation-inline-image.json");
        Uri sampleFakeDotLottie = new Uri("https://dotlottie.io/sample_files/animation.shottie");

        [SetUp]
        public void Setup()
        {
            //
        }

        [Test]
        public async Task Open_File_Async()
        {
            //Try parse a lottie
            var stream = File.OpenRead("Lotties/animation-external-image.lottie");
            DotLottie lottie = await DotLottie.OpenAsync(stream);

            Assert.Pass();
        }

        [Test]
        public async Task Open_Uri_Sync()
        {
            //Try parse a lottie            
            DotLottie lottie = DotLottie.Open(sampleDotLottie);

            Assert.Pass();
        }


        [Test]
        public async Task Open_Uri_Async()
        {
            //Try parse a lottie            
            DotLottie lottie = await DotLottie.OpenAsync(sampleDotLottie);

            Assert.Pass();
        }

        [Test]
        public void Open_Uri_Async_Fail_Bad_Uri()
        {
            Assert.CatchAsync(async () =>
            {
                DotLottie lottie = await DotLottie.OpenAsync(sampleFakeDotLottie);
            });            
        }

        [Test]
        public async Task Add_Animation_Check_Images_Parsed()
        {
            var lottie = await DotLottie.OpenAsync(sampleDotLottie);
            var filename = "Lotties/animation-inline-image.json";
            var jsonLottie = File.ReadAllText(filename);
            
            var localFileName = Path.GetFileName(filename);
            var nakedFileName = Path.GetFileNameWithoutExtension(localFileName);

            lottie.AddAnimation(localFileName, true, 1.5f, "ffffff", jsonLottie);

            Assert.True(lottie.Animations.ContainsKey(localFileName));
            Assert.True(lottie.Manifest.Animations.Any(x => x.Id == nakedFileName));
        }

        [Test]
        public async Task Temp_Open_Dotlottie_And_Save()
        {
            var stream = File.OpenRead("Lotties/animation-external-image.lottie");
            var ms = new MemoryStream();
            stream.CopyTo(ms);

            DotLottie lottie = await DotLottie.OpenAsync(stream);
            var existingData = ms.GetBuffer();
            var exportedData = lottie.GetData();
            File.WriteAllBytes("temp.lottie", exportedData);
        }
    }
}