using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace DuplicateFileFinder
{
    public class ImageHashing
    {
        #region Private constants and utility methods
        /// <summary>
        /// Bitcounts array used for BitCount method (used in Similarity comparisons).
        /// Don't try to read this or understand it, I certainly don't.
        /// </summary>
        private static readonly byte[] BitCounts = {
            0,1,1,2,1,2,2,3,1,2,2,3,2,3,3,4,1,2,2,3,2,3,3,4,2,3,3,4,3,4,4,5,1,2,2,3,2,3,3,4,
            2,3,3,4,3,4,4,5,2,3,3,4,3,4,4,5,3,4,4,5,4,5,5,6,1,2,2,3,2,3,3,4,2,3,3,4,3,4,4,5,
            2,3,3,4,3,4,4,5,3,4,4,5,4,5,5,6,2,3,3,4,3,4,4,5,3,4,4,5,4,5,5,6,3,4,4,5,4,5,5,6,
            4,5,5,6,5,6,6,7,1,2,2,3,2,3,3,4,2,3,3,4,3,4,4,5,2,3,3,4,3,4,4,5,3,4,4,5,4,5,5,6,
            2,3,3,4,3,4,4,5,3,4,4,5,4,5,5,6,3,4,4,5,4,5,5,6,4,5,5,6,5,6,6,7,2,3,3,4,3,4,4,5,
            3,4,4,5,4,5,5,6,3,4,4,5,4,5,5,6,4,5,5,6,5,6,6,7,3,4,4,5,4,5,5,6,4,5,5,6,5,6,6,7,
            4,5,5,6,5,6,6,7,5,6,6,7,6,7,7,8
        };

        /// <summary>
        /// Counts bits (duh). Utility function for similarity.
        /// I wouldn't try to understand this. I just copy-pasta'd it
        /// from its's implementation. It works.
        /// </summary>
        /// <param name="num">The hash we are counting.</param>
        /// <returns>The total bit count.</returns>
        private static uint BitCount(ulong num)
        {
            uint count = 0;
            for (; num > 0; num >>= 8)
                count += BitCounts[(num & 0xff)];
            return count;
        }
        #endregion

        #region Public interface methods
        /// <summary>
        /// Computes the average hash of an image according to the algorithm given by Dr. Neal Krawetz
        /// on his blog: http://www.hackerfactor.com/blog/index.php?/archives/432-Looks-Like-It.html.
        /// </summary>
        /// <param name="image">The image to hash.</param>
        /// <returns>The hash of the image.</returns>
        public static ulong AverageHash(Image image)
        {
            // Squeeze the image into an 8x8 canvas
            var squeezed = new Bitmap(8, 8, PixelFormat.Format32bppRgb);
            var canvas = Graphics.FromImage(squeezed);
            canvas.CompositingQuality = CompositingQuality.HighQuality;
            canvas.InterpolationMode = InterpolationMode.HighQualityBilinear;
            canvas.SmoothingMode = SmoothingMode.HighQuality;
            canvas.DrawImage(image, 0, 0, 8, 8);

            // Reduce colors to 6-bit grayscale and calculate average color value
            var grayscale = new byte[64];
            uint averageValue = 0;
            for (var y = 0; y < 8; y++)
                for (var x = 0; x < 8; x++)
                {
                    var pixel = (uint)squeezed.GetPixel(x, y).ToArgb();
                    var gray = (pixel & 0x00ff0000) >> 16;
                    gray += (pixel & 0x0000ff00) >> 8;
                    gray += (pixel & 0x000000ff);
                    gray /= 12;

                    grayscale[x + (y * 8)] = (byte)gray;
                    averageValue += gray;
                }
            averageValue /= 64;

            // Compute the hash: each bit is a pixel
            // 1 = higher than average, 0 = lower than average
            ulong hash = 0;
            for (var i = 0; i < 64; i++)
                if (grayscale[i] >= averageValue)
                    hash |= (1UL << (63 - i));

            return hash;
        }

        /// <summary>
        /// Computes the average hash of the image content in the given file.
        /// </summary>
        /// <param name="path">Path to the input file.</param>
        /// <returns>The hash of the input file's image content.</returns>
        public static ulong AverageHash(string path)
        {
            var bmp = new Bitmap(path);
            return AverageHash(bmp);
        }

        /// <summary>
        /// Returns a percentage-based similarity value between the two given hashes. The higher
        /// the percentage, the closer the hashes are to being identical.
        /// </summary>
        /// <param name="hash1">The first hash.</param>
        /// <param name="hash2">The second hash.</param>
        /// <returns>The similarity percentage.</returns>
        public static double Similarity(ulong hash1, ulong hash2)
        {
            return ((64 - BitCount(hash1 ^ hash2)) * 100) / 64.0;
        }

        /// <summary>
        /// Returns a percentage-based similarity value between the two given images. The higher
        /// the percentage, the closer the images are to being identical.
        /// </summary>
        /// <param name="image1">The first image.</param>
        /// <param name="image2">The second image.</param>
        /// <returns>The similarity percentage.</returns>
        public static double Similarity(Image image1, Image image2)
        {
            var hash1 = AverageHash(image1);
            var hash2 = AverageHash(image2);
            return Similarity(hash1, hash2);
        }

        /// <summary>
        /// Returns a percentage-based similarity value between the image content of the two given
        /// files. The higher the percentage, the closer the image contents are to being identical.
        /// </summary>
        /// <param name="path1">The first image file.</param>
        /// <param name="path2">The second image file.</param>
        /// <returns>The similarity percentage.</returns>
        public static double Similarity(string path1, string path2)
        {
            var hash1 = AverageHash(path1);
            var hash2 = AverageHash(path2);
            return Similarity(hash1, hash2);
        }
        #endregion
    }
}
