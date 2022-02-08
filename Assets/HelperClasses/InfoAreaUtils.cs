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
            Vector3 newPosition = player + dir * (float)Config.Instance.conf.UISettings["HorizonPlaneRadius"];
            return new Vector3(
                    newPosition.x,
                    (float)Config.Instance.conf.VesselSettingsD["BridgeHeight"] - (float)Config.Instance.conf.DataSettings["UIElementHeight"], // newPosition.y > align with the horizon. TODO: Get layer number
                    newPosition.z
                );
        }

        public Vector3 UnityCoordsToSkyArea(Vector3 obj, Vector3 player)
        {
            Vector3 tmp = UnityCoordsToHorizonPlane(obj, player);
            return new Vector3(
                    tmp.x,
                    tmp.y + (float)Config.Instance.conf.DataSettings["SkyAreaHeight"],
                    tmp.z
                );
        }

        public void ScaleStick(GameObject target, float scale)
        {
            BoxCollider boxCollider = target.GetComponent<BoxCollider>();
            GameObject stick = target.transform.Find($"StickAnchor").gameObject;
            GameObject pin = target.transform.Find($"StickAnchor/Stick/PinAnchor").gameObject;
            stick.transform.localScale = new Vector3(stick.transform.localScale.x, stick.transform.localScale.y * scale, stick.transform.localScale.y);
            pin.transform.localScale = new Vector3(pin.transform.localScale.x, pin.transform.localScale.y * (1/scale), pin.transform.localScale.z);

            boxCollider.size = new Vector3(boxCollider.size.x, boxCollider.size.y + (scale / boxCollider.size.y) * boxCollider.size.y, boxCollider.size.z);
            boxCollider.center = new Vector3(boxCollider.center.x, boxCollider.center.y + (scale / boxCollider.center.y) * boxCollider.center.y, boxCollider.center.z);
        }

        public void ScalePin(GameObject target, float scale)
        {
            BoxCollider boxCollider = target.GetComponent<BoxCollider>();
            GameObject pin = target.transform.Find($"StickAnchor/Stick/PinAnchor").gameObject;
            pin.transform.localScale = new Vector3(pin.transform.localScale.x * scale, pin.transform.localScale.y * scale, pin.transform.localScale.z);

            boxCollider.size = new Vector3(boxCollider.size.x, boxCollider.size.y + (scale / boxCollider.size.y) * boxCollider.size.y, boxCollider.size.z);
            boxCollider.center = new Vector3(boxCollider.center.x, boxCollider.center.y + (scale / boxCollider.center.y) * boxCollider.center.y, boxCollider.center.z);
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
