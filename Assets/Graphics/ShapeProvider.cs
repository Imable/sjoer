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
            else if (infoItem.TargetHasChanged()) TransferTarget(infoItem, InjectNewShape);
        }

        private void TransferTarget(InfoItem infoItem, Action<InfoItem> Inject)
        {
            bool target = infoItem.GetTargetHandler().IsTarget;
            UnityEngine.Object.Destroy(infoItem.Shape);
            Inject(infoItem);
            infoItem.GetTargetHandler(true).IsTarget = target;
        }

        private void InjectNewShape(InfoItem infoItem)
        {
            string prefab = "AISPin";
            infoItem.Shape = GetShape(prefab);

            HelperClasses.InfoAreaUtils.Instance.ToggleAISPinOverflowVisible(infoItem.Shape, infoItem.IsTarget);

            if (infoItem.IsTarget)
            {
                HelperClasses.InfoAreaUtils.Instance.ShowAISPinInfo(infoItem.Shape, 4);
            }
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
