using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filter.FunctionFilters
{
    public class CustomFilter : FunctionFilter
    {
        private List<byte> functionMapper;

        public CustomFilter()
        {
            functionMapper = new List<byte>();
        }

        private byte filterFunction(byte x)
        {
            byte max = 255;
            byte y = (byte)(max - x);

            return y;
        }

        public override Bitmap ApplyFilter(Bitmap image)
        {
            return applyFilter(image, filterFunction);
        }
    }
}
