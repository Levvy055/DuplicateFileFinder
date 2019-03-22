using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateFileFinder
{
    public class Bhattacharyya
    {
        public static double BhattacharyyaDifference(Image img1, Image img2)
        {
            var img1GrayscaleValues = img1.GetGrayScaleValues();
            var img2GrayscaleValues = img2.GetGrayScaleValues();

            var normalizedHistogram1 = new double[16, 16];
            var normalizedHistogram2 = new double[16, 16];

            var histSum1 = img1GrayscaleValues.Cast<byte>().
                Aggregate(0.0, (current, value) => current + value);
            var histSum2 = img2GrayscaleValues.Cast<byte>().
                Aggregate(0.0, (current, value) => current + value);
            
            for (var x = 0; x < img1GrayscaleValues.GetLength(0); x++)
            {
                for (var y = 0; y < img1GrayscaleValues.GetLength(1); y++)
                {
                    normalizedHistogram1[x, y] = (double)img1GrayscaleValues[x, y] / histSum1;
                }
            }
            for (var x = 0; x < img2GrayscaleValues.GetLength(0); x++)
            {
                for (var y = 0; y < img2GrayscaleValues.GetLength(1); y++)
                {
                    normalizedHistogram2[x, y] = (double)img2GrayscaleValues[x, y] / histSum2;
                }
            }

            var bCoefficient = 0.0;
            for (var x = 0; x < img2GrayscaleValues.GetLength(0); x++)
            {
                for (var y = 0; y < img2GrayscaleValues.GetLength(1); y++)
                {
                    var histSquared = normalizedHistogram1[x, y] * normalizedHistogram2[x, y];
                    bCoefficient += Math.Sqrt(histSquared);
                }
            }

            var dist1 = 1.0 - bCoefficient;
            dist1 = Math.Round(dist1, 8);
            var distance = Math.Sqrt(dist1);
            distance = Math.Round(distance, 8);
            return distance;
        }
    }
}
