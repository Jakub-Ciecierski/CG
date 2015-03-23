using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorQuantization
{
    class Pixel
    {
        private Color color;

        private int x;
        private int y;

        public Color Color
        {
            get { return color;}
            set { color = value;}
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public Pixel(Color color, int x, int y)
        {
            this.color = color;
            this.x = x;
            this.y = y;
        }

        public Pixel(Color color)
        {
            this.color = color;
        }
    }
}
