using Assets.DataManagement;
using Assets.Graphics.Drawers;
using Assets.Graphics.Positioners;
using Assets.Graphics.Shapes;
using Assets.Positional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Graphics
{
    class Graphic
    {
        Positioner positioner;
        Shape shape;
        Drawer drawer;

        public Graphic(GraphicTypes graphicType, WorldAligner aligner)
        {
            GraphicFactory.Instance.aligner ??= aligner;
            this.shape = GraphicFactory.Instance.getShape(graphicType);
            this.positioner = GraphicFactory.Instance.getPositioner(graphicType);
            this.drawer = GraphicFactory.Instance.getDrawer(graphicType);
        }

        public void display(DTO dto)
        {
            this.shape.Get(dto);
            shape = positioner.Position(this.shape);
            AISDTOs dtos = (AISDTOs)dto;
            Debug.Log($"Got {dtos.vessels.Count} vessels from BarentsWatch");
        }
    }
}
