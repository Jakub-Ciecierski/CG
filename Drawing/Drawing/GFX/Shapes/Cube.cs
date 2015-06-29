using Drawing.SimpleMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing.GFX.Shapes
{
    public class Cube : Shape
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        private static double[][] vertices;
        private static int[] drawOrder;

        private double width;

        public double Width
        {
            get { return width; }
            set { width = value; }
        }


        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        public Cube(Drawer drawer, double width)
            : base(drawer)
        {
            VERTEX_COUNT = 8;
            Width = width;

            initData();
        }

        public Cube(Drawer drawer, double x, double y, double z, double width)
            : base(drawer, x, y, z)
        {
            VERTEX_COUNT = 8;
            Width = width;

            initData();
        }
        public Cube(double width)
            : base()
        {
            VERTEX_COUNT = 8;
            Width = width;

            initData();
        }

        public Cube(double x, double y, double z, double width)
            : base(x, y, z)
        {
            VERTEX_COUNT = 8;
            Width = width;

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
            vertices = new double[][]
            {
                new double[] {0, 1 , 1},
                new double[] {1, 1 , 1},
                new double[] {0, 0 , 1},
                new double[] {1, 0 , 1},

                new double[] {0, 1 ,0},
                new double[] {1, 1 ,0},
                new double[] {0, 0 ,0},
                new double[] {1, 0 ,0}
            };

            verticesAffine = new Matrix[VERTEX_COUNT];

            verticesAffine[0] = new Matrix
            (
                new double[4][] {   new double[] { vertices[0][0] },
                                    new double[] { vertices[0][1] }, 
                                    new double[] { vertices[0][2] }, 
                                    new double[] { 1 } }
            );
            verticesAffine[1] = new Matrix
            (
                new double[4][] {   new double[] { vertices[1][0] }, 
                                    new double[] { vertices[1][1] }, 
                                    new double[] { vertices[1][2] }, 
                                    new double[] { 1 } }
            );
            verticesAffine[2] = new Matrix
            (
                new double[4][] {   new double[] { vertices[2][0] }, 
                                    new double[] { vertices[2][1] }, 
                                    new double[] { vertices[2][2] }, 
                                    new double[] { 1 } }
            );
            verticesAffine[3] = new Matrix
            (
                new double[4][] {   new double[] { vertices[3][0] }, 
                                    new double[] { vertices[3][1] }, 
                                    new double[] { vertices[3][2] }, 
                                    new double[] { 1 } }
            );
            verticesAffine[4] = new Matrix
            (
                new double[4][] {   new double[] { vertices[4][0] }, 
                                    new double[] { vertices[4][1] }, 
                                    new double[] { vertices[4][2] }, 
                                    new double[] { 1 } }
            );
            verticesAffine[5] = new Matrix
            (
                new double[4][] {   new double[] { vertices[5][0] }, 
                                    new double[] { vertices[5][1] }, 
                                    new double[] { vertices[5][2] }, 
                                    new double[] { 1 } }
            );
            verticesAffine[6] = new Matrix
            (
                new double[4][] {   new double[] { vertices[6][0] }, 
                                    new double[] { vertices[6][1] }, 
                                    new double[] { vertices[6][2] }, 
                                    new double[] { 1 } }
            );
            verticesAffine[7] = new Matrix
            (
                new double[4][] {   new double[] { vertices[7][0] }, 
                                    new double[] { vertices[7][1] }, 
                                    new double[] { vertices[7][2] }, 
                                    new double[] { 1 } }
            );
        }

        protected override void initDrawOrder()
        {
            drawOrder = new int[]
            {
                0, 1, 
                1, 3, 
                3, 2, 
                2, 0,
                4, 5,
                5, 7,
                7, 6,
                6, 4,
                4, 0,
                5, 1,
                3, 7,
                6, 2
            };
        }

        protected override void draw(Matrix[] transformedPoints)
        {
            int x1, y1, x2, y2;
            for(int i = 0; i < drawOrder.Length - 1; i+=2)
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
                    new double[] {Width,    0,      0,      0},
                    new double[] {0,        Width,  0,      0},
                    new double[] {0,        0,      Width,  0},
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
