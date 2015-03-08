using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filter.ConvolutionFilters
{
    public class EmbossFilter : ConvolutionFilter
    {
        public EmbossFilter() : base(3,3,4) // n, m, anchor
        {
            kernelMatrix = new List<int>
            {
                -1,0,1,
                -1,1,1,
                -1,0,1
            };

            setDevisorAndOffset();
        }

        public override Bitmap ApplyFilter(Bitmap image)
        {
            return compute(image);
        }
    }
}
