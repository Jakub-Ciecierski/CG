using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dither
{
    /*
     * average dithering & median -cut CQ
     */
    public class Thresholding
    {
        private int threshold;

        public Thresholding(int threshHold)
        {
            this.threshold = threshHold;
        }

        private byte threshHoldFunction(byte x)
        {
            byte y = 0;
            if (x > threshold)
                y = 255;
            else
                y = 0;
            return y;
        }

        protected Bitmap compute(Bitmap image)
        {
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
