using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filter.FunctionFilters
{
    public class GammaCorrectionFilter : FunctionFilter
    {
        private double gamma;
        private int A;

        public GammaCorrectionFilter(double gamma, int A)
        {
            this.gamma = gamma;
            this.A = A;
        }
        private byte filterFunction(byte x)
        {
            byte max = 255;
            byte min = 0;

            int y = (int)(255 * (Math.Pow((double)x / (double)255, gamma)));          

            return (byte)y;
        }

        public override Bitmap ApplyFilter(Bitmap image)
        {
            return compute(image, filterFunction);
        }
    }
}
