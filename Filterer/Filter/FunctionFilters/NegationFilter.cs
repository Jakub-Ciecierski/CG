using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filter.FunctionFilters
{
    public class NegationFilter : FunctionFilter
    {
        public NegationFilter()
        { 
        }

        private byte filterFunction(byte x)
        {
            byte max = 255;
            byte y = (byte)(max - x);

            return y;
        }

        public override Bitmap ApplyFilter(Bitmap image)
        {
            return compute(image, filterFunction);
        }
    }
}
