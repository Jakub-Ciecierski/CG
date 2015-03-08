using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filter.ConvolutionFilters
{
    public class EdgeDetectionFilter : ConvolutionFilter
    {
        public EdgeDetectionFilter() : base(3,3,4) // n, m, anchor
        {
            kernelMatrix = new List<int>
            {
                0,-1,0,
                0,1,0,
                0,0,0
            };

            setDevisorAndOffset();
        }

        public override Bitmap ApplyFilter(Bitmap image)
        {
            return compute(image);
        }
    }
}
