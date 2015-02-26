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

namespace Filterer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ImageHandler imageHandler;

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

        private void negationButtonClick(object sender, RoutedEventArgs e)
        {
            if (imageHandler != null)
            {
                // TODO Background worker
                new Thread(() =>
                {
                    imageHandler.ApplyFilter(image => FunctionFilters.Negation(image));

                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
                    {
                        filteredImage.Source = BitmapLoader.loadBitmap(imageHandler.getFiltered());
                    }));
                    
                }).Start();

            }
        }

        private void editorButtonClick(object sender, RoutedEventArgs e)
        {
        }
    }
}
