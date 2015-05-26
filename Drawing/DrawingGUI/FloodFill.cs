using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DrawingGUI
{
    public class FloodFill
    {
         Action<int, int> PutPixel;

        public FloodFill(Action<int, int> putPixel)
        {
            PutPixel = putPixel;
        }

        public static void fillArea(int x, int y, Color original, Color fill)
        {
            if (x != 0)
                x--;
            if (y != 0)
                y--;
            
            Queue<Point> queue = new LinkedList<Point>();

            queue.add(new Point(x, y));

            while (!queue.isEmpty())
            {
                Point p = queue.remove();
                if (picture[p.y][p.x] == original)
                {
                    PutPixel(x, y);
                    picture[p.y][p.x] = fill;
                    queue.add(new Point(p.x - 1, p.y));
                    queue.add(new Point(p.x + 1, p.y));
                    queue.add(new Point(p.x, p.y - 1));
                    queue.add(new Point(p.x, p.y + 1));
                }
            }

            return;
        }
    }
}
