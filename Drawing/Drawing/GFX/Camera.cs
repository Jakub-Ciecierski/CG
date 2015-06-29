using Drawing.GFX.Shapes;
using Drawing.SimpleMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing.GFX
{
    public class Camera
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

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

        private double theta;

        public double Theta
        {
            get { return theta; }
            set { theta = value; }
        }

        private double fov;

        public double FOV
        {
            get { return fov; }
            set { fov = value; }
        }

        private double z;

        public double Z
        {
            get { return z; }
            set { z = value; }
        }

        private double x;

        public double X
        {
            get { return x; }
            set { x = value; }
        }

        private double y;

        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        int width;
        int height;

        double d;

        double cx;
        double cy;

        private Matrix RotationMatrixY;
        private Matrix RotationMatrixX;
        private Matrix RotationMatrixZ;
        private Matrix RotationMatrix;

        private Matrix TranslationMatrix;

        private Matrix ViewMatrix;
        private Matrix ProjectionMatrix;

        // The View and Projection part of MVP
        public Matrix VP;

        private double cameraSensitivity;

        public double CameraSensitivity
        {
            get { return cameraSensitivity; }
            set { cameraSensitivity = value; }
        }

        private double movementSensitivity;

        public double MovementSensitivity
        {
            get { return movementSensitivity; }
            set { movementSensitivity = value; }
        }

        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        public Camera(int width, int height)
        {
            this.width = width;
            this.height = height;

            Alpha = 0;
            Beta = 0;
            Theta = 0;

            X = 0;
            Y = 0;
            Z = 5;

            FOV = 130;
    
            CameraSensitivity = 0.009;
            MovementSensitivity = 0.1;

            Update();
        }

        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/

        public void Update()
        {

            d = (width / 2) * (1 / Math.Tan(FOV / 2));
            cx = width / 2;
            cy = height / 2;

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

            RotationMatrixZ = new Matrix
            (
                new double[4][]
                {
                    new double[] {Math.Cos(Theta), -Math.Sin(Theta),0 , 0},
                    new double[] {Math.Sin(Theta), Math.Cos(Theta),0 , 0},
                    new double[] {0, 0 , 1, 0},
                    new double[] {0, 0 , 0, 1},
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

            ProjectionMatrix = new Matrix 
            (
                new double[4][]
                {
                    new double[] {d, 0 ,cx, 0},
                    new double[] {0, -d ,cy, 0},
                    new double[] {0, 0 ,0, 1},
                    new double[] {0, 0 ,1, 0},
                }
            );

            // TransformMatrix = Translate * Rotation
            // Rotation = Rx * Ry * Rz
            RotationMatrix = MM.Multiply(RotationMatrixY, RotationMatrixZ);
            RotationMatrix = MM.Multiply(RotationMatrixX, RotationMatrix);

            ViewMatrix = MM.Multiply(RotationMatrix, TranslationMatrix);

            // The goal of this update method is to compute the View Projection part of MVP
            VP = MM.Multiply(ProjectionMatrix, ViewMatrix);
        }
    }
}
