using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filter.FunctionFilters
{
    public class ContrastFilter : FunctionFilter
    {
        private double contrast;

        public ContrastFilter(double contrast)
        {
            if (contrast < -100) contrast = -100;
            if (contrast > 100) contrast = 100;
            contrast = (100.0 + contrast) / 100.0;
            contrast *= contrast;
            this.contrast = contrast;
        }
        private byte filterFunction(byte x)
        {
            byte max = 255;
            byte min = 0;

            double y = x / 255.0;
            y -= 0.5;
            y *= contrast;
            y += 0.5;
            y *= 255;

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
