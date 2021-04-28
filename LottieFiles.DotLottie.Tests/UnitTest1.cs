using System;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

namespace LottieFiles.IO.Tests
{
    public class Tests
    {
        Uri sampleDotLottie = new Uri("https://dotlottie.io/sample_files/animation.lottie");
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
        public async Task Open_Uri_Async()
        {
            //Try parse a lottie            
            DotLottie lottie = await DotLottie.OpenAsync(sampleDotLottie);

            Assert.Pass();
        }

        [Test]
        public async Task Open_Uri_Async_Fail_Bad_Uri()
        {
            Assert.CatchAsync(async () =>
            {
                DotLottie lottie = await DotLottie.OpenAsync(sampleFakeDotLottie);
            });            
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

        [Test]
        public void Fail_To_Create_Manifest()
        {
            
        }
    }
}