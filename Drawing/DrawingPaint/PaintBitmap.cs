using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DrawingPaint
{
    public class PaintBitmap
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/
        /// <summary>
        ///     The DPI constants
        /// </summary>
        private const double DPI_X = 300.0;
        private const double DPI_Y = 300.0;

        /// <summary>
        ///     Color of the nest
        /// </summary>
        private System.Drawing.Color NEST_COLOR = System.Drawing.Color.FromArgb(200, 20, 20);

        public System.Drawing.Color BACKGROUND_COLOR = System.Drawing.Color.FromArgb(255, 255, 255);

        public System.Drawing.Color BRUSH_COLOR = System.Drawing.Color.FromArgb(0, 0, 0);

        /// <summary>
        ///     Dimensions of a cell in pixels
        /// </summary>
        private const int CELL_WIDTH = 10;
        private const int CELL_HEIGHT = 10;

        private int widthPixels;
        private int heightPixels;

        private int stride;
        private int bytesPerPixel;

        /// <summary>
        ///     Actual bitmap of the automaton
        /// </summary>
        private WriteableBitmap wBitmap;

        public WriteableBitmap Bitmap
        {
            get { return wBitmap; }
            private set { wBitmap = value; }
        }

        /// <summary>
        ///     The destination of our bitmap
        /// </summary>
        private Image imageDest;

        double originalImageWidth;
        double originalImageHeight;

        const double MAX_ZOOM_SCALE = 10.0;
        const double MIN_ZOOM_SCALE = 10.0;

        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        /// <summary>
        ///     
        /// </summary>
        /// <param name="width">
        ///     Number of automaton cells in a row, NOT in pixels
        /// </param>
        /// <param name="height">
        ///     
        /// </param>
        /// <param name="automaton"></param>
        /// <param name="imageDestionation"></param>
        public PaintBitmap(int width, int height, Image imageDestionation)
        {
            this.widthPixels = width;
            this.heightPixels = height;

            this.imageDest = imageDestionation;

            init();
        }

        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/

        private void init()
        {
            initBitmap();
            renderBitmap();
        }

        private unsafe void initBitmap()
        {
            wBitmap = new WriteableBitmap(widthPixels, heightPixels, DPI_X, DPI_Y, PixelFormats.Rgb24, null);

            imageDest.Width = widthPixels;
            imageDest.Height = heightPixels;

            originalImageWidth = imageDest.Width;
            originalImageHeight = imageDest.Height;

            imageDest.Source = wBitmap;

            stride = wBitmap.BackBufferStride;
            bytesPerPixel = (wBitmap.Format.BitsPerPixel) / 8;
        }

        private unsafe void renderBitmap()
        {
            wBitmap.Lock();
            byte* pImgData = (byte*)wBitmap.BackBuffer;

            int cRowStart = 0;
            int cColStart = 0;
            for (int row = 0; row < heightPixels; row++)
            {
                cColStart = cRowStart;
                for (int col = 0; col < widthPixels; col++)
                {
                    byte* bPixel = pImgData + cColStart;

                    bPixel[0] = BACKGROUND_COLOR.R;
                    bPixel[1] = BACKGROUND_COLOR.G;
                    bPixel[2] = BACKGROUND_COLOR.B;

                    cColStart += bytesPerPixel;
                }
                cRowStart += stride;
            }
            Int32Rect rect = new Int32Rect(0, 0, widthPixels, heightPixels);
            wBitmap.AddDirtyRect(rect);
            wBitmap.Unlock();
        }

        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/

        public unsafe System.Drawing.Color GetColor(int i, int j)
        {
            int width = wBitmap.PixelWidth;
            int height = wBitmap.PixelHeight;

            double actualWidth = imageDest.ActualWidth;
            double actualHeight = imageDest.ActualHeight;

            double scaleWidth = actualWidth / width;
            double scaleHeight = actualHeight / height;

            // scalled pixel indecies image
            int indexI = (int)(j / scaleWidth);
            int indexJ = (int)(i / scaleHeight);

            wBitmap.Lock();
            byte* pImgData = (byte*)wBitmap.BackBuffer;

            Int32Rect rect = new Int32Rect(indexJ, indexI, 1, 1);

            byte* pixel = pImgData +
                                (stride * indexI) +
                                ((indexJ * bytesPerPixel));
            System.Drawing.Color c;
            c = System.Drawing.Color.FromArgb(pixel[0], pixel[1], pixel[2]);

            wBitmap.Unlock();

            return c;
        }

        public unsafe void PutPixel(int i, int j)
        {
            int width = wBitmap.PixelWidth;
            int height = wBitmap.PixelHeight;

            double actualWidth = imageDest.ActualWidth;
            double actualHeight = imageDest.ActualHeight;

            double scaleWidth = actualWidth / width;
            double scaleHeight = actualHeight / height;

            // scalled pixel indecies image
            int indexI = (int)(j / scaleWidth);
            int indexJ = (int)(i / scaleHeight);

            wBitmap.Lock();
            byte* pImgData = (byte*)wBitmap.BackBuffer;

            Int32Rect rect = new Int32Rect(indexJ, indexI, 1, 1);

            byte* pixel = pImgData +
                                (stride * indexI) +
                                ((indexJ * bytesPerPixel));

            try
            {
                System.Drawing.Color c = BRUSH_COLOR;
                // color the bitmap
                pixel[0] = c.R;
                pixel[1] = c.G;
                pixel[2] = c.B;
            }
            catch (AccessViolationException e) { Console.Write(e.StackTrace); }

            try
            {
                wBitmap.AddDirtyRect(rect);
            }
            catch (ArgumentException e) { Console.Write(e.StackTrace); }
            wBitmap.Unlock();
        }

        public unsafe void PutPixel(int i, int j, System.Drawing.Color c)
        {
            int width = wBitmap.PixelWidth;
            int height = wBitmap.PixelHeight;

            double actualWidth = imageDest.ActualWidth;
            double actualHeight = imageDest.ActualHeight;

            double scaleWidth = actualWidth / width;
            double scaleHeight = actualHeight / height;

            // scalled pixel indecies image
            int indexI = (int)(j / scaleWidth);
            int indexJ = (int)(i / scaleHeight);

            wBitmap.Lock();
            byte* pImgData = (byte*)wBitmap.BackBuffer;

            Int32Rect rect = new Int32Rect(indexJ, indexI, 1, 1);

            byte* pixel = pImgData +
                                (stride * indexI) +
                                ((indexJ * bytesPerPixel));

            try
            {
                // color the bitmap
                pixel[0] = c.R;
                pixel[1] = c.G;
                pixel[2] = c.B;
            }
            catch (AccessViolationException e) { Console.Write(e.StackTrace); }

            try
            {
                wBitmap.AddDirtyRect(rect);
            }
            catch (ArgumentException e) { Console.Write(e.StackTrace); }
            wBitmap.Unlock();
        }
     
        /// <summary>
        ///     Takes point from the image
        ///     Scales it to proper index for bitmap
        ///     and fills a proper cell with given color
        /// </summary>
        /// <param name="point"></param>
        /// <param name="c"></param>
        public void PutPixelByImagePoint(System.Windows.Point point)
        {
            int width = wBitmap.PixelWidth;
            int height = wBitmap.PixelHeight;

            double actualWidth = imageDest.ActualWidth;
            double actualHeight = imageDest.ActualHeight;

            double scaleWidth = actualWidth / width;
            double scaleHeight = actualHeight / height;

            // scalled pixel indecies image
            int pixelI = (int)(point.X / scaleWidth);
            int pixelJ = (int)(point.Y / scaleHeight);

            PutPixel(pixelJ, pixelI);
            
        }

        /// <summary>
        ///     Resets the bitmap.
        ///     Has to be called after resizing automaton.
        ///     Currectly, does not save previous state.
        /// </summary>
        public void Reset()
        {
            init();
        }

        public bool ScaleImage(double scale)
        {
            try
            {
                double width = originalImageWidth * scale;
                double height = originalImageHeight * scale;

                if ((width < originalImageWidth / MIN_ZOOM_SCALE && height < originalImageHeight / MIN_ZOOM_SCALE) ||
                    (width > originalImageWidth * MAX_ZOOM_SCALE && height > originalImageHeight * MAX_ZOOM_SCALE))
                    return false;

                imageDest.Width = width;
                imageDest.Height = height;


            }
            catch (Exception e) { Console.Write(e.StackTrace); }

            return true;
        }
    }
}
