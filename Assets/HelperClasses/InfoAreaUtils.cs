﻿using System;
using UnityEngine;
using Assets.Resources;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using Assets.Positional;

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

        public Vector3 UnityCoordsToSkyArea(Vector3 obj, Player player)
        {
            Vector3 tmp = UnityCoordsToHorizonPlane(obj, player.mainCamera.transform.position);
            return new Vector3(
                    tmp.x,
                    tmp.y + (float)Config.Instance.conf.DataSettings["SkyAreaHeight"],
                    tmp.z
                );
        }

        public float Vector3ToCircleT(Vector3 pt, Vector3 center)
        {
            Vector3 up = center + Vector3.forward + new Vector3(0, pt.y, 0);
            float angle = Vector3.Angle(up, pt);
            return angle;
        }

        // t in radians
        public Vector3 CircleTToVector3(float t, float height, Vector3 center)
        {
            Vector3 up = center + Vector3.forward + new Vector3(0, height, 0);
            Vector3 ret = (Quaternion.AngleAxis(t, Vector3.up) * up).normalized * (float) Config.Instance.conf.UISettings["HorizonPlaneRadius"];

            return ret;
        }

        public static Vector2 rotate(Vector2 v, float delta)
        {
            return new Vector2(
                v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
                v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
            );
        }

        public Vector3 MoveAlongCircle(Vector3 pt, float step, Vector3 center)
        {
            Vector2 problem2D = new Vector2(pt.x, pt.z);
            problem2D = rotate(problem2D, step);

            return new Vector3(
                problem2D.x,
                pt.y,
                problem2D.y);


            float t = Vector3ToCircleT(pt, center);
            return CircleTToVector3(t + step, pt.y, center);
        }


        // Calculates the new position on the SkyArea/HoriznPlane after moving by xDist
        public Vector3 MoveAlongX(Vector3 obj, float dist, Vector3 player)
        {
            float angle = (float) (Math.PI - 2 * Math.Acos(
                (dist / 2) / Config.Instance.conf.UISettings["HorizonPlaneRadius"]));
            return Quaternion.Euler(0, RadiansToDegrees(angle), 0) * (obj - player);
        }

        float RadiansToDegrees(float radians)
        {
            return radians * (float) (180 / Math.PI);
        }

        float DegreesToRadians(float degrees)
        {
            return degrees * (float)(Math.PI / 180);
        }

        public Quaternion FaceUser(Vector3 position, Vector3 player)
        {
            return Quaternion.LookRotation(player - position);
        }

        public void ShowAISPinInfo(GameObject target, float numInfo)
        {
            BoxCollider boxCollider = target.GetComponent<BoxCollider>();
            GameObject pin = target.transform.Find($"StickAnchor/Stick/PinAnchor").gameObject;
            pin.transform.localScale = new Vector3(pin.transform.localScale.x, pin.transform.localScale.y * numInfo, pin.transform.localScale.z);
            GameObject icon = target.transform.Find($"StickAnchor/Stick/PinAnchor/AISPinTarget/ShipIconAnchor").gameObject;
            icon.transform.localScale = new Vector3(icon.transform.localScale.x, icon.transform.localScale.y, icon.transform.localScale.z / numInfo);

            boxCollider.size = new Vector3(boxCollider.size.x, boxCollider.size.y + (numInfo - 1) * boxCollider.size.y, boxCollider.size.z);
            boxCollider.center = new Vector3(boxCollider.center.x, boxCollider.size.y / 2, boxCollider.center.z);
        }

        public void ToggleAISPinOverflowVisible(GameObject g, bool shouldBecomeVisible)
        {
            List<string> labels = new List<string>
            {
                "1Label", "1Value",
                "2Label", "2Value",
                "TargetNum"
            };
            foreach (string label in labels)
            {
                GetAISPinComponent(g, label).enabled = shouldBecomeVisible;
            }

            // Lastly enable/disable the target image
            g.transform.Find($"StickAnchor/Stick/PinAnchor/AISPinTarget/ShipIconAnchor/Canvas/Target").gameObject.GetComponent<Image>().enabled = shouldBecomeVisible;
        }

        private TextMeshProUGUI GetAISPinComponent(GameObject g, string fname)
        {
            GameObject obj = g.transform.Find($"StickAnchor/Stick/PinAnchor/AISPinTarget/ShipIconAnchor/Canvas/{fname}").gameObject;
            return obj.GetComponent<TextMeshProUGUI>(); ;
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
