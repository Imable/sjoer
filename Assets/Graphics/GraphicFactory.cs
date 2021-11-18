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

        public Filler GetFiller(DataType dataType)
        {
            Filler filler;

            switch (dataType)
            {
                case DataType.AIS:
                    filler = aisFiller;
                    break;
                default:
                    throw new ArgumentException("No such data type", nameof(dataType));
            }

            return filler;
        }

        public Positioner getPositioner(DataType dataType)
        {
            Positioner positioner;

            switch (dataType)
            {
                case DataType.AIS:
                    aisPositioner.SetAligner(aligner);
                    positioner = aisPositioner;
                    break;
                default:
                    throw new ArgumentException("No such data source", nameof(dataType));
            }

            return positioner;
        }

        public ShapeProvider getShapeProvider(DataType dataType)
        {
            ShapeProvider shape;

            switch (dataType)
            {
                case DataType.AIS:
                    shape = aisShapeProvider;
                    break;
                default:
                    throw new ArgumentException("No such data type", nameof(dataType));
            }

            return shape;
        }
    }
}
