using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dither
{
    public class AverageDither
    {
        private int k;

        private byte[] threshold;

        public AverageDither(int k)
        {
            this.k = k;

            threshold = new byte[k - 1];
        }

        private void computeThreshold(Bitmap image)
        {
            int[] sum;
            int[] pixelCount;

            sum = new int[k - 1];
            pixelCount = new int[k - 1];

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Color color = image.GetPixel(i, j);

                    int thresholdIndex = color.R * (k - 1) / 256;
                    sum[thresholdIndex] += color.R;
                    pixelCount[thresholdIndex]++;
                }
            }
            for (int i = 0; i < k - 1; i++) 
            {
                threshold[i] = (byte)(sum[i] / pixelCount[i]);
            }
        }

        private byte threshHoldFunction(byte x)
        {
            int thresholdIndex = (x * (k - 1)) / 256;

            if (x >= threshold[thresholdIndex])
            {
                int t = (int)(((double)(thresholdIndex +1 )/ (k - 1)) * 255);
                return (byte)t;
            }
            else
            {
                int t = (int)(((double)(thresholdIndex )  / (k - 1)) * 255);
                return (byte)t;
            }
            
        }

        protected Bitmap compute(Bitmap image)
        {
            computeThreshold(image);

            Bitmap filteredImage = (Bitmap)image.Clone();
            for (int i = 0; i < filteredImage.Width; i++)
            {
                for (int j = 0; j < filteredImage.Height; j++)
                {
                    Color color = filteredImage.GetPixel(i, j);

                    byte newRead = threshHoldFunction(color.R);
                    byte newGreen = threshHoldFunction(color.G);
                    byte newBlue = threshHoldFunction(color.B);

                    Color newColor = Color.FromArgb(color.A, newRead, newGreen, newBlue);

                    filteredImage.SetPixel(i, j, newColor);
                }
            }
            return filteredImage;
        }

        public Bitmap ApplyDithering(Bitmap image)
        {
            return compute(image);
        }
    }
}
