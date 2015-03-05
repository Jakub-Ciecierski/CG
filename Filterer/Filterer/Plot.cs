using Filter.FunctionFilters;
using Filterer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FilterGUI
{
    public class Plot
    {
        private Ellipse leftPoint;
        private Ellipse rightPoint;

        private List<Ellipse> points;

        private List<Line> lines;

        private Canvas canvasPlot;

        public Plot(Ellipse leftPoint, Ellipse rightPoint, Line line, Canvas canvasPlot)
        {
            this.leftPoint = leftPoint;
            this.rightPoint = rightPoint;

            points = new List<Ellipse>();
            lines = new List<Line>();

            lines.Add(line);

            this.canvasPlot = canvasPlot;


        }

        public Ellipse GetPointAt(int i)
        {
            if (i == 0)
                return leftPoint;
            if (i == points.Count + 1)
                return rightPoint;
            if (i >= points.Count + 2)
                return null;
            return points.ElementAt(i-1);
        }
        public int GetPointsSize()
        {
            return points.Count + 2;
        }

        public void AddPoint(Ellipse point)
        {
            // find his immediate neighbors
            double x = Canvas.GetLeft(point);
            Ellipse leftNeighbor = GetPointAt(0);
            Ellipse rightNeighbor = GetPointAt(GetPointsSize() - 1);

            int leftNeighborIndex = 0;
            int rightNeighborIndex = GetPointsSize() - 1;

            for (int i = 0; i < GetPointsSize(); i++)
            {
                Ellipse currentPoint = GetPointAt(i);
                double currentX = Canvas.GetLeft(currentPoint);
                if (currentX < x && currentX > Canvas.GetLeft(leftNeighbor))
                {
                    leftNeighbor = currentPoint;
                    leftNeighborIndex = i;
                }
            }

            for (int i = 0; i < GetPointsSize(); i++)
            {
                Ellipse currentPoint = GetPointAt(i);
                double currentX = Canvas.GetLeft(currentPoint);
                if (currentX > x && currentX < Canvas.GetLeft(rightNeighbor))
                {
                    rightNeighbor = currentPoint;
                    rightNeighborIndex = i;
                }
            }
            points.Insert(rightNeighborIndex - 1, point);

            // Remove line between neightbors
            Line line = lines.ElementAt(rightNeighborIndex - 1);
            canvasPlot.Children.Remove(line);
            lines.RemoveAt(rightNeighborIndex - 1);

            // Add two lines between, LeftNeigh - point - RightNeigh
            Line leftLine = new Line();

            leftLine.X1 = Canvas.GetLeft(leftNeighbor);
            leftLine.Y1 = Canvas.GetTop(leftNeighbor);

            leftLine.X2 = Canvas.GetLeft(point);
            leftLine.Y2 = Canvas.GetTop(point);

            leftLine.Stroke = Brushes.Black;
            leftLine.StrokeThickness = 0.5;

            Line rightLine = new Line();

            rightLine.X1 = Canvas.GetLeft(point);
            rightLine.Y1 = Canvas.GetTop(point);

            rightLine.X2 = Canvas.GetLeft(rightNeighbor);
            rightLine.Y2 = Canvas.GetTop(rightNeighbor);

            rightLine.Stroke = Brushes.Black;
            rightLine.StrokeThickness = 0.5;

            lines.Insert(rightNeighborIndex - 1, leftLine);
            lines.Insert(rightNeighborIndex, rightLine);

            canvasPlot.Children.Add(leftLine);
            canvasPlot.Children.Add(rightLine);
        }

        public void UpdatePoint(Ellipse point, double mouseX, double mouseY)
        {
            int pointIndex = points.IndexOf(point);
            pointIndex++;
            double x = Canvas.GetLeft(point);
            double y = Canvas.GetTop(point);

            Ellipse leftNeighbor = GetPointAt(pointIndex - 1);
            Ellipse rightNeighbor = GetPointAt(pointIndex + 1);

            int leftNeighborIndex = pointIndex - 1;
            int rightNeighborIndex = pointIndex + 1;

            Line leftLine = lines.ElementAt(leftNeighborIndex);
            Line rightLine = lines.ElementAt(leftNeighborIndex + 1);

            if (mouseY >= 0 && mouseY <= 255 && mouseX >= 0 && mouseX <= 255 )
            {
                if (!(x >= Canvas.GetLeft(leftNeighbor)))
                {
                    if (mouseX >= x)
                    {
                        Canvas.SetLeft(point, mouseX);
                        Canvas.SetTop(point, mouseY);
                    }
                    else
                    {
                        Canvas.SetLeft(point, x);
                        Canvas.SetTop(point, mouseY);
                    }
                }

                else if (!(x <= Canvas.GetLeft(rightNeighbor)))
                {
                    if (mouseX <= x)
                    {
                        Canvas.SetLeft(point, mouseX);
                        Canvas.SetTop(point, mouseY);
                    }
                    else
                    {
                        Canvas.SetLeft(point, x);
                        Canvas.SetTop(point, mouseY);
                    }
                }
                else { 
                    Canvas.SetLeft(point, mouseX);
                    Canvas.SetTop(point, mouseY);
                }

                leftLine.X2 = Canvas.GetLeft(point);
                leftLine.Y2 = Canvas.GetTop(point);

                rightLine.X1 = Canvas.GetLeft(point);
                rightLine.Y1 = Canvas.GetTop(point);
            }
        }

        public void UpdateLeftPoint()
        {
            Line line = lines.ElementAt(0);
            line.Y1 = Canvas.GetTop(leftPoint);
        }

        public void UpdateRightPoint()
        {
            Line line = lines.ElementAt(lines.Count - 1);
            line.Y2 = Canvas.GetTop(rightPoint);
        }

        public void ApplyFilter(MainWindow mainWindow)
        {
            List<byte> functionMapper = new List<byte>();
            for (int i = 0; i < lines.Count; i++)
            {
                Line line = lines.ElementAt(i);
                int x0 = (int)line.X1;
                int y0 = 255 - (int)line.Y1;

                int x1 = (int)line.X2;
                int y1 = 255 - (int)line.Y2;

                int lengthX = x1 - x0;

                for (int x = x0; x <= x1; x++)
                {
                    double yDiff = y1 - y0;
                    double xDiff = x1 - x0;
                    double m = yDiff / xDiff;

                    double diff = (x - x0);
                    double d_y = y0 + diff * m;
                    int y = (int)d_y;
                    functionMapper.Add((byte)y);
                }
            }

            CustomFilter filter = new CustomFilter(functionMapper);

            mainWindow.imageHandler.ApplyFilter(image => filter.ApplyFilter(image));

            mainWindow.filteredImage.Source = BitmapLoader.loadBitmap(mainWindow.imageHandler.getFiltered());

        }
    }
}
