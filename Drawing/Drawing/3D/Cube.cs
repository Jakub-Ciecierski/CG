using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing._3D
{
    public class Cube
    {
        public double[][] coords;

        public Cube()
        {
            
            coords = new double[8][]
                {
                    new double[] {-1, 1 ,-1},
                    new double[] {1, 1 ,-1},
                    new double[] {1, -1 ,-1},
                    new double[] {-1, -1 ,-1},

                    new double[] {-1, 1 , 1},
                    new double[] {1, 1 , 1},
                    new double[] {1, -1 , 1},
                    new double[] {-1,- 1 , 1},
                };
            


            /*
            coords = new double[8][]
                {
                    new double[] {20, 40 ,20},
                    new double[] {40, 40 ,20},
                    new double[] {40, 20 ,20},
                    new double[] {20, 20 ,20},

                    new double[] {20, 40 , 40},
                    new double[] {40, 40 , 40},
                    new double[] {40, 20 , 40},
                    new double[] {20 ,20 , 40},
                };*/
        }
    }
}
