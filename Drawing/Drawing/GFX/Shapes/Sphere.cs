using Drawing.SimpleMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing.GFX.Shapes
{
    public class Sphere : Shape
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        private const int RINGS = 10;
        private const int SECTIONS = 10;

        private const int DATA_PER_VERTEX = 4;

        private int[] drawOrder;

        private double radius;

        public double Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/
        public Sphere(){}
        public Sphere(Drawer drawer, double radius)
            : base(drawer)
        {
            VERTEX_COUNT = RINGS * SECTIONS;
            Radius = radius;

            initData();
        }

        public Sphere(Drawer drawer, double x, double y, double z, double radius)
            : base(drawer, x, y, z)
        {
            VERTEX_COUNT = RINGS * SECTIONS;
            Radius = radius;

            initData();
        }

        public Sphere(double radius)
            : base()
        {
            VERTEX_COUNT = RINGS * SECTIONS;
            Radius = radius;

            initData();
        }

        public Sphere(double x, double y, double z, double radius)
            : base( x, y, z)
        {
            VERTEX_COUNT = RINGS * SECTIONS;
            Radius = radius;

            initData();
        }

        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/

        protected override void initData()
        {
            initVertices();
            initDrawOrder();
        }

        protected override void initVertices()
        {
            const int SIZE = RINGS * SECTIONS;
            verticesAffine = new Matrix[SIZE];

            double R = 1.0 / (double)(RINGS - 1);
            double S = 1.0 / (double)(SECTIONS - 1);

            for (int r = 0; r < RINGS; r++ )
            {
                for (int s = 0; s < SECTIONS; s++)
                {
                    double x = Math.Cos(2 * Math.PI * s * S) * Math.Sin(Math.PI * r * R);
                    double y = Math.Sin(-Math.PI/2.0 + Math.PI * r * R);
                    double z = Math.Sin(2 * Math.PI * s * S) * Math.Sin(Math.PI * r * R);

                    x *= Radius;
                    y *= Radius;
                    z *= Radius;

                    verticesAffine[r*RINGS + s] = new Matrix
                    (
                        new double[4][]
                        {
                            new double[] { x },
                            new double[] { y },
                            new double[] { z },
                            new double[] { 1 }
                        }
                    );
                }
            }
        }

        protected override void initDrawOrder()
        {
            const int SIZE = RINGS * SECTIONS * DATA_PER_VERTEX;
            drawOrder = new int[SIZE];

            int i = 0;

            for (int r = 0; r < RINGS - 1; r++)
            {
                for (int s = 0; s < SECTIONS - 1; s++)
                {
                    drawOrder[i++] = r * SECTIONS + s;
                    drawOrder[i++] = r * SECTIONS + (s + 1);
                    drawOrder[i++] = (r + 1) * SECTIONS + (s + 1);
                    drawOrder[i++] = (r + 1) * SECTIONS + s;
                }
            }
        }

        protected override void draw(Matrix[] transformedPoints)
        {
            int x1, y1, x2, y2;
            for (int i = 0; i < drawOrder.Length - 1; i ++)
            {
                x1 = (int)transformedPoints[drawOrder[i]].MatValues[0][0];
                y1 = (int)transformedPoints[drawOrder[i]].MatValues[1][0];

                x2 = (int)transformedPoints[drawOrder[i + 1]].MatValues[0][0];
                y2 = (int)transformedPoints[drawOrder[i + 1]].MatValues[1][0];

                drawer.DrawSymmetricLine(x1, y1, x2, y2);
            }
        }

        protected override void update()
        {
            RotationMatrixY = new Matrix
            (
               new double[4][]
                {
                    new double[] {Math.Cos(Alpha), 0 ,Math.Sin(Alpha), 0},
                    new double[] {0, 1 ,0, 0},
                    new double[] {-Math.Sin(Alpha), 0 ,Math.Cos(Alpha), 0},
                    new double[] {0, 0 ,0, 1},
                }
            );

            RotationMatrixX = new Matrix
            (
                new double[4][]
                {
                    new double[] {1, 0 , 0, 0},
                    new double[] {0, Math.Cos(Beta),-Math.Sin(Beta), 0},
                    new double[] {0, Math.Sin(Beta), Math.Cos(Beta), 0},
                    new double[] {0, 0 ,0, 1},
                }
            );

            ScaleMatrix = new Matrix
            (
                new double[4][]
                {
                    new double[] {1,    0,      0,      0},
                    new double[] {0,        1,  0,      0},
                    new double[] {0,        0,      1,  0},
                    new double[] {0,        0,      0,      1},
                }
            );

            TranslationMatrix = new Matrix
            (
                new double[4][]
                {
                    new double[] {1, 0 ,0, X},
                    new double[] {0, 1 ,0, Y},
                    new double[] {0, 0 ,1, Z},
                    new double[] {0, 0 ,0, 1},
                }
            );

            RotationMatrix = MM.Multiply(RotationMatrixX, RotationMatrixY);
            ModelMatrix = MM.Multiply(RotationMatrix, ScaleMatrix);

            ModelMatrix = MM.Multiply(TranslationMatrix, ModelMatrix);
        }

        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/

    }
}
