using System;
using UnityEngine;
using Assets.Resources;

namespace Assets.HelperClasses
{
    public class InfoAreaUtils : CSSingleton<InfoAreaUtils>
    {
        public Vector3 UnityCoordsToHorizonPlane(Vector3 obj, Vector3 player)
        {
            Vector3 dir = (obj - player).normalized;
            Vector3 newPosition = dir * (float)Config.Instance.conf.UISettings["HorizonPlaneRadius"];
            return new Vector3(
                    newPosition.x,
                    0, // newPosition.y > align with the horizon. TODO: Get layer number
                    newPosition.z
                );
        }

        public void ScaleStick(GameObject target, float scale)
        {
            GameObject stick = target.transform.Find($"StickAnchor").gameObject;
            GameObject pin = target.transform.Find($"StickAnchor/Stick/PinAnchor").gameObject;
            stick.transform.localScale = new Vector3(stick.transform.localScale.x, stick.transform.localScale.y * scale, stick.transform.localScale.y);
            pin.transform.localScale = new Vector3(pin.transform.localScale.x, pin.transform.localScale.y * (1/scale), pin.transform.localScale.z);
        }

        public void ScalePin(GameObject target, float scale)
        {
            GameObject pin = target.transform.Find($"StickAnchor/Stick/PinAnchor").gameObject;
            pin.transform.localScale = new Vector3(pin.transform.localScale.x * scale, pin.transform.localScale.y * scale, pin.transform.localScale.z);
        }

        public void PinToLayerOne(GameObject target)
        {
            HelperClasses.InfoAreaUtils.Instance.ScaleStick(target, 1f);
            //HelperClasses.InfoAreaUtils.Instance.ScalePin(gameObject, 2f);
        }

        public void PinToLayerTwo(GameObject target)
        {
            HelperClasses.InfoAreaUtils.Instance.ScaleStick(target, 2f);

        }

    }
}
