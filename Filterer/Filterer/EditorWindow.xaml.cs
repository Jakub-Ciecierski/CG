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
        private Canvas plot;
        private Ellipse yAxisPoint;
        private Ellipse xAxisPoint;

        public EditorWindow()
        {
            InitializeComponent();

            plot = filterPlot;
            yAxisPoint = new Ellipse();
            xAxisPoint = new Ellipse();
            drawPlot();
        }

        private void drawPlot()
        {
            yAxisPoint.Stroke = SystemColors.WindowFrameBrush;
            yAxisPoint.Width = 10;
            yAxisPoint.Height = 10;
            yAxisPoint.Fill = SystemColors.ActiveCaptionBrush;

            xAxisPoint.Stroke = SystemColors.WindowFrameBrush;
            xAxisPoint.Width = 10;
            xAxisPoint.Height = 10;
            xAxisPoint.Fill = SystemColors.ActiveCaptionBrush;

            Line yAxis = new Line();
            yAxis.Stroke = SystemColors.WindowFrameBrush;
            yAxis.StrokeThickness = 1;
            yAxis.X1 = 0;
            yAxis.Y1 = 0;
            yAxis.X2 = 0;
            yAxis.Y2 = 255;

            Line xAxis = new Line();
            xAxis.Stroke = SystemColors.WindowTextBrush;
            xAxis.StrokeThickness = 1;
            xAxis.X1 = 0;
            xAxis.Y1 = 255;
            xAxis.X2 = 255;
            xAxis.Y2 = 255;

            for (int i = 0; i < plot.Width; i+=5)
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

                plot.Children.Add(gridLine1);
                plot.Children.Add(gridLine2);
            }

            plot.Children.Add(yAxis);
            plot.Children.Add(xAxis);
            plot.Children.Add(yAxisPoint);
            plot.Children.Add(xAxisPoint);

            plot.Background = new SolidColorBrush(Colors.LightCyan);

            Canvas.SetTop(xAxisPoint,0);
            Canvas.SetLeft(xAxisPoint,250);

            Canvas.SetTop(yAxisPoint, 250);
            Canvas.SetLeft(yAxisPoint, -5);
        }

        private void filterPlot_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point currentPoint = e.GetPosition(filterPlot);
            MessageBox.Show("x: " + currentPoint.X + " y: " + currentPoint.Y);
        }

        private void filterPlot_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Point currentPoint = e.GetPosition(filterPlot);
            //MessageBox.Show("x: " + currentPoint.X + " y: " + currentPoint.Y);
        }
    }
}
