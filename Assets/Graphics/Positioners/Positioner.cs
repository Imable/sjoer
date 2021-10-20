using Assets.DataManagement;
using Assets.Graphics.Shapes;
using Assets.Positional;
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
        protected WorldAligner aligner;

        public Positioner(WorldAligner aligner)
        {
            this.aligner = aligner;
        }

        public virtual Shape Position(Shape obj)
        {
            return obj;
        }

        protected virtual void MoveShape(DTO dto, GameObject gameObject)
        {
            //this.shape.transform.position = p;
            //this.shape.transform.rotation = q;
        }

        protected void FaceUser(GameObject g)
        {
            g.transform.rotation = Quaternion.LookRotation(g.transform.position - aligner.mainCamera.transform.position);
        }
    }

    class AISPositioner : Positioner
    {
        // VS was complaining, so I added this, but it shouldn't be necessary...
        public AISPositioner(WorldAligner aligner) : base(aligner)
        {
            this.aligner = aligner;
        }

        public override Shape Position(Shape s)
        {
            AISShape shape = (AISShape)s;
            foreach (Tuple<AISDTO, GameObject> aisObj in shape.objects.Values)
            {
                MoveShape(aisObj.Item1, aisObj.Item2);
                FaceUser(aisObj.Item2);
            }

            return shape;
        }

        protected override void MoveShape(DTO dto, GameObject gameObject)
        {
            AISDTO aisDTO = (AISDTO)dto;
            Tuple<Vector3, Quaternion> pos = aligner.GetWorldTransform(aisDTO.Latitude, aisDTO.Longitude);
            gameObject.transform.position = pos.Item1;
            gameObject.transform.rotation = pos.Item2;
        }
    }
}
