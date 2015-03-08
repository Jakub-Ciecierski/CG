using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filter.ConvolutionFilters
{
    public class SharpenFilter : ConvolutionFilter
    {
        public SharpenFilter() : base(3,3,4) // n, m, anchor
        {
            // define kernel
            kernelMatrix = new List<int>
            {
                0,-1,0,
                -1,5,-1,
                0,-1,0
            };

            setDevisorAndOffset();
            
        }

        public override Bitmap ApplyFilter(Bitmap image)
        {
            return compute(image);
        }
    }
}
