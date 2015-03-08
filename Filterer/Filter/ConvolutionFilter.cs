using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filter
{
    public abstract class ConvolutionFilter
    {
        protected readonly int n;
        protected readonly int m;

        protected int anchor;

        protected int offset;
        protected int devisor;

        protected List<int> kernelMatrix; // M[n x m]

        public ConvolutionFilter(int n, int m, int anchor)
        {
            this.n = n;
            this.m = m;
            this.anchor = anchor;
        }

        abstract public Bitmap ApplyFilter(Bitmap image);

        private int getKernelPoint(int i, int j)
        {
            return kernelMatrix[anchor + n*i + j];
        }

        protected virtual void setDevisorAndOffset()
        {
            offset = 0;
            devisor = 0;

            foreach (int d in kernelMatrix)
                devisor += d;

            if (devisor < 1)
            {
                offset = 127;
                devisor = 1;
            }
        }

        protected Bitmap compute(Bitmap image)
        {
            Bitmap filteredImage = (Bitmap)image.Clone();

            for (int i = 0; i < filteredImage.Width; i++)
            {
                for (int j = 0; j < filteredImage.Height; j++)
                {
                    Color color = image.GetPixel(i, j);

                    int newRead = 0;
                    int newGreen = 0;
                    int newBlue = 0;

                    for (int k = -1; k <= 1; k++)
                    {
                        for (int l = -1; l <= 1; l++)
                        {
                            Color currentColor;
                            int readValue;
                            int greenValue;
                            int blueValue;

                            try 
                            { 
                                currentColor = image.GetPixel(i + k, j + l);
                            
                                readValue = (currentColor.R * getKernelPoint(k, l));
                                greenValue = (currentColor.G * getKernelPoint(k, l));
                                blueValue = (currentColor.B * getKernelPoint(k, l));
                            }
                            catch (ArgumentOutOfRangeException e)
                            {
                                readValue = 0;
                                greenValue = 0;
                                blueValue = 0;
                            }

                            newRead += readValue;
                            newGreen += greenValue;
                            newBlue += blueValue;
                        }
                    }

                    newRead = (newRead / devisor) + offset;
                    newGreen = (newGreen / devisor) + offset;
                    newBlue = (newBlue / devisor) + offset;

                    if (newRead < 0) newRead = 0;
                    if (newRead > 255) newRead = 255;
                    if (newGreen < 0) newGreen = 0;
                    if (newGreen > 255) newGreen = 255;
                    if (newBlue < 0) newBlue = 0;
                    if (newBlue > 255) newBlue = 255;

                    if (newRead < 0 || newRead > 255 || newGreen < 0 || newGreen > 255 || newBlue < 0 || newBlue > 255)
                    {
                        throw new ArithmeticException("byte out of bound");
                    }    

                    Color newColor = Color.FromArgb(color.A, (byte)newRead, (byte)newGreen, (byte)newBlue);

                    filteredImage.SetPixel(i, j, newColor);
                }
            }
            return filteredImage;
        }

    }
}
