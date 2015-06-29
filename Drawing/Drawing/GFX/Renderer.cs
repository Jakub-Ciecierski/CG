using Drawing.GFX.Shapes;
using Drawing.SimpleMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing.GFX
{
    public class Renderer
    {
        private Camera camera;

        private List<Shape> shapes = new List<Shape>();

        public Renderer(Camera camera)
        {
            this.camera = camera;
        }

        public void AddShape(Shape shape)
        {
            this.shapes.Add(shape);
        }

        public void Render()
        {
            Matrix VP = camera.VP;

            foreach(Shape shape in shapes)
            {
                shape.Render(VP);
            }
        }

    }
}
