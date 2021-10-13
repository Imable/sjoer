using Assets.DataManagement;
using System;
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

        private async void updateGPS()
        {
            lastGPSUpdate = (GPSInfoDTO) await dataRetriever.fetch();
        }

        // Start is called before the first frame update
        void Start()
        {
            dataRetriever = new DataRetriever(DataSources.GPSInfo);
        }

        // Update is called once per frame
        void Update()
        {
            if (dataRetriever.isConnected())
            {
                updateGPS();
            }


            //Debug.Log($"HoloForward: {mainCamera.transform.forward}");
            //Debug.Log($"HoloRotationAroundY: {mainCamera.transform.rotation.eulerAngles.y}");
            //Debug.Log($"UnityToTrueNorth: {unityToTrueNorthRotation}");
        }

        public Tuple<Vector3, Quaternion> GetWorldTransform(double lat, double lon)
        {
            double x, y, z;
            if (Config.Instance.conf.VesselMode)
            {
                HelperClasses.GPSUtils.Instance.GeodeticToEnu(
                    lat, lon, 0, lastGPSUpdate.Latitude, 
                    lastGPSUpdate.Longitude, -Config.Instance.conf.VesselSettings["BridgeHeight"], 
                    out x, out y, out z
                );
            }
            else
            {
                HelperClasses.GPSUtils.Instance.GeodeticToEnu(
                    lat, lon, 0, Config.Instance.conf.NonVesselSettings["Latitude"], 
                    Config.Instance.conf.NonVesselSettings["Longitude"], -Config.Instance.conf.NonVesselSettings["PlatformHeight"], 
                    out x, out y, out z
                );
            }

            return new Tuple<Vector3, Quaternion>(mainCamera.transform.position + new Vector3((float)x, (float)z, (float)y), Quaternion.Euler(unityToTrueNorthRotation));
        }

        public Vector2 GetCurrentLatLon()
        {
            return new Vector2((float)lastGPSUpdate.Latitude, (float)lastGPSUpdate.Longitude);
        }

        public Tuple<Vector2, Vector2> GetCurrentLatLonArea()
        {
            Tuple<Vector2, Vector2> response = new Tuple<Vector2, Vector2>(Vector2.zero, Vector2.zero);

            if (lastGPSUpdate != null)
            {
                response = HelperClasses.GPSUtils.Instance.GetCurrentLatLonArea(
                    lastGPSUpdate.Latitude, 
                    lastGPSUpdate.Longitude
                );
            }

            return response;
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
