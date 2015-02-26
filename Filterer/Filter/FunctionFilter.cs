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
    public abstract class FunctionFilter
    {
        abstract public Bitmap ApplyFilter(Bitmap image);

        protected Bitmap applyFilter(Bitmap image, Func<byte, byte> filter)
        {
            Bitmap filteredImage = (Bitmap)image.Clone();
            for (int i = 0; i < filteredImage.Width; i++)
            {
                for (int j = 0; j < filteredImage.Height; j++)
                {
                    Color color = filteredImage.GetPixel(i, j);

                    byte newRead = filter(color.R);
                    byte newGreen = filter(color.G);
                    byte newBlue = filter(color.B);

                    Color newColor = Color.FromArgb(color.A, newRead, newGreen, newBlue);

                    filteredImage.SetPixel(i, j, newColor);
                }
            }
            return filteredImage;
        }
    }
}
