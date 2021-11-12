using Assets.DataManagement;
using Assets.HelperClasses;
using System;
using UnityEngine;
using Assets.Resources;
using Microsoft.MixedReality.Toolkit;

namespace Assets.Positional
{
    public class Player : MonoSingleton<Player>
    {
        public Camera mainCamera;

        // Difference between vessel bearing (true north) and Hololens bearing (unity coordinates)
        private Quaternion unityToTrueNorthRotation = Quaternion.identity;
        private AISDTO lastGPSUpdate;
        private DataRetriever gpsRetriever;
        Timer GPSTimer;

        private async void updateGPS()
        {
            lastGPSUpdate = (AISDTO) await gpsRetriever.fetch();
            Debug.Log("Heading: " + lastGPSUpdate.Heading);
        }

        // Start is called before the first frame update
        void Start()
        {
            gpsRetriever = new DataRetriever(DataConnections.BluetoothGPS, DataAdapters.GPSInfo, ParameterExtractors.None, this);
            GPSTimer = new Timer(1f, updateGPS);
            SetLightIntensity();
        }

        public Camera GetMainCamera()
        {
            EnsureMainCamera();
            return mainCamera;
        }

        // Black becomes transparent on HoloLens (and so do shadows)
        // Therefore, this code ensures that the scene is brightly lit
        private void SetLightIntensity()
        {
            Light playerLight = mainCamera.transform.GetChild(0).gameObject.GetComponent<Light>();
            playerLight.range = (float)Config.Instance.conf.UISettings["HorizonPlaneRadius"] + 1;
            playerLight.intensity = 30;
        }

        void EnsureMainCamera ()
        {
            if (!mainCamera)
            {
                mainCamera = (Camera)GameObject.Find("MixedRealityPlayspace").transform.GetChild(0).gameObject.GetComponent<Camera>();
                SetLightIntensity();
            }
        }

        // Update is called once per frame
        void Update()
        {
            EnsureMainCamera();

            //  Temporary not insanely swift GPS update hack
            if (!gpsRetriever.isConnected() || GPSTimer.hasFinished())
            {
                GPSTimer.restart();
            }
            GPSTimer.Update();

            // The regular code
            //if (dataRetriever.isConnected())
            //{
            //    updateGPS();
            //}

        }

        private void OnDestroy()
        {
            gpsRetriever.OnDestroy();
        }

        public Vector3 GetWorldTransform(double lat, double lon)
        {
            double x, y, z;
            if (lastGPSUpdate != null && lastGPSUpdate.Valid)
            {
                GPSUtils.Instance.GeodeticToEnu(
                    lat, lon, 0, 
                    lastGPSUpdate.Latitude, lastGPSUpdate.Longitude, 0, 
                    out x, out y, out z
                );
            }
            else
            {
                GPSUtils.Instance.GeodeticToEnu(
                    lat, lon, 0, 
                    Config.Instance.conf.NonVesselSettings["Latitude"], Config.Instance.conf.NonVesselSettings["Longitude"], 0, 
                    out x, out y, out z
                );
            }

            Vector3 newPos = unityToTrueNorthRotation * (new Vector3((float)x, (float)z, (float)y) - mainCamera.transform.position) + mainCamera.transform.position;

            return newPos;

            //return mainCamera.transform.RotateAround(mainCamera.transform.position, mainCamera.transform.position + new Vector3((float)x, (float)z, (float)y), unityToTrueNorthRotation.y);
        }

        public Vector2 GetCurrentLatLon()
        {
            return new Vector2((float)lastGPSUpdate.Latitude, (float)lastGPSUpdate.Longitude);
        }

        public Tuple<Vector2, Vector2> GetCurrentLatLonArea()
        {
            double lat, lon;

            // Use the harcoded values if the GPS reading is invalid
            if (lastGPSUpdate != null && lastGPSUpdate.Valid)
            {
                lat = lastGPSUpdate.Latitude;
                lon = lastGPSUpdate.Longitude;
            }
            else
            {
                Debug.Log("Invalid GPS, using hardcoded one");
                lat = Config.Instance.conf.NonVesselSettings["Latitude"];
                lon = Config.Instance.conf.NonVesselSettings["Longitude"];
            }

            return HelperClasses.GPSUtils.Instance.GetCurrentLatLonArea(
                    lat,
                    lon
                );
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
            Debug.Log("Unity to true north");
            float heading = lastGPSUpdate != null ? (float)lastGPSUpdate.Heading : 0;
            unityToTrueNorthRotation = Quaternion.Euler(0, mainCamera.transform.rotation.eulerAngles.y
                                    - heading, 0);
            Debug.Log(heading);
        }
    }

}
