using Assets.DataManagement;
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
    abstract class Positioner
    {
        protected Player aligner;
        protected Action<InfoItem> InnerPosition;

        public Positioner(Player aligner)
        {
            this.aligner = aligner;
        }

        public void SetAligner (Player aligner)
        {
            this.aligner = aligner;
        }

        public abstract void Position(InfoItem infoItem);

        protected void FaceUser(GameObject g)
        {
            g.transform.rotation = Quaternion.LookRotation(aligner.mainCamera.transform.position - g.transform.position);
        }

        protected Vector3 GetWorldTransform(AISDTO aisDTO)
        {
            return aligner.GetWorldTransform(aisDTO.Latitude, aisDTO.Longitude);
        }
    }

    class AISHorizonPositioner : Positioner
    {
        // VS was complaining, so I added this, but it shouldn't be necessary...
        public AISHorizonPositioner(Player aligner) : base(aligner)
        {
        }

        public override void Position(InfoItem infoItem)
        {
            Vector3 position = GetWorldTransform((AISDTO)infoItem.GetDTO);
            infoItem.Shape.transform.position = HelperClasses.InfoAreaUtils.Instance.UnityCoordsToHorizonPlane(position, aligner.mainCamera.transform.position);
            FaceUser(infoItem.Shape);
        }
    }

    class AISSkyPositioner : Positioner
    {
        public AISSkyPositioner(Player aligner) : base(aligner)
        {
        }

        public override void Position(InfoItem infoItem)
        {
            Vector3 position = GetWorldTransform((AISDTO)infoItem.GetDTO);
            infoItem.Shape.transform.position = HelperClasses.InfoAreaUtils.Instance.UnityCoordsToSkyArea(position, aligner.mainCamera.transform.position);
            FaceUser(infoItem.Shape);
        }
    }
}
