using Drawing.GFX;
using Drawing.GFX.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingPaint
{
    public class SceneLoader
    {
        private const string SHAPE_TAG = "SHAPE";
        private const string RADIUS_TAG = "RADIUS";
        private const string HEIGHT_TAG = "HEIGHT";
        private const string WIDTH_TAG = "WIDTH";
        private const string POSITION_TAG = "POSITION";
        private const string X_TAG = "X";
        private const string Y_TAG = "Y";
        private const string Z_TAG = "Z";
        private const string ALPHA_TAG = "ALPHA";
        private const string BETA_TAG = "BETA";
        private const string SEPERATOR_TAG = "-";
        private const string EOF_TAG = "EOF";

        private const string CONE_TAG = "CONE";
        private const string CUBE_TAG = "CUBE";
        private const string CYLINDER_TAG= "CYLINDER";
        private const string SPHERE_TAG = "SPHERE";

        public static List<Shape> LoadScene(string filename)
        {
            List<Shape> shapes = new List<Shape>();

            string[] lines = System.IO.File.ReadAllLines(filename, Encoding.GetEncoding("ISO-8859-1"));

            double x = 0, y = 0, z = 0;
            double radius = 0, height = 0, width = 0;
            double alpha = 0, beta = 0;

            int i = 0;
            while (i < lines.Length)
            {
                // new shape
                if (lines[i++].Equals(SHAPE_TAG))
                {
                    string shapeName = lines[i++];

                    while (!lines[i].Equals(SEPERATOR_TAG) && !lines[i].Equals(EOF_TAG))
                    {
                        switch (lines[i++])
                        {
                            case RADIUS_TAG:
                                radius = Convert.ToDouble(lines[i]);
                                break;
                            case WIDTH_TAG:
                                width = Convert.ToDouble(lines[i]);
                                break;
                            case HEIGHT_TAG:
                                height = Convert.ToDouble(lines[i]);
                                break;
                            case X_TAG:
                                x = Convert.ToDouble(lines[i]);
                                break;
                            case Y_TAG:
                                y = Convert.ToDouble(lines[i]);
                                break;
                            case Z_TAG:
                                z = Convert.ToDouble(lines[i]);
                                break;
                            case ALPHA_TAG:
                                alpha = Convert.ToDouble(lines[i]);
                                break;
                            case BETA_TAG:
                                beta = Convert.ToDouble(lines[i]);
                                break;
                        }
                        if (++i >= lines.Length) break;
                    }
                    switch (shapeName)
                    {
                        case CONE_TAG:
                            Cone cone = new Cone(x, y, z, radius, height);
                            cone.Alpha = alpha;
                            cone.Beta = beta;
                            shapes.Add(cone);
                            break;
                        case CUBE_TAG:
                            Cube cube = new Cube(x, y, z, width);
                            cube.Alpha = alpha;
                            cube.Beta = beta;
                            shapes.Add(cube);
                            break;
                        case CYLINDER_TAG:
                            Cylinder cylinder = new Cylinder(x, y, z, radius, height);
                            cylinder.Alpha = alpha;
                            cylinder.Beta = beta;
                            shapes.Add(cylinder);
                            break;
                        case SPHERE_TAG:
                            Sphere sphere = new Sphere(x, y, z, radius);
                            sphere.Alpha = alpha;
                            sphere.Beta = beta;
                            shapes.Add(sphere);
                            break;
                    }
                }
            }
            return shapes;
        }
    }
}
