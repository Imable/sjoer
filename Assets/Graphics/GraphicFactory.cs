using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Positional;

namespace Assets.Graphics
{
    class GraphicFactory : HelperClasses.CSSingleton<GraphicFactory>
    {
        public Player aligner = null;

        AISShapeProvider aisShapeProvider;
        AISFiller aisFiller;
        AISPositioner aisPositioner;

        public GraphicFactory()
        {
            aisShapeProvider = new AISShapeProvider();
            aisFiller = new AISFiller();
            aisPositioner = new AISPositioner(aligner);
        }

        public Filler GetFiller(DataType dataType, DisplayArea displayArea)
        {
            Filler filler;

            switch ((dataType, displayArea))
            {
                case (DataType.AIS, DisplayArea.HorizonPlane):
                    filler = aisFiller;
                    break;
                case (DataType.AIS, DisplayArea.SkyArea):
                    filler = aisFiller;
                    break;
                default:
                    throw new ArgumentException("No such data type", nameof(dataType));
            }

            return filler;
        }

        public Positioner getPositioner(DataType dataType, DisplayArea displayArea)
        {
            Positioner positioner;

            switch ((dataType, displayArea))
            {
                case (DataType.AIS, DisplayArea.HorizonPlane):
                    aisPositioner.SetAligner(aligner);
                    positioner = aisPositioner;
                    break;
                case (DataType.AIS, DisplayArea.SkyArea):
                    aisPositioner.SetAligner(aligner);
                    positioner = aisPositioner;
                    break;
                default:
                    throw new ArgumentException("No such data source", nameof(dataType));
            }

            return positioner;
        }

        public ShapeProvider getShapeProvider(DataType dataType, DisplayArea displayArea)
        {
            ShapeProvider shape;

            switch ((dataType, displayArea))
            {
                case (DataType.AIS, DisplayArea.HorizonPlane):
                    shape = aisShapeProvider;
                    break;
                case (DataType.AIS, DisplayArea.SkyArea):
                    shape = aisShapeProvider;
                    break;
                default:
                    throw new ArgumentException("No such data type", nameof(dataType));
            }

            return shape;
        }
    }
}
