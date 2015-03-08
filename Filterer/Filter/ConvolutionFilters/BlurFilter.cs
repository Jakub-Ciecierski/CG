using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filter.ConvolutionFilters
{
    public class BlurFilter : ConvolutionFilter
    {
        public BlurFilter() : base(3,3,4) // n, m, anchor
        {
            kernelMatrix = new List<int>
            {
                1,1,1,
                1,1,1,
                1,1,1
            };

            setDevisorAndOffset();
        }

        public override Bitmap ApplyFilter(Bitmap image)
        {
            return compute(image);
        }
    }
}
