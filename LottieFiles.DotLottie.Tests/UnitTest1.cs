using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

namespace LottieFiles.IO.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            //
        }

        [Test]
        public async Task Test1()
        {
            //Try parse a lottie
            var stream = File.OpenRead("Lotties/animation-external-image.lottie");
            DotLottie lottie = await DotLottie.OpenAsync(stream);


            Assert.Pass();
        }
    }
}