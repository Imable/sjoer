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

        public void SetAligner (Player aligner)
        {
            this.aligner = aligner;
        }

        public abstract void Position(InfoItem infoItem);

        protected Vector3 GetWorldTransform(AISDTO aisDTO)
        {
            return aligner.GetWorldTransform(aisDTO.Latitude, aisDTO.Longitude);
        }
    }

    class AISHorizonPositioner : Positioner
    {
        public override void Position(InfoItem infoItem)
        {
            Vector3 position = GetWorldTransform((AISDTO)infoItem.GetDTO);
            //infoItem.Shape.transform.position = HelperClasses.InfoAreaUtils.Instance.UnityCoordsToHorizonPlane(position, aligner.mainCamera.transform.position);
            infoItem.Shape.transform.position =
                HelperClasses.InfoAreaUtils.Instance
                .UnityCoordsToHorizonPlane(position, aligner.mainCamera.transform.position);
            infoItem.Shape.transform.rotation =
                HelperClasses.InfoAreaUtils.Instance
                .FaceUser(position, aligner.mainCamera.transform.position);
        }
    }

    class AISSkyPositioner : Positioner
    {
        public override void Position(InfoItem infoItem)
        {
            Vector3 position = GetWorldTransform((AISDTO)infoItem.GetDTO);
            infoItem.Shape.transform.position = 
                HelperClasses.InfoAreaUtils.Instance
                .UnityCoordsToSkyArea(position, aligner.mainCamera.transform.position);
            infoItem.Shape.transform.rotation =
                HelperClasses.InfoAreaUtils.Instance
                .FaceUser(position, aligner.mainCamera.transform.position);
        }
    }
}
