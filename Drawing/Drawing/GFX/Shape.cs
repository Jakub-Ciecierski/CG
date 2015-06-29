using Drawing.SimpleMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing.GFX
{
    public abstract class Shape
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        public Drawer drawer;

        protected int VERTEX_COUNT;

        protected Matrix RotationMatrixY;
        protected Matrix RotationMatrixX;
        protected Matrix RotationMatrixZ;
        protected Matrix RotationMatrix;

        protected Matrix TranslationMatrix;

        protected Matrix ScaleMatrix;

        protected Matrix ModelMatrix;

        protected Matrix[] verticesAffine;

        public Matrix[] VerticesAffine
        {
            get { return verticesAffine; }
            set { verticesAffine = value; }
        }

        protected double x;

        public double X
        {
            get { return x; }
            set { x = value; }
        }

        protected double y;

        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        protected double z;

        public double Z
        {
            get { return z; }
            set { z = value; }
        }

        private double alpha;

        public double Alpha
        {
            get { return alpha; }
            set { alpha = value; }
        }

        private double beta;

        public double Beta
        {
            get { return beta; }
            set { beta = value; }
        }

        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        public Shape(Drawer drawer)
        {
            this.drawer = drawer;
            X = 0;
            Y = 0;
            Z = 0;

            Alpha = 0;
            Beta = 0;
        }

        public Shape(Drawer drawer, double x, double y, double z)
        {
            this.drawer = drawer;

            X = x;
            Y = y;
            Z = z;

            Alpha = 0;
            Beta = 0;
        }

        public Shape()
        {
            X = 0;
            Y = 0;
            Z = 0;

            Alpha = 0;
            Beta = 0;
        }

        public Shape(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;

            Alpha = 0;
            Beta = 0;
        }

        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/

        protected abstract void initData();
        protected abstract void initVertices();
        protected abstract void initDrawOrder();

        protected abstract void draw(Matrix[] transformedPoints);

        protected abstract void update();

        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/

        //public abstract void Render(Matrix VP);

        public void Render(Matrix VP)
        {
            update();

            Matrix MVP = MM.Multiply(VP, ModelMatrix);

            Matrix[] transformedPoints = new Matrix[VERTEX_COUNT];

            // Transform the point
            for (int i = 0; i < VERTEX_COUNT; i++)
            {
                transformedPoints[i] = MM.Multiply(MVP, VerticesAffine[i]);

                for (int j = 0; j < 4; j++)
                {
                    transformedPoints[i].MatValues[j][0] /= transformedPoints[i].MatValues[3][0];
                }
            }

            draw(transformedPoints);
        }
    }
}
