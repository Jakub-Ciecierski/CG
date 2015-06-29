using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing.SimpleMath
{
    public class Matrix
    {
        private int n;

        public int N
        {
            get { return n; }
            set { n = value; }
        }

        private int m;

        public int M
        {
            get { return m; }
            set { m = value; }
        }

        private double[][] _matrix;

        public double[][] MatValues
        {
            get { return _matrix; }
            private set { _matrix = value; }
        }

        public Matrix(double[][] _matrix)
        {
            this._matrix = _matrix;
            N = _matrix.Length;

            M = _matrix[0].Length;
        }

    }
}
