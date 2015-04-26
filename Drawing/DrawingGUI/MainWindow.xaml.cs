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

        public MainWindow()
        {
            InitializeComponent();

            
            drawer = new Drawer(1);
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
                            drawLine(point1, point2);
                        else
                            drawCircle(point1, point2);
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
