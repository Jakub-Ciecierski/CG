using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filter.FunctionFilters
{
    public class BrightnessFilter : FunctionFilter
    {
        private int a;

        public BrightnessFilter(int a)
        {
            if (a > 255)
                a = 255;
            if (a < -255)
                a = -255;
            this.a = a;
        }
        private byte filterFunction(byte x)
        {
            byte max = 255;
            byte min = 0;
            
            int y = (x + a);

            y = (y > max) ? max : y;
            y = (y < min) ? min : y;

            return (byte)y;
        }

        public override Bitmap ApplyFilter(Bitmap image)
        {
            return compute(image, filterFunction);
        }


    }
}
