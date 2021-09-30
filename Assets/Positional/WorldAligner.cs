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
