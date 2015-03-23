using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Drawing;

using Filter;
using System.ComponentModel;
using System.Threading;
using System.Windows.Threading;
using Filter.FunctionFilters;
using FilterGUI;
using Filter.ConvolutionFilters;
using Dither;
using ColorQuantization.MedianCut;

namespace Filterer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ImageHandler imageHandler;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void loadButtonClick(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".jpg"; // Default file extension
            dlg.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;

                Bitmap image = new Bitmap(filename);

                imageHandler = new ImageHandler(image);

                originalImage.Source = BitmapLoader.loadBitmap(image);
                filteredImage.Source = BitmapLoader.loadBitmap(image);
            }
        }

        private void editorButtonClick(object sender, RoutedEventArgs e)
        {
            EditorWindow editorWindow = new EditorWindow(this);
            editorWindow.Show();
            //this.Close();
        }

        private void negationButtonClick(object sender, RoutedEventArgs e)
        {
            if (imageHandler != null)
            {
                // TODO Background worker
                new Thread(() =>
                {
                    NegationFilter negationFilter = new NegationFilter();
                    imageHandler.ApplyFilter(image => negationFilter.ApplyFilter(image));

                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
                    {
                        filteredImage.Source = BitmapLoader.loadBitmap(imageHandler.getFiltered());
                    }));
                    
                }).Start();

            }
        }

        private void brightnessButtonClick(object sender, RoutedEventArgs e)
        {
            if (imageHandler != null)
            {
                // TODO Background worker
                new Thread(() =>
                {
                    int a = -100;
                    BrightnessFilter filter = new BrightnessFilter(a);
                    imageHandler.ApplyFilter(image => filter.ApplyFilter(image));

                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
                    {
                        filteredImage.Source = BitmapLoader.loadBitmap(imageHandler.getFiltered());
                    }));

                }).Start();

            }
        }

        private void contrastButtonClick(object sender, RoutedEventArgs e)
        {
            if (imageHandler != null)
            {
                // TODO Background worker
                new Thread(() =>
                {
                    int contrast = 50;
                    ContrastFilter filter = new ContrastFilter(contrast);
                    imageHandler.ApplyFilter(image => filter.ApplyFilter(image));

                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
                    {
                        filteredImage.Source = BitmapLoader.loadBitmap(imageHandler.getFiltered());
                    }));

                }).Start();

            }
        }

        private void gammaCorrectionButtonClick(object sender, RoutedEventArgs e)
        {
            if (imageHandler != null)
            {
                // TODO Background worker
                new Thread(() =>
                {
                    double gamma = 1;
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        gamma = Convert.ToDouble(gammaTextBox.Text);
                    }));
                    
                    GammaCorrectionFilter filter = new GammaCorrectionFilter(gamma, 1);
                    imageHandler.ApplyFilter(image => filter.ApplyFilter(image));

                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
                    {
                        filteredImage.Source = BitmapLoader.loadBitmap(imageHandler.getFiltered());
                    }));

                }).Start();

            }
        }

       
        private void blurFilter_Click(object sender, RoutedEventArgs e)
        {
            if (imageHandler != null)
            {
                // TODO Background worker
                new Thread(() =>
                {
                    BlurFilter filter = new BlurFilter();
                    imageHandler.ApplyFilter(image => filter.ApplyFilter(image));

                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
                    {
                        filteredImage.Source = BitmapLoader.loadBitmap(imageHandler.getFiltered());
                    }));

                }).Start();

            }
        }

        private void edgeDetectionFilter_Click(object sender, RoutedEventArgs e)
        {
            if (imageHandler != null)
            {
                // TODO Background worker
                new Thread(() =>
                {
                    EdgeDetectionFilter filter = new EdgeDetectionFilter();
                    imageHandler.ApplyFilter(image => filter.ApplyFilter(image));

                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
                    {
                        filteredImage.Source = BitmapLoader.loadBitmap(imageHandler.getFiltered());
                    }));

                }).Start();

            }
        }

        private void embossFilter_Click(object sender, RoutedEventArgs e)
        {
            if (imageHandler != null)
            {
                // TODO Background worker
                new Thread(() =>
                {
                    EmbossFilter filter = new EmbossFilter();
                    imageHandler.ApplyFilter(image => filter.ApplyFilter(image));

                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
                    {
                        filteredImage.Source = BitmapLoader.loadBitmap(imageHandler.getFiltered());
                    }));

                }).Start();

            }
        }

        private void gausianSmoothingFilter_Click(object sender, RoutedEventArgs e)
        {
            if (imageHandler != null)
            {
                // TODO Background worker
                new Thread(() =>
                {
                    GausianSmoothingFilter filter = new GausianSmoothingFilter();
                    imageHandler.ApplyFilter(image => filter.ApplyFilter(image));

                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
                    {
                        filteredImage.Source = BitmapLoader.loadBitmap(imageHandler.getFiltered());
                    }));

                }).Start();

            }
        }

        private void sharpenFilter_Click(object sender, RoutedEventArgs e)
        {
            if (imageHandler != null)
            {
                // TODO Background worker
                new Thread(() =>
                {
                    SharpenFilter filter = new SharpenFilter();
                    imageHandler.ApplyFilter(image => filter.ApplyFilter(image));

                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
                    {
                        filteredImage.Source = BitmapLoader.loadBitmap(imageHandler.getFiltered());
                    }));

                }).Start();

            }
        }

        private void threshHoldingButtonClick(object sender, RoutedEventArgs e)
        {
            if (imageHandler != null)
            {
                // TODO Background worker
                new Thread(() =>
                {
                    int threshhold = 170;
                    TreshHolding threshHolder = new TreshHolding(threshhold);
                    Bitmap filtered = threshHolder.ApplyDithering(imageHandler.getOriginal());

                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
                    {
                        filteredImage.Source = BitmapLoader.loadBitmap(filtered);
                    }));

                }).Start();

            }
        }

        private void medianCutClick(object sender, RoutedEventArgs e)
        {
            if (imageHandler != null)
            {
                // TODO Background worker
                new Thread(() =>
                {
                    int k = 8;
                    MedianCut medianCut = new MedianCut(imageHandler.getOriginal());
                    medianCut.Compute(k);

                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
                    {
                        filteredImage.Source = BitmapLoader.loadBitmap(medianCut.Image);
                    }));
                    Console.Write("Median Cut finished \n");
                }).Start();

            }
        }
    }
}
