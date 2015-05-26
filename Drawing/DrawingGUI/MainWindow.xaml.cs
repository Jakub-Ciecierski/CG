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

namespace DrawingGUI
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

        public MainWindow()
        {
            InitializeComponent();

            
            drawer = new Drawer(1);
        }

        private void initMultiSampling()
        {
            MSamplingBitmap = new byte[2 * (int)canvas.ActualHeight][];
            for (int i = 0; i < 2*(int)canvas.ActualHeight; i++)
            {
                MSamplingBitmap[i] = new byte[2 * (int)canvas.ActualWidth];
            }
        }


        private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            point1 = e.GetPosition(canvas);
        }

        private void canvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            point2 = e.GetPosition(canvas);

            if (point1 != null)
            {
                new Thread(() =>
                {
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
                    {
                        if (whichToDraw)
                            multiSamplingDrawLine(point1, point2);//drawLine(point1, point2);
                        else
                            multiSamplingDrawCircle(point1, point2);// drawCircle(point1, point2);
                    }));
                    
                }).Start();
            }
        }

        private void drawCircle(Point point1, Point point2)
        {
            int x2 = (int)point2.X;
            int x1 = (int)point1.X;
            int y2 = (int)point2.Y;
            int y1 = (int)point1.Y;

            int R = (int)Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
            drawer.DrawCircle((int)point1.X, (int)canvas.ActualHeight - (int)point1.Y, R, putPixel);
        }

        private void drawLine(Point point1, Point point2)
        {
            drawer.DrawSymmetricLine((int)point1.X, (int)canvas.ActualHeight - (int)point1.Y, 
                            (int)point2.X, (int)canvas.ActualHeight - (int)point2.Y , putPixel);

        }

        private void multiSamplingDrawCircle(Point point1, Point point2)
        {
            initMultiSampling();

            int height = (int)canvas.ActualHeight;
            int width = (int)canvas.ActualWidth;

            int x2 = (int)point2.X;
            int x1 = (int)point1.X;
            int y2 = (int)point2.Y;
            int y1 = (int)point1.Y;

            //x2 *= 2;
            //x1 *= 2;
            //y2 *= 2;
            //y1 *= 2;

            int R = (int)Math.Sqrt(Math.Pow(2*x2 - 2*x1, 2) + Math.Pow(2*y2 - 2*y1, 2));
            drawer.DrawCircle(2*x1,  (2*height - 2*y1), R, putMultiSamplingPixel);

            // for each pixel on canvas, get average of four pixel
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int sum = 0;
                    sum += MSamplingBitmap[2 * i][2 * j];
                    sum += MSamplingBitmap[2 * i + 1][2 * j];
                    sum += MSamplingBitmap[2 * i][2 * j + 1];
                    sum += MSamplingBitmap[2 * i + 1][2 * j + 1];

                    if (sum != 0)
                        Console.Write(sum);

                    double op = (double)(sum / 4.0);

                    putPixelOpacity(i, j, op);
                }
            }
        }

        private void multiSamplingDrawLine(Point point1, Point point2)
        {
            initMultiSampling();

            int height = (int)canvas.ActualHeight;
            int width = (int)canvas.ActualWidth;

            drawer.Thickness = 2 * drawer.Thickness + 1;
            if (drawer.Thickness == 11)
                drawer.Thickness = 10;

            drawer.DrawSymmetricLine(2* (int)point1.X, 2* (height - (int)point1.Y),
                            2 * (int)point2.X, 2 * (height - (int)point2.Y), putMultiSamplingPixel);


            // for each pixel on canvas, get average of four pixel
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int sum = 0;
                    sum += MSamplingBitmap[2*i][2*j];
                    sum += MSamplingBitmap[2*i+1][2*j];
                    sum += MSamplingBitmap[2*i][2*j+1];
                    sum += MSamplingBitmap[2*i+1][2*j+1];

                    if (sum != 0)
                        Console.Write(sum);

                    double op = (double)(sum / 4.0);

                    putPixelOpacity(i, j, op);
                }
            }
        }

        private void putMultiSamplingPixel(int x, int y)
        {
            MSamplingBitmap[x][y] = 1;
        }

        private void putPixelOpacity(int x, int y, double opacity)
        {
            byte C = 0;
            C = (byte)(255 -  (255 * opacity));

            var brush = new SolidColorBrush(Color.FromArgb(255, C, C, C));

            System.Windows.Shapes.Rectangle rect;
            rect = new System.Windows.Shapes.Rectangle();
            rect.Stroke = brush;// new SolidColorBrush(Colors.Black);
            rect.Fill = brush;// new SolidColorBrush(Colors.Black);
            rect.StrokeThickness = 1;
            rect.Width = 1;
            rect.Height = 1;

            canvas.Children.Add(rect);
            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, (int)canvas.ActualHeight - y);
        }

        private void putPixel(int x, int y)
        {

            System.Windows.Shapes.Rectangle rect;
            rect = new System.Windows.Shapes.Rectangle();
            rect.Stroke = new SolidColorBrush(Colors.Black);
            rect.Fill = new SolidColorBrush(Colors.Black);
            rect.StrokeThickness = 1;
            rect.Width = 1;
            rect.Height = 1;

            canvas.Children.Add(rect);
            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, (int)canvas.ActualHeight - y);
        }

        private void lineButton_Click(object sender, RoutedEventArgs e)
        {
            whichToDraw = true;
        }

        private void circleButton_Click(object sender, RoutedEventArgs e)
        {
            whichToDraw = false;
        }

        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            ComboBox box = sender as ComboBox;
            if (box.Text != "")
                drawer.Thickness = System.Convert.ToInt32(box.Text);
        }
    }
}
