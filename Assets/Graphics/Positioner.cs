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

        protected abstract Action<InfoItem> GetInnerPositioner(DisplayArea displayArea);

        public void Position(InfoItem infoItem, DisplayArea displayArea)
        {
            this.GetInnerPositioner(displayArea)(infoItem);
        }

        protected void FaceUser(GameObject g)
        {
            g.transform.rotation = Quaternion.LookRotation(aligner.mainCamera.transform.position - g.transform.position);
        }
    }

    class AISPositioner : Positioner
    {
        // VS was complaining, so I added this, but it shouldn't be necessary...
        public AISPositioner(Player aligner) : base(aligner)
        {

        }

        protected override Action<InfoItem> GetInnerPositioner(DisplayArea displayArea)
        {
            Action<InfoItem> positionFunc = HorizonPlane;
            switch (displayArea)
            {
                case DisplayArea.HorizonPlane:
                    positionFunc = HorizonPlane;
                    break;
                case DisplayArea.SkyArea:
                    positionFunc = SkyArea;
                    break;
                case DisplayArea.HUD:
                    break;
                default:
                    break;
            }

            return positionFunc;
        }

        protected void HorizonPlane(InfoItem infoItem)
        {
            Vector3 position = GetWorldTransform((AISDTO)infoItem.GetDTO);
            infoItem.Shape.transform.position = HelperClasses.InfoAreaUtils.Instance.UnityCoordsToHorizonPlane(position, aligner.mainCamera.transform.position);
            FaceUser(infoItem.Shape);
        }

        protected void SkyArea(InfoItem infoItem)
        {
            Vector3 position = GetWorldTransform((AISDTO)infoItem.GetDTO);
            infoItem.Shape.transform.position = HelperClasses.InfoAreaUtils.Instance.UnityCoordsToSkyArea(position, aligner.mainCamera.transform.position);
            FaceUser(infoItem.Shape);
        }

        private Vector3 GetWorldTransform(AISDTO aisDTO)
        {
            return aligner.GetWorldTransform(aisDTO.Latitude, aisDTO.Longitude);
        }
    }
}
