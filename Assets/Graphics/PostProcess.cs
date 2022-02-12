using Assets.InfoItems;
using Assets.Positional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Graphics
{
    class Overlapper
    {
        public Overlapper(InfoItem infoItem, float offsetVector)
        {
            this.Item = infoItem;
            this.OffsetVector = offsetVector;
        }

        public InfoItem Item { get; set; }
        // -1 for left, 1 for right
        public float OffsetVector { get; set; }


    }

    class PostProcessor
    {
        protected List<InfoItem> infoItems;
        protected Player aligner;

        public List<InfoItem> PostProcess(List<InfoItem> infoItems)
        {
            this.infoItems = infoItems;
            Process();
            return infoItems;
        }

        public void SetAligner(Player aligner)
        {
            this.aligner = aligner;
        }

        protected virtual void Process() { }
    }

    class AISSkyPostProcessor : PostProcessor
    {
        float rectWidth = 2f;
        protected override void Process() 
        {
            Debug.Log("AISSKY Post proces");
            foreach (InfoItem infoItem in infoItems)
            {
                List<Overlapper> overlappers = GetOverlappingInfoItems(infoItem);
                foreach(Overlapper o in overlappers)
                {
                    o.Item.Shape.transform.position =
                        HelperClasses.InfoAreaUtils.Instance
                        .MoveAlongX(
                            o.Item.Shape.transform.position,
                            o.OffsetVector,
                            aligner.mainCamera.transform.position);
                    o.Item.Shape.transform.rotation =
                        HelperClasses.InfoAreaUtils.Instance
                        .FaceUser(
                            o.Item.Shape.transform.position,
                            aligner.mainCamera.transform.position);
                }
            }

        }

        private List<Overlapper> GetOverlappingInfoItems(InfoItem i)
        {
            List<Overlapper> colliders = new List<Overlapper>();
            foreach (InfoItem o in infoItems)
            {
                // Skip yourself
                if (o == i) continue;

                if (Overlaps(i, o)) colliders.Add(
                    new Overlapper(
                        o,
                        CalculateOffsetVector(i, o)
                    ));
            }
            return colliders;
        }

        // How far does Y need to move to not be in X
        private float CalculateOffsetVector(InfoItem x, InfoItem y)
        {
            float dist = (y.Shape.transform.position - x.Shape.transform.position).magnitude;
            return dist < 0 ? -rectWidth - dist : rectWidth - dist;
        }

        // Is X overlapped by Y?
        private bool Overlaps(InfoItem x, InfoItem y)
        {
            float minX, maxX;
            minX = x.Shape.transform.position.x - rectWidth;
            maxX = x.Shape.transform.position.x + rectWidth;

            float yX = y.Shape.transform.position.x;

            return yX > minX && yX < maxX;

        }
    }

    class AISHorizonPostProcessor : PostProcessor
    {
        protected override void Process()
        {

        }
    }
}
