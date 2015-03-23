using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorQuantization.MedianCut
{
    /// <summary>
    /// Median cut is an algorithm to sort data of an arbitrary number of
    /// dimensions into series of sets by recursively cutting 
    /// each set of data at the median point along the longest dimension. 
    /// Median cut is typically used for color quantization. For example, to reduce 
    /// a 64k-colour image to 256 colours, median cut is used to find 256 colours that match the original data well.
    /// 
    /// source of summary: http://en.wikipedia.org/wiki/Median_cut
    /// </summary>
    public class MedianCut
    {
        private List<Pixel> pixels = new List<Pixel>();
        private List<Cube> cubes = new List<Cube>();

        private Bitmap image;

        public Bitmap Image
        {
            get { return image;}
        }

        public MedianCut(Bitmap image)
        {
            this.image = image;
        }

        /// <summary>
        /// Compute the coloir palette with k colors
        /// </summary>
        /// <param name="k">
        ///     Number of colors
        /// </param>
        private void computePalette(int k)
        {
            // Add all the Pixels 
            Bitmap tmpImage = (Bitmap)image.Clone();
            for (int i = 0; i < tmpImage.Width; i++)
            {
                for (int j = 0; j < tmpImage.Height; j++)
                {
                    pixels.Add(new Pixel(tmpImage.GetPixel(i, j), i, j));
                }
            }

            // Initial Cube with all the pixels
            Cube cube = new Cube(pixels);
            cubes.Add(cube);

            int currentIndex = cubes.Count - 1;

            // Proceed to split cubes into smaller cube
            while (cubes.Count < k)
            {
                // The cube to be split
                Cube cubeToSplit = cubes[currentIndex];

                Cube[] newCubes = cubeToSplit.Split();

                cubes.RemoveAt(currentIndex);

                cubes.Insert(currentIndex, newCubes[0]);
                cubes.Insert(currentIndex, newCubes[1]);

                currentIndex--;
                currentIndex = (currentIndex < 0) ? cubes.Count - 1 : currentIndex;
            }
        }

        private void applyToImage()
        {
            foreach (Cube cube in cubes)
            {
                foreach (Pixel pixel in cube.Pixels)
                {
                    Color paletteColor = cube.GetColor();
                    image.SetPixel(pixel.X, pixel.Y, paletteColor);
                }
            }
        }

        public void Compute(int k)
        {
            computePalette(k);
            applyToImage();
        }


       
    }
}
