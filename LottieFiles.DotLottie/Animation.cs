namespace LottieFiles.DotLottie
{
    public class Animation
    {
        /// <summary>
        /// Name of the Lottie animation file without .json
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Desired playback speed
        /// </summary>
        public float Speed { get; set; }

        /// <summary>
        /// Theme color
        /// </summary>
        public string ThemeColor { get; set; }

        /// <summary>
        /// Whether to loop or not
        /// </summary>
        public bool Loop { get; set; }
    }
}
