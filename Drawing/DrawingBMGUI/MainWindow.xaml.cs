using Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DrawingBMGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SolidColorBrush brush = new SolidColorBrush() { Color = Colors.Black };

        Point point1;
        Point point2;

        Drawer drawer;

        bool whichToDraw = true;

        byte[][] MSamplingBitmap; 

        // The writable bitmap
        WriteableBitmap wBitmap;

        public MainWindow()
        {
            InitializeComponent();

            
            drawer = new Drawer(1);
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            wBitmap = new WriteableBitmap(100, 100, 300, 300, PixelFormats.Rgb24, null);
            mainBitmap.Source = wBitmap;
            int width = (int)mainBitmap.ActualWidth;
            int height = (int)mainBitmap.ActualHeight;

            putPixel();
        }

        private unsafe void putPixel()
        {
            int width = wBitmap.PixelWidth;
            int height = wBitmap.PixelHeight;
            int stride = wBitmap.BackBufferStride;
            int bytesPerPixel = (wBitmap.Format.BitsPerPixel ) / 8;

            wBitmap.Lock();
            byte* pImgData = (byte*)wBitmap.BackBuffer;

            int cRowStart = 0;
            int cColStart = 0;
            for (int row = 0; row < height; row++)
            {
                cColStart = cRowStart;
                for (int col = 0; col < 1; col++)
                {
                    byte* bPixel = pImgData + cColStart;

                    bPixel[0] = 250;
                    bPixel[1] = 50;
                    bPixel[2] = 10;

                    cColStart += bytesPerPixel;
                }
                
                cRowStart += stride;
            }
            wBitmap.Unlock();
            // if you are going across threads, you will need to additionally freeze the source
            wBitmap.Freeze();
        }

    }
}
