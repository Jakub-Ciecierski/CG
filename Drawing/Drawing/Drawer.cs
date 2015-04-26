using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing
{
    public class Drawer
    {
        private int thickness;

        public int Thickness
        {
            get { return thickness; }
            set { thickness = value; }
        }

        public Drawer(int thickness)
        {
            Thickness = thickness;
        }

        public void DrawSymmetricLine(int x1, int y1, int x2, int y2, Action<int, int> putPixel)
        {
            // if the line too steep, "kick" the plane around
            bool steep = Math.Abs(y2 - y1) > Math.Abs(x2 - x1);
            if (steep)
            {
                int tmpX1 = x1;
                x1 = y1;
                y1 = tmpX1;

                int tmpX2 = x2;
                x2 = y2;
                y2 = tmpX2;
            }
            // if x1 is greater than x2, the starting point
            if (x1 > x2)
            {
                int tmpX1 = x1;
                x1 = x2;
                x2 = tmpX1;

                int tmpY1 = y1;
                y1 = y2;
                y2 = tmpY1;
            }

            int dx = x2 - x1;
            int dy = Math.Abs(y2 - y1);

            int d = 2 * dy - dx;
            int dE = 2 * dy;
            int dNE = 2 * (dy - dx);

            // front and back
            int xf = x1, yf = y1;
            int xb = x2, yb = y2;

            int ystep = (y1 < y2) ? 1 : -1;

            if(steep)
            {
                putThickPixel(yf, xf, putPixel);
                putThickPixel(yb, xb, putPixel);
            }
            else
            {
                putThickPixel(xf, yf, putPixel);
                putThickPixel(xb, yb, putPixel);
            }

            while (xf < xb)
            {
                ++xf;
                --xb;
                if (d < 0)
                    d += dE;
                else
                {
                    d += dNE;
                    yf += ystep;
                    yb -= ystep;
                }
                if (steep)
                {
                    putThickPixel(yf, xf, putPixel);
                    putThickPixel(yb, xb, putPixel);
                }
                else
                {
                    putThickPixel(xf, yf, putPixel);
                    putThickPixel(xb, yb, putPixel);
                }
            }
        }

        public void DrawCircle(int xc, int yc, int R, Action<int, int> putPixel)
        {
            int dE = 3;
            int dSE = 5 - 2 * R;
            int d = 1 - R;

            int x = 0;
            int y = R;

            while (y > x)
            {
                if (d < 0)
                {
                    d += dE;
                    dE += 2;
                    dSE += 2;
                }
                else
                {
                    d += dSE;
                    dE += 2;
                    dSE += 4;
                    --y;
                }
                ++x;
                drawAllOctans(xc, yc, x, y, putPixel);
            }
        }

        private void drawAllOctans(int xc, int yc, int x, int y, Action<int, int> putPixel)
        {
            putThickPixel(xc + x, yc + y, putPixel);
            putThickPixel(xc - x, yc + y, putPixel);
            putThickPixel(xc + x, yc - y, putPixel);
            putThickPixel(xc - x, yc - y, putPixel);
            putThickPixel(xc + y, yc + x, putPixel);
            putThickPixel(xc - y, yc + x, putPixel);
            putThickPixel(xc + y, yc - x, putPixel);
            putThickPixel(xc - y, yc - x, putPixel);
        }

        private void putThickPixel(int x, int y, Action<int, int> putPixel)
        {
            if (Thickness == 1)
                thickness1(x, y, putPixel);
            else if (Thickness == 3)
                thickness3(x, y, putPixel);
            else if (Thickness == 5)
                thickness5(x, y, putPixel);
            else if (Thickness == 7)
                thickness7(x, y, putPixel);
            else
                thickness1(x, y, putPixel);
        }

        private void thickness1(int x, int y, Action<int, int> putPixel)
        {
            putPixel(x, y);
        }

        private void thickness3(int x, int y, Action<int, int> putPixel)
        {
            putPixel(x, y + 1);
            putPixel(x + 1, y);
            putPixel(x, y);
            putPixel(x, y - 1);
            putPixel(x - 1, y);
        }

        private void thickness5(int x, int y, Action<int, int> putPixel)
        {
            putPixel(x - 1, y + 2);
            putPixel(x, y + 2);
            putPixel(x+1, y + 2);

            putPixel(x - 2, y + 1);
            putPixel(x - 1, y + 1);
            putPixel(x, y + 1);
            putPixel(x + 1, y + 1);
            putPixel(x + 2, y + 1);

            putPixel(x - 2, y);
            putPixel(x - 1, y);
            putPixel(x, y);
            putPixel(x + 1, y);
            putPixel(x + 2, y);

            putPixel(x - 2, y - 1);
            putPixel(x - 1, y - 1);
            putPixel(x, y - 1);
            putPixel(x + 1, y - 1);
            putPixel(x + 2, y - 1);

            putPixel(x - 1, y - 2);
            putPixel(x, y - 2);
            putPixel(x + 1, y - 2);
        }

        private void thickness7(int x, int y, Action<int, int> putPixel)
        {
            putPixel(x - 1, y + 3);
            putPixel(x,     y + 3);
            putPixel(x + 1, y + 3);

            putPixel(x - 2, y + 2);
            putPixel(x - 1, y + 2);
            putPixel(x,     y + 2);
            putPixel(x + 1, y + 2);
            putPixel(x + 2, y + 2);

            putPixel(x - 3, y + 1);
            putPixel(x - 2, y + 1);
            putPixel(x - 1, y + 1);
            putPixel(x,     y + 1);
            putPixel(x + 1, y + 1);
            putPixel(x + 2, y + 1);
            putPixel(x + 3, y + 1);

            putPixel(x - 3, y);
            putPixel(x - 2, y);
            putPixel(x - 1, y);
            putPixel(x,     y);
            putPixel(x + 1, y);
            putPixel(x + 2, y);
            putPixel(x + 3, y);

            putPixel(x - 3, y - 1);
            putPixel(x - 2, y - 1);
            putPixel(x - 1, y - 1);
            putPixel(x,     y - 1);
            putPixel(x + 1, y - 1);
            putPixel(x + 2, y - 1);
            putPixel(x + 3, y - 1);

            putPixel(x - 2, y - 2);
            putPixel(x - 1, y - 2);
            putPixel(x, y - 2);
            putPixel(x + 1, y - 2);
            putPixel(x + 2, y - 2);

            putPixel(x - 1, y - 3);
            putPixel(x, y - 3);
            putPixel(x + 1, y - 3);
        }
    }
}
