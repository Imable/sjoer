using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Graphics.Drawers;
using Assets.Graphics.Positioners;
using Assets.Graphics.Shapes;
using Assets.Positional;

namespace Assets.Graphics
{
    class GraphicFactory : HelperClasses.CSSingleton<GraphicFactory>
    {
        public Player aligner = null;

        public Positioner getPositioner(GraphicTypes graphicType)
        {
            Positioner positioner;

            switch (graphicType)
            {
                case GraphicTypes.Point3D:
                    positioner = new AISPositioner(aligner);
                    break;
                case GraphicTypes.HUD2D:
                    positioner = new Positioner(aligner);
                    break;
                default:
                    throw new ArgumentException("No such data source", nameof(graphicType));
            }

            return positioner;
        }

        public Shape getShape(GraphicTypes graphicType)
        {
            Shape shape;

            switch (graphicType)
            {
                case GraphicTypes.Point3D:
                    shape = new AISShape();
                    break;
                case GraphicTypes.HUD2D:
                    shape = new Shape();
                    break;
                default:
                    throw new ArgumentException("No such data source", nameof(graphicType));
            }

            return shape;
        }

        public Drawer getDrawer(GraphicTypes graphicType)
        {
            Drawer drawer = new Drawer(aligner);

            switch (graphicType)
            {
                case GraphicTypes.Point3D:
                    drawer = new AISDrawer(aligner);
                    break;
                case GraphicTypes.HUD2D:
                    drawer = new Drawer(aligner);
                    break;
                default:
                    throw new ArgumentException("No such data source", nameof(graphicType));
            }

            return drawer;
        }
    }
}
