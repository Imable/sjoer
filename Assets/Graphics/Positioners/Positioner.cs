using Assets.Graphics.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Graphics.Positioners
{
    class Positioner
    {
        public Shape Position(Shape obj)
        {
            return obj;
        }

        public void MoveShape(Vector3 p, Quaternion q)
        {
            //this.shape.transform.position = p;
            //this.shape.transform.rotation = q;
        }
    }
}
