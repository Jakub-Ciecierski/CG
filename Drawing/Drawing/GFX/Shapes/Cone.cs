using Drawing.SimpleMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing.GFX.Shapes
{
    public class Cone : Shape
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        private const int BASE_TRIANGLES_COUNT = 20;

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

        public Cone(Drawer drawer, double radius, double height)
            : base(drawer)
        {
            VERTEX_COUNT = BASE_TRIANGLES_COUNT + 1;

            Radius = radius;
            Height = height;

            initData();
        }

        public Cone(Drawer drawer, double x, double y, double z, double radius, double height)
            : base(drawer, x, y, z)
        {
            VERTEX_COUNT = BASE_TRIANGLES_COUNT + 1;

            Radius = radius;
            Height = height;

            initData();
        }

        public Cone(double radius, double height)
            : base()
        {
            VERTEX_COUNT = BASE_TRIANGLES_COUNT + 1;

            Radius = radius;
            Height = height;

            initData();
        }

        public Cone(double x, double y, double z, double radius, double height)
            : base(x, y, z)
        {
            VERTEX_COUNT = BASE_TRIANGLES_COUNT + 1;

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

            for (int i = 0; i < BASE_TRIANGLES_COUNT; i++)
            {
                double angleRad = ((double)i / (double)BASE_TRIANGLES_COUNT) * Math.PI * 2.0;

                double x = Radius * Math.Cos(angleRad);
                double y = 0;
                double z = Radius * Math.Sin(angleRad);

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

            verticesAffine[VERTEX_COUNT - 1] = new Matrix
            (
                new double[4][]
                {
                    new double[] { 0 },
                    new double[] { Height },
                    new double[] { 0 },
                    new double[] { 1 }
                }
            );

        }

        protected override void initDrawOrder()
        {
            const int SIZE = BASE_TRIANGLES_COUNT + 1;
            drawOrder = new int[SIZE];

            for (int i = 0; i < BASE_TRIANGLES_COUNT; i++)
            {
                drawOrder[i] = i;
            }
            drawOrder[SIZE - 1] = 0;
        }

        protected override void draw(Matrix[] transformedPoints)
        {
            int x1, y1, x2, y2;
            int midX, midY;

            midX = (int)transformedPoints[VERTEX_COUNT - 1].MatValues[0][0];
            midY = (int)transformedPoints[VERTEX_COUNT - 1].MatValues[1][0];

            for (int i = 0; i < drawOrder.Length - 1; i++)
            {
                x1 = (int)transformedPoints[drawOrder[i]].MatValues[0][0];
                y1 = (int)transformedPoints[drawOrder[i]].MatValues[1][0];

                x2 = (int)transformedPoints[drawOrder[i + 1]].MatValues[0][0];
                y2 = (int)transformedPoints[drawOrder[i + 1]].MatValues[1][0];

                drawer.DrawSymmetricLine(x1, y1, x2, y2);

                drawer.DrawSymmetricLine(x1, y1, midX, midY);
                drawer.DrawSymmetricLine(x2, y2, midX, midY);
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
