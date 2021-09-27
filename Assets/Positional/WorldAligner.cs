using Assets.DataManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Positional
{
    public class WorldAligner : MonoBehaviour
    {
        //public GameObject cloneThisObject;
        public Camera mainCamera;

        // Difference between vessel bearing (true north) and Hololens bearing (unity coordinates)
        private Vector3 unityToTrueNorthRotation = Vector3.zero;
        private GPSInfoDTO lastGPSUpdate;

        private DataRetriever dataRetriever;

        // Start is called before the first frame update
        void Start()
        {
            dataRetriever = new DataRetriever(DataSources.GPSInfo);
            lastGPSUpdate = (GPSInfoDTO)dataRetriever.fetch();
            unitytoTrueNorth();

            DataRetriever marineTrafficAIS = new DataRetriever(DataSources.MarineTrafficAIS);
            MarineTrafficAISDTO mtAIS = (MarineTrafficAISDTO)marineTrafficAIS.fetch();
            Debug.Log(mtAIS.Identifier);

            //double x, y, z;
            //if (Config.Instance.conf.VesselMode)
            //{
            //    HelperClasses.GPSUtils.Instance.GeodeticToEnu(mtAIS.Latitude, mtAIS.Longitude, -Config.Instance.conf.VesselSettings["BridgeHeight"], lastGPSUpdate.Latitude, lastGPSUpdate.Longitude, 0, out x, out y, out z);
            //    Debug.Log($"{x},{y},{z}");

            //    Instantiate(cloneThisObject, mainCamera.transform.position + new Vector3((float)x, (float)z, (float)y), Quaternion.Euler(unityToTrueNorthRotation));
            //} else
            //{
            //    HelperClasses.GPSUtils.Instance.GeodeticToEnu(60.402585, 5.323235, -Config.Instance.conf.NonVesselSettings["PlatformHeight"], Config.Instance.conf.NonVesselSettings["Latitude"], Config.Instance.conf.NonVesselSettings["Longitude"], 0, out x, out y, out z);
            //    Debug.Log($"{x},{y},{z}");

            //    Instantiate(cloneThisObject, mainCamera.transform.position + new Vector3((float)x, (float)z, (float)y), Quaternion.Euler(unityToTrueNorthRotation));


            //}

            //updateAlignVesselAndHoloLens();
        }

        // Update is called once per frame
        void Update()
        {
            lastGPSUpdate = (GPSInfoDTO)dataRetriever.fetch();


            //Debug.Log($"HoloForward: {mainCamera.transform.forward}");
            //Debug.Log($"HoloRotationAroundY: {mainCamera.transform.rotation.eulerAngles.y}");
            //Debug.Log($"UnityToTrueNorth: {unityToTrueNorthRotation}");
        }

        // The rotation that transforms the Unity north axis to True north
        // This should only be executed when Hololens and vessel are aligned
        // (and thus vessel compass information and Hololens direction match)
        public void calibrate()
        {
            this.unitytoTrueNorth();
        }
        private void unitytoTrueNorth()
        {
            unityToTrueNorthRotation = new Vector3(0, mainCamera.transform.rotation.eulerAngles.y
                                    - (float) lastGPSUpdate.TrueCourse, 0);
        }
    }

}
