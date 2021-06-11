using System.Collections.Generic;

namespace LottieFiles.IO
{
    public sealed class Manifest
    {
        /// <summary>
        /// Name and version of the software that created the dotLottie
        /// </summary>
        public string Generator { get; set; }

        /// <summary>
        /// Target dotLottie version
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Revision version number of the dotLottie
        /// </summary>
        public int Revision { get; set; }

        /// <summary>
        /// Name of the author
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// List of animations
        /// </summary>
        public List<Animation> Animations { get; set; }
    }
}
