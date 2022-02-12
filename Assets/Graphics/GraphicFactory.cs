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

        AISHorizonShapeProvider aisHorizonShapeProvider;
        AISSkyShapeProvider aisSkyShapeProvider;
        AISFiller aisFiller;
        Filler baseFiller;
        AISHorizonPositioner aisHorizonPositioner;
        AISSkyPositioner aisSkyPositioner;

        AISSkyPostProcessor aisSkyPostProcessor;
        AISHorizonPostProcessor aisHorizonPostProcessor;



        public GraphicFactory()
        {
            aisHorizonShapeProvider = new AISHorizonShapeProvider();
            aisSkyShapeProvider = new AISSkyShapeProvider();
            aisFiller = new AISFiller();
            baseFiller = new Filler();
            aisHorizonPositioner = new AISHorizonPositioner();
            aisSkyPositioner = new AISSkyPositioner();
            aisSkyPostProcessor = new AISSkyPostProcessor();
            aisHorizonPostProcessor = new AISHorizonPostProcessor();
        }

        public PostProcessor GetPostProcessor(DataType dataType, DisplayArea displayArea)
        {
            PostProcessor postProcessor;

            switch ((dataType, displayArea))
            {
                case (DataType.AIS, DisplayArea.HorizonPlane):
                    aisHorizonPostProcessor.SetAligner(aligner);
                    postProcessor = aisHorizonPostProcessor;
                    break;
                case (DataType.AIS, DisplayArea.SkyArea):
                    aisSkyPostProcessor.SetAligner(aligner);
                    postProcessor = aisSkyPostProcessor;
                    break;
                default:
                    throw new ArgumentException("No such data type", nameof(dataType));
            }

            return postProcessor;

        }

        public Filler GetFiller(DataType dataType, DisplayArea displayArea, bool target)
        {
            Filler filler;

            switch ((dataType, displayArea, target))
            {
                case (_, _, false):
                    filler = baseFiller;
                    break;
                case (DataType.AIS, DisplayArea.HorizonPlane, true):
                    filler = aisFiller;
                    break;
                case (DataType.AIS, DisplayArea.SkyArea, true):
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
                    aisHorizonPositioner.SetAligner(aligner);
                    positioner = aisHorizonPositioner;
                    break;
                case (DataType.AIS, DisplayArea.SkyArea):
                    aisSkyPositioner.SetAligner(aligner);
                    positioner = aisSkyPositioner;
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
                    shape = aisHorizonShapeProvider;
                    break;
                case (DataType.AIS, DisplayArea.SkyArea):
                    shape = aisSkyShapeProvider;
                    break;
                default:
                    throw new ArgumentException("No such data type", nameof(dataType));
            }

            return shape;
        }
    }
}
