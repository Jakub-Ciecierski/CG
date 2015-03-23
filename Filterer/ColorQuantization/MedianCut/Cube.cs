using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorQuantization.MedianCut
{
    class Cube
    {
        private List<Pixel> pixels = new List<Pixel>();

        private byte redLength;
        private byte blueLength;
        private byte greenLength;

        private Color color;
        bool first = true;

        public List<Pixel> Pixels
        {
            get { return pixels; }
        }

        public Cube(List<Pixel> pixels)
        {
            this.pixels = pixels;
            computeLengthOfColors();
        }

        /// <summary>
        /// Finds max and min of each color channel
        /// To compute the length of each channel (axis)
        /// </summary>
        private void computeLengthOfColors()
        {
            byte minRed = 255;
            byte minGreen = 255;
            byte minBlue = 255;

            byte maxRed = 0;
            byte maxGreen = 0;
            byte maxBlue = 0;


            foreach (Pixel pixel in pixels)
            {
                Color color = pixel.Color;
                if (color.R < minRed)
                    minRed = color.R;
                if (color.G < minGreen)
                    minGreen = color.G;
                if (color.B < minBlue)
                    minBlue = color.B;

                if (color.R > maxRed)
                    maxRed = color.R;
                if (color.G > maxGreen)
                    maxGreen = color.G;
                if (color.B > maxBlue)
                    maxBlue = color.B;
            }

            redLength = (byte)(maxRed - minRed);
            greenLength = (byte)(maxGreen - minGreen);
            blueLength = (byte)(maxBlue - minBlue);
        }

        public Color GetColor()
        {
            if (first)
            {
                int red = 0;
                int green = 0;
                int blue = 0;
                foreach (Pixel pixel in pixels)
                {
                    Color currentColor = pixel.Color;
                    red += currentColor.R;
                    green += currentColor.G;
                    blue += currentColor.B;
                }
                int count = pixels.Count;
                if (count != 0)
                {
                    red /= count;
                    green /= count;
                    blue /= count;
                }

                color = Color.FromArgb(red, green, blue);
                first = false;
            }
            return color;
        }

        public Cube[] Split()
        {
            Cube cube1;
            Cube cube2;

            /**
             * For the longest color(axis), sort the pixels by that color
             * and split them in half to two new cubes
             */
            if (redLength >= greenLength && redLength >= blueLength)
                pixels.Sort((p1, p2) => p1.Color.R.CompareTo(p2.Color.R));
            else if (greenLength >= blueLength)
                pixels.Sort((p1, p2) => p1.Color.G.CompareTo(p2.Color.G));
            else
                pixels.Sort((p1, p2) => p1.Color.B.CompareTo(p2.Color.B));

            int median = pixels.Count / 2;
            cube1 = new Cube(pixels.GetRange(0, median));
            cube2 = new Cube(pixels.GetRange(median, pixels.Count - median));

            Cube[] newCubes = { cube1, cube2 };
            return newCubes;
        }
    }
}
