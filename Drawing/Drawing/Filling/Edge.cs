using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing.Filling
{
    public class Edge
    {
        public int max_y;

        public int min_y;

        public double curr_x;

        public double m;

        public double d_m;

        public Edge next = null;

        public Edge(int max_y, int min_y, double m)
        {
            this.max_y = max_y;
            this.min_y = min_y;
            this.m = m;

            this.d_m = 1 / m;
        }
    }
}
