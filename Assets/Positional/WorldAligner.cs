using Assets.DataManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Positional
{
    public class WorldAligner : MonoBehaviour
    {
        public GameObject cloneThisObject;
        public Camera mainCamera;

        // Difference between vessel bearing (true north) and Hololens bearing (unity coordinates)
        private Vector3 unityToTrueNorthRotation = Vector3.zero;
        private GPSInfoDTO lastGPSUpdate;

        private DataRetriever dataRetriever;

        private TouchScreenKeyboard keyboard;

        // Start is called before the first frame update
        void Start()
        {
            dataRetriever = new DataRetriever(DataSources.GPSInfo);
            lastGPSUpdate = (GPSInfoDTO)dataRetriever.fetch();
            unityToTrueNorthRotation = unitytoTrueNorth();

            //updateAlignVesselAndHoloLens();
        }

        // Update is called once per frame
        void Update()
        {
            lastGPSUpdate = (GPSInfoDTO)dataRetriever.fetch();


            Debug.Log($"HoloForward: {mainCamera.transform.forward}");
            Debug.Log($"HoloRotationAroundY: {mainCamera.transform.rotation.eulerAngles.y}");
            Debug.Log($"UnityToTrueNorth: {unityToTrueNorthRotation}");



            if (Input.GetKey(KeyCode.O))
            {
                DataRetriever marineTrafficAIS = new DataRetriever(DataSources.MarineTrafficAIS);
                MarineTrafficAISDTO mtAIS = (MarineTrafficAISDTO)marineTrafficAIS.fetch();
                Debug.Log(mtAIS.Identifier);

                double x, y, z;
                HelperClasses.GPSUtils.Instance.GeodeticToEnu(mtAIS.Latitude, mtAIS.Longitude, 0, lastGPSUpdate.Latitude, lastGPSUpdate.Longitude, 0, out x, out y, out z);
                Debug.Log($"{x},{y},{z}");

                Instantiate(cloneThisObject, mainCamera.transform.position + new Vector3((float)x, (float)z, (float)y), Quaternion.Euler(unityToTrueNorthRotation));
            }
                //    //Instantiate(cloneThisObject, mainCamera.transform.position + mainCamera.transform.forward * 2.0f, mainCamera.transform.rotation);
                //}

                //if (Input.GetKey(KeyCode.A))
                //{
                //    updateAlignVesselAndHoloLens();
                //}
        }


        // The rotation that transforms the Unity north axis to True north
        // This should only be executed when Hololens and vessel are aligned
        // (and thus vessel compass information and Hololens direction match)
        Vector3 unitytoTrueNorth()
        {
            return new Vector3(0, mainCamera.transform.rotation.eulerAngles.y
                                    - (float) lastGPSUpdate.TrueCourse, 0);
        }

        // HoloLens should face in same direction as the vessel and then this function should be invoked
        // Now we have reference values of the direction the HoloLens is facing, compared to 'True North'
        //void updateAlignVesselAndHoloLens()
        //{

        //    this.bearingDiff = new Vector3(0, (float)lastGPSUpdate.TrueCourse - mainCamera.transform.rotation.eulerAngles.y, 0);
        //}

        //Quaternion holoLensRelativeToTrueNorth(Quaternion holoLensRotation)
        //{
        //    return holoLensRotation *= Quaternion.Euler(Vector3.up * bearingDiff);
        //}
    }

}
