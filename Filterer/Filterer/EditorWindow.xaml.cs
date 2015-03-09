using Filterer;
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
using System.Windows.Shapes;

namespace FilterGUI
{
    /// <summary>
    /// Interaction logic for EditorWindow.xaml
    /// </summary>
    public partial class EditorWindow : Window
    {
        private Canvas canvasPlot;
        private Ellipse leftAxisPoint;
        private Ellipse rightAxisPoint;

        private Plot plot;

        private MainWindow mainWindow;

        public EditorWindow(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();

            canvasPlot = filterPlot;
            leftAxisPoint = new Ellipse();
            rightAxisPoint = new Ellipse();

            canvasPlot.MouseRightButtonUp += plot_MouseRightButtonUp;

            leftAxisPoint.MouseMove += leftAxisPoint_MouseLeftMove;
            leftAxisPoint.MouseDown += leftAxisPoint_MouseLeftDown;
            leftAxisPoint.MouseUp += leftAxisPoint_MouseLeftUp;

            rightAxisPoint.MouseMove += rightAxisPoint_MouseLeftMove;
            rightAxisPoint.MouseDown += rightAxisPoint_MouseLeftDown;
            rightAxisPoint.MouseUp += rightAxisPoint_MouseLeftUp;

            drawPlot();
        }

        private void drawPlot()
        {
            leftAxisPoint.Stroke = Brushes.Red;
            leftAxisPoint.Width = 10;
            leftAxisPoint.Height = 10;
            leftAxisPoint.Fill = Brushes.Red;

            rightAxisPoint.Stroke = Brushes.Red;
            rightAxisPoint.Width = 10;
            rightAxisPoint.Height = 10;
            rightAxisPoint.Fill = Brushes.Red;

            Line yAxis = new Line();
            yAxis.Stroke = Brushes.Black;
            
            yAxis.StrokeThickness = 1;
            yAxis.X1 = 0;
            yAxis.Y1 = 0;
            yAxis.X2 = 0;
            yAxis.Y2 = 255;

            Line xAxis = new Line();
            xAxis.Stroke = Brushes.Black;
            xAxis.StrokeThickness = 1;
            xAxis.X1 = 0;
            xAxis.Y1 = 255;
            xAxis.X2 = 255;
            xAxis.Y2 = 255;

            for (int i = 0; i < canvasPlot.Width; i+=5)
            {
                Line gridLine1 = new Line();
                Line gridLine2 = new Line();

                gridLine1.Stroke = SystemColors.WindowTextBrush;
                gridLine1.StrokeThickness = 0.2;

                gridLine1.X1 = i;
                gridLine1.Y1 = 0;

                gridLine1.X2 = i;
                gridLine1.Y2 = 255;


                gridLine2.Stroke = SystemColors.WindowTextBrush;
                gridLine2.StrokeThickness = 0.2;

                gridLine2.X1 = 0;
                gridLine2.Y1 = i;

                gridLine2.X2 = 255;
                gridLine2.Y2 = i;

                canvasPlot.Children.Add(gridLine1);
                canvasPlot.Children.Add(gridLine2);
            }

            canvasPlot.Children.Add(yAxis);
            canvasPlot.Children.Add(xAxis);
            canvasPlot.Children.Add(leftAxisPoint);
            canvasPlot.Children.Add(rightAxisPoint);

            canvasPlot.Background = new SolidColorBrush(Colors.LightCyan);

            Canvas.SetTop(rightAxisPoint,0);
            Canvas.SetLeft(rightAxisPoint,255);

            Canvas.SetTop(leftAxisPoint, 255);
            Canvas.SetLeft(leftAxisPoint, 0);

            rightPointInfoBlock.Text = "255";
            leftPointInfoBlock.Text = "0";

            Line line = new Line();
            line.X1 = 0;
            line.Y1 = Canvas.GetTop(leftAxisPoint);

            line.X2 = 255;
            line.Y2 = Canvas.GetTop(rightAxisPoint);

            line.Stroke = Brushes.Black;
            line.StrokeThickness = 0.5;

            plot = new Plot(leftAxisPoint, rightAxisPoint, line, canvasPlot, this);

            canvasPlot.Children.Add(line);
        }

