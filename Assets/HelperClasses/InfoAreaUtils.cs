using System;
using UnityEngine;
using Assets.Resources;

namespace Assets.HelperClasses
{
    public class InfoAreaUtils : CSSingleton<GPSUtils>
    {
        public void UnityCoordsToHorizonPlane(Vector3 unityCoords)
        {
            // Get Horizon plane distance in config
            // get vector from object to player
            // place object at distance of horizon plane in direction of the calculated vector
        }
    }
}
