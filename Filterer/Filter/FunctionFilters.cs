using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Filter
{
    /*
     * 1.1) negation
     * 1.2) brightness
     * 1.3) contrast
     */
    public class FunctionFilters
    {
        private static byte negationFunction(byte x)
        {
            byte max = 255;
            byte y = (byte)(max - x);

            return y;
        }
        private static byte brightnessFunction(byte x, byte a)
        {
            byte max = 255;
            byte min = 0;
            byte y = (byte)(x+a);
            y = (y > 255) ? max : y;
            y = (y < 0) ? min : y;

            return y;
        }

        public static Bitmap Negation(Bitmap image)
        {
            Bitmap filteredImage = (Bitmap)image.Clone();
            for (int i = 0; i < filteredImage.Width; i++)
            {
                for (int j = 0; j < filteredImage.Height; j++)
                {
                    Color color = filteredImage.GetPixel(i, j);

                    byte newRead = negationFunction(color.R);
                    byte newGreen = negationFunction(color.G);
                    byte newBlue = negationFunction(color.B);

                    Color newColor = Color.FromArgb(color.A, newRead, newGreen, newBlue);

                    filteredImage.SetPixel(i, j, newColor);
                }
            }
                return filteredImage;
        }
    }
}
