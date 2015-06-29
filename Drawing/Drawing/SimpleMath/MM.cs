using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing.SimpleMath
{
    public class MM
    {
        public static double[][] Multiply(double[][] mat1, double[][] mat2)
        {
            int dim1 = mat1.Length;
            int dim2 = mat2.Length;

            double[][] mat = new double[dim1][];

            for(int i = 0; i < dim1;i++)
            {
                int rowSize = mat2[0].Length;
                double[] vec = new double[rowSize];
                mat[i] = new double[mat2[0].Length];

                for(int l = 0; l < rowSize;l++)
                {
                    for (int j = 0; j < dim2; j++)
                    {
                        mat[i][l] += mat1[i][j] * mat2[j][l];
                    }
                }
            }

            return mat;
        }

        public static Matrix Multiply(Matrix mat1, Matrix mat2)
        {
            int dim1 = mat1.N;
            int dim2 = mat2.N;

            double[][] matValues = new double[dim1][];

            for (int i = 0; i < dim1; i++)
            {
                int rowSize = mat2.M;
                double[] vec = new double[rowSize];
                matValues[i] = new double[rowSize];

                for (int l = 0; l < rowSize; l++)
                {
                    for (int j = 0; j < dim2; j++)
                    {
                        matValues[i][l] += mat1.MatValues[i][j] * mat2.MatValues[j][l];
                    }
                }
            }

            return new Matrix(matValues);
        }

        public static double[] MultiplyVec(double[][] mat1, double[] mat2)
        {
            int dim1 = mat1.Length;
            int dim2 = mat2.Length;

            double[] mat = new double[dim1];

            for (int i = 0; i < dim1; i++)
            {
                int rowSize = 1;
                double[] vec = new double[rowSize];

                for (int l = 0; l < rowSize; l++)
                {
                    for (int j = 0; j < dim2; j++)
                    {
                        mat[i] += mat1[i][j] * mat2[j];
                    }
                }
            }

            return mat;
        }
    }
}
