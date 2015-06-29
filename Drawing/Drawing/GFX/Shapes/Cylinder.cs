using Drawing.SimpleMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing.GFX.Shapes
{
    public class Cylinder : Shape
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        private const int BASE_TRIANGLES_COUNT = 20;

        private const int DATA_PER_VERTEX = 4;

        private int[] drawOrder;

        private double radius;

        public double Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        private double height;

        public double Height
        {
            get { return height; }
            set { height = value; }
        }

        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        public Cylinder(Drawer drawer, double radius, double height)
            : base(drawer)
        {
            VERTEX_COUNT = BASE_TRIANGLES_COUNT * 2;

            Radius = radius;
            Height = height;

            initData();
        }

        public Cylinder(Drawer drawer, double x, double y, double z, double radius, double height)
            : base(drawer, x, y, z)
        {
            VERTEX_COUNT = BASE_TRIANGLES_COUNT * 2;

            Radius = radius;
            Height = height;

            initData();
        }

        public Cylinder(double radius, double height)
            : base()
        {
            VERTEX_COUNT = BASE_TRIANGLES_COUNT * 2;

            Radius = radius;
            Height = height;

            initData();
        }

        public Cylinder(double x, double y, double z, double radius, double height)
            : base(x, y, z)
        {
            VERTEX_COUNT = BASE_TRIANGLES_COUNT * 2;

            Radius = radius;
            Height = height;

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
            verticesAffine = new Matrix[VERTEX_COUNT];
            int index = 0;

            for (int i = 0; i < VERTEX_COUNT; i++)
            {
                double angleRad = ((double)i / (double)BASE_TRIANGLES_COUNT) * Math.PI * 2.0;

                double x = Radius * Math.Cos(angleRad);
                double y = Radius * Math.Sin(angleRad);
                double z = index++ >= BASE_TRIANGLES_COUNT ? Height : 0;

                verticesAffine[i] = new Matrix
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

        protected override void initDrawOrder()
        {
            const int SIZE = 3 * BASE_TRIANGLES_COUNT + 3;
            drawOrder = new int[SIZE];

            // Base 1 
            for (int i = 0; i < BASE_TRIANGLES_COUNT; i++)
            {
                drawOrder[i] = i;
            }
            drawOrder[BASE_TRIANGLES_COUNT] = 0;

            drawOrder[BASE_TRIANGLES_COUNT + 1] = -1;

            // Base 2
            for (int i = BASE_TRIANGLES_COUNT + 2; i < 2 * BASE_TRIANGLES_COUNT + 2; i++)
            {
                drawOrder[i] = i - 2;
            }
            drawOrder[2 * BASE_TRIANGLES_COUNT + 2] = BASE_TRIANGLES_COUNT;

            // Basis Connection
            for (int i = 2 * BASE_TRIANGLES_COUNT + 3; i < 3 * BASE_TRIANGLES_COUNT + 3; i += 2)
            {
                drawOrder[i] = (i - 3) - 2 * BASE_TRIANGLES_COUNT;
                drawOrder[i + 1] = i - 3 - BASE_TRIANGLES_COUNT;
            }
        }

        protected override void draw(Matrix[] transformedPoints)
        {
            int x1, y1, x2, y2;

            for (int i = 0; i < drawOrder.Length - 1; i++)
            {
                if (drawOrder[i] == -1 || drawOrder[i + 1] == -1) continue;

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
