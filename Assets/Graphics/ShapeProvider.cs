using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using UnityEngine;
using TMPro;
using System.IO;
using Assets.DataManagement;
using Assets.Resources;
using Assets.InfoItems;

namespace Assets.Graphics
{
    
    class ShapeProvider
    {
        public virtual void Get(InfoItem infoItem)
        {
            throw new NotImplementedException();
        }

        protected GameObject GetShape(string fname)
        {
            GameObject gameObject = GameObject.Instantiate(
                    AssetManager.Instance.objects[fname], 
                    Vector3.zero, 
                    Quaternion.identity
                );

            // 30/15 = 2, 150/15
            gameObject.transform.localScale = gameObject.transform.localScale * ((float)Config.Instance.conf.UISettings["HorizonPlaneRadius"] / 23);
            //HelperClasses.InfoAreaUtils.Instance.ScaleStick(gameObject, 2f);
            //HelperClasses.InfoAreaUtils.Instance.ScalePin(gameObject, 2f);


            return gameObject;
        }
    }

    class AISHorizonShapeProvider : ShapeProvider
    {
        public override void Get(InfoItem infoItem)
        {
            if (!infoItem.Shape) InjectNewShape(infoItem);

            if (infoItem.TargetHasChanged())
            {
                UpdateShape(infoItem);
            }
        }

        private void UpdateShape(InfoItem infoItem)
        {
            HelperClasses.InfoAreaUtils.Instance.ToggleAISPinOverflowVisible(infoItem.Shape, infoItem.IsTarget);

            if (infoItem.IsTarget)
            {
                if (!infoItem.IsExpanded) HelperClasses.InfoAreaUtils.Instance.ShowAISPinInfo(infoItem.Shape, 4);
                infoItem.IsExpanded = true;
            }
            else
            {
                if (infoItem.IsExpanded) HelperClasses.InfoAreaUtils.Instance.ShowAISPinInfo(infoItem.Shape, 0.25f);
                infoItem.IsExpanded = false;
            }
        }

        private void InjectNewShape(InfoItem infoItem)
        {
            string prefab = "AISPin";
            infoItem.Shape = GetShape(prefab);

            // A new shape always starts collapsed
            HelperClasses.InfoAreaUtils.Instance.ToggleAISPinOverflowVisible(infoItem.Shape, infoItem.IsTarget);
        }
    }

    class AISSkyShapeProvider : ShapeProvider
    {
        public override void Get(InfoItem infoItem)
        {
            if (!infoItem.Shape) infoItem.Shape = GetShape("AISPinTarget");
        }
    }
}
