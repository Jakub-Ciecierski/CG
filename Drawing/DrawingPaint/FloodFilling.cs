using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DrawingPaint
{
    public class FloodFill
    {
        Action<int, int, System.Drawing.Color> PutPixel;
        PaintBitmap paintBitmap;

        public FloodFill(PaintBitmap paintBitmap, Action<int, int, System.Drawing.Color> putPixel)
        {
            PutPixel = putPixel;
            this.paintBitmap = paintBitmap;
        }

        public void Fill(int x, int y, Color originalColor, Color fillColor)
        {
            Queue<System.Windows.Point> queue = new Queue<System.Windows.Point>();
             
            queue.Enqueue(new System.Windows.Point(x, y));

            while (queue.Count != 0)
            {
                System.Windows.Point p = queue.Dequeue();

                Color currentColor = paintBitmap.GetColor((int)p.X, (int)p.Y);

                if (equalColor(currentColor, originalColor))
                {
                    PutPixel(x, y, fillColor);
                }

                // ADD 4 NEIGHBORS

                System.Windows.Point pointToAdd = new System.Windows.Point(p.X - 1, p.Y);
                Color nbColor = paintBitmap.GetColor((int)p.X - 1, (int)p.Y);
                if (equalColor(nbColor, originalColor))
                {
                    queue.Enqueue(pointToAdd);
                }

                pointToAdd = new System.Windows.Point(p.X + 1, p.Y);
                nbColor = paintBitmap.GetColor((int)p.X + 1, (int)p.Y);
                if (equalColor(nbColor, originalColor))
                {
                    queue.Enqueue(pointToAdd);
                }

                pointToAdd = new System.Windows.Point(p.X, p.Y - 1);
                nbColor = paintBitmap.GetColor((int)p.X, (int)p.Y - 1);
                if (equalColor(nbColor, originalColor))
                {
                    queue.Enqueue(pointToAdd);
                }

                pointToAdd = new System.Windows.Point(p.X, p.Y + 1);
                nbColor = paintBitmap.GetColor((int)p.X, (int)p.Y + 1);
                if (equalColor(nbColor, originalColor))
                {
                    queue.Enqueue(pointToAdd);
                }
            }

            return;
        }

        private bool hasPoint(Queue<System.Windows.Point> queue, System.Windows.Point point) {
            for(int i =0;i< queue.Count;i++){
                System.Windows.Point pointInQueue = queue.ElementAt(i);

                if (pointInQueue.X == point.X && point.Y == pointInQueue.Y)
                    return true;
            }
            return false;
        }

        private bool equalColor(Color color1, Color color2) 
        {
            return (color1.R == color2.R &&
                    color1.G == color2.G &&
                    color1.B == color2.B);
        }
    }
}
