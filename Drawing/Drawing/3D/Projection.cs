using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing._3D
{
    public class Projection
    {
        private double alpha;

	    public double Alpha
	    {
		    get { return alpha;}
		    set { alpha = value;}
	    }

        private double fov;

	    public double FOV
	    {
		    get { return fov;}
		    set { fov = value;}
	    }


        int width;
        int height;

        double d;

        double cx;
        double cy;

        double[][] RotationMatrix;

        double[][] ProjectionMatrix;

        double[][] TranslationMatrix;
   
        public Projection(int width, int height)
        {
            this.width = width;
            this.height = height;

            Alpha = 0;
            FOV = 90;

            init();
        }

        private void init() 
        {
            

            d = (width / 2) * (1 / Math.Tan(FOV/2));
            cx = width / 2;
            cy = height / 2;

            RotationMatrix = new double[4][]
            {
                new double[] {Math.Cos(alpha), 0 ,-Math.Sin(alpha), 0},
                new double[] {0, 1 ,0, 0},
                new double[] {Math.Sin(alpha), 0 ,Math.Cos(alpha), 0},
                new double[] {0, 0 ,0, 1},
            };

            ProjectionMatrix = new double[4][]
            {
                new double[] {d, 0 ,cx, 0},
                new double[] {0, -d ,cy, 0},
                new double[] {0, 0 ,0, 1},
                new double[] {0, 0 ,1, 0},
            };

            double z = 5;

            TranslationMatrix = new double[4][]
            {
                new double[] {1, 0 ,0, 0},
                new double[] {0, 1 ,0, 0},
                new double[] {0, 0 ,1, z},
                new double[] {0, 0 ,0, 1},
            };
        }

        public void Update()
        {
            init();
        }
           

        public double[][] Render(Cube cube)
        {
            int size = cube.coords.Length;

            double[][] projectedPoints = new double[size][];
            

            // for each point
            // multiply by RotationMatrix
            for (int i = 0; i < size; i++)
            {
                double[] point = cube.coords[i];

                double[] rotatedPoint = new double[4] { 0, 0, 0, 0 };

                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        if (k == 3)
                            rotatedPoint[j] += 1 * RotationMatrix[j][k];
                        else
                            rotatedPoint[j] += point[k] * RotationMatrix[j][k];
                    }
                }

                projectedPoints[i] = rotatedPoint;
            }



            // Translation
            for (int i = 0; i < size; i++)
            {
                double[] translatedPoint = new double[4] { 0, 0, 0, 0 };

                translatedPoint = projectedPoints[i];
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        translatedPoint[j] += projectedPoints[i][k] * TranslationMatrix[j][k];
                    }
                }

                projectedPoints[i] = translatedPoint;
            }





            // for each point
            // multiply by ProjectionMatrix
            for(int i = 0; i < size; i++)
            {
                double[] q_prim = new double[4] { 0, 0, 0, 0 };
                for(int j = 0; j < 4; j++)
                {
                    for(int k = 0; k < 4; k++)
                    {
                        q_prim[j] += projectedPoints[i][k] * ProjectionMatrix[j][k]; 
                    }
                }
                projectedPoints[i] = q_prim;
            }

            // change q_prim into q, i.e. divide by qz
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    projectedPoints[i][j] /= projectedPoints[i][3];
                }
            }

            return projectedPoints;
        }
    }
}