        public Ellipse CreatePoint(double x, double y) 
        {
            Ellipse point = new Ellipse();

            point.Stroke = Brushes.Yellow;
            point.Width = 10;
            point.Height = 10;
            point.Fill = Brushes.Yellow;

            point.MouseLeftButtonDown += plotPoint_MouseLeftDown;
            point.MouseLeftButtonUp += plotPoint_MouseLeftUp;
            point.MouseMove += plotPoint_MouseLeftMove;


            Canvas.SetTop(point, x);
            Canvas.SetLeft(point, y);

            canvasPlot.Children.Add(point);

            return point;
        }

        /****** Adding new Plot Points handler *******/
        private void plot_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point currentPoint = e.GetPosition(canvasPlot);

            Ellipse newPoint = CreatePoint(currentPoint.Y, currentPoint.X);

            plot.AddPoint(newPoint);
        }

        /********** PlotPoint Mouse Handlers **********/
        private void plotPoint_MouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse point = (Ellipse)sender;
            point.CaptureMouse();
        }

        private void plotPoint_MouseLeftUp(object sender, MouseButtonEventArgs e)
        {
            Ellipse point = (Ellipse)sender;
            point.ReleaseMouseCapture();
        }

        private void plotPoint_MouseLeftMove(object sender, MouseEventArgs e)
        {
            Ellipse point = (Ellipse)sender;
            if (point.IsMouseCaptured)
            {
                double x = e.GetPosition(canvasPlot).X;
                double y = e.GetPosition(canvasPlot).Y;

                plot.UpdatePoint(point, x, y);
            }
        }
        /********** End of PlotPoint Mouse Handlers **********/


        /********** Left AxisPoint Mouse Handlers **********/
        private void leftAxisPoint_MouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            leftAxisPoint.CaptureMouse();
        }

        private void leftAxisPoint_MouseLeftUp(object sender, MouseButtonEventArgs e)
        {
            leftAxisPoint.ReleaseMouseCapture();
        }

        private void leftAxisPoint_MouseLeftMove(object sender, MouseEventArgs e)
        {
            if (leftAxisPoint.IsMouseCaptured)
            {
                double position = e.GetPosition(canvasPlot).Y;
                
                if (position >= 0 && position <= 255)
                {
                    Canvas.SetTop(leftAxisPoint, e.GetPosition(canvasPlot).Y);
                    double actualPosition = 255 - position;
                    leftPointInfoBlock.Text = actualPosition.ToString();

                    plot.UpdateLeftPoint();
                }
            }
        }
        /********** End of Left AxisPoint Mouse Handlers **********/


        /********** Right AxisPoint Mouse Handlers **********/
        private void rightAxisPoint_MouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            rightAxisPoint.CaptureMouse();
        }

        private void rightAxisPoint_MouseLeftUp(object sender, MouseButtonEventArgs e)
        {
            rightAxisPoint.ReleaseMouseCapture();
        }

        private void rightAxisPoint_MouseLeftMove(object sender, MouseEventArgs e)
        {
            if (rightAxisPoint.IsMouseCaptured)
            {
                double position = e.GetPosition(canvasPlot).Y;

                if (position >= 0 && position <= 255)
                {
                    Canvas.SetTop(rightAxisPoint, e.GetPosition(canvasPlot).Y);
                    double actualPosition = 255 - position;
                    rightPointInfoBlock.Text = actualPosition.ToString();
                    plot.UpdateRightPoint();
                }
            }
        }
        /********** End of Right AxisPoint Mouse Handlers **********/

        private void applyFilterButton(object sender, RoutedEventArgs e)
        {
            plot.ApplyFilter(mainWindow);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            
            if (plot != null) 
            {
                string filter = (e.AddedItems[0] as ComboBoxItem).Content.ToString();
                if (filter.Equals("Brightness filter"))
                    plot.DrawBrightnessFilter();
                if (filter.Equals("Contrast filter"))
                    plot.DrawContrastFilter();
                if (filter.Equals("Negation filter"))
                    plot.DrawNegationFilter();
                if (filter.Equals("Custom filter"))
                    plot.Clear();
            }
            
        }
    }
}
