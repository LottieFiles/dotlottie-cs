using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LottieFiles.IO
{
    public static class DotLottieHelpers
    {
        //todo: expose this as a helper method or override for simplicity
        public static KeyValuePair<string, string> FirstAnimation(this DotLottie dotLottie)
        {
            var animationStream = dotLottie.Animations.First();
            var animationText = Encoding.UTF8.GetString(animationStream.Value.ToArray());

            return new KeyValuePair<string, string>(animationStream.Key, animationText);
        }

        public static List<string> Validate(this DotLottie dotLottie)
        {
            var errors = new List<string>();

            //todo: Check 

            return errors;
        }
    }
}