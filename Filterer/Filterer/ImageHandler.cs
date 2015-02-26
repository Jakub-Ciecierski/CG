using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Filterer
{
    public class ImageHandler
    {
        private Bitmap originalImage;
        private Bitmap filteredImage;

        public ImageHandler()
        {
        }

        public ImageHandler(Bitmap originalImage)
        {
            this.originalImage = originalImage;
            this.filteredImage = originalImage;
        }

        public ImageHandler(Bitmap originalImage, Bitmap filteredImage)
        {
            this.originalImage = originalImage;
            this.filteredImage = filteredImage;
        }

        public void ApplyFilter(Func<Bitmap,Bitmap> filter)
        {
            filteredImage = filter(originalImage);
        }

        public Bitmap getOriginal()
        {
            return this.originalImage;
        }

        public Bitmap getFiltered()
        {
            return this.filteredImage;
        }
    }
}
