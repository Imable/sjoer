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

        // Difference between vessel bearing and Hololens bearing
        private float bearingDiff = 0;
        private GPSInfoDTO lastGPSUpdate;

        private DataRetriever dataRetriever;

        private TouchScreenKeyboard keyboard;

        // Start is called before the first frame update
        void Start()
        {
            dataRetriever = new DataRetriever(DataSources.GPSInfo);
            lastGPSUpdate = (GPSInfoDTO)dataRetriever.fetch();
            updateAlignVesselAndHoloLens();
            keyboard = TouchScreenKeyboard.Open("text to edit");    
        }

        // Update is called once per frame
        void Update()
        {
            //lastGPSUpdate = (GPSInfoDTO) dataRetriever.fetch();
            //Debug.Log(mainCamera.transform.position);
            
            if (Input.GetKey(KeyCode.S))
            {
                DataRetriever marineTrafficAIS = new DataRetriever(DataSources.MarineTrafficAIS);
                MarineTrafficAISDTO mtAIS = (MarineTrafficAISDTO) marineTrafficAIS.fetch();
                Debug.Log(mtAIS.Identifier);

                double x, y, z;
                HelperClasses.GPSUtils.Instance.GeodeticToEnu(mtAIS.Latitude, mtAIS.Longitude, 0, lastGPSUpdate.Latitude, lastGPSUpdate.Longitude, 0, out x, out y, out z);
                Debug.Log($"{x},{y},{z}");

                Instantiate(cloneThisObject, mainCamera.transform.position + new Vector3((float)x, (float)y, (float)z), holoLensRelativeToTrueNorth(mainCamera.transform.rotation));
                //Instantiate(cloneThisObject, mainCamera.transform.position + mainCamera.transform.forward * 2.0f, mainCamera.transform.rotation);
            }

            if (Input.GetKey(KeyCode.A))
            {
                updateAlignVesselAndHoloLens();
            }
        }

        // HoloLens should face in same direction as the vessel and then this function should be invoked
        // Now we have reference values of the direction the HoloLens is facing, compared to 'True North'
        void updateAlignVesselAndHoloLens()
        {
            this.bearingDiff = (float)lastGPSUpdate.TrueCourse - mainCamera.transform.rotation.eulerAngles.y;
        }

        Quaternion holoLensRelativeToTrueNorth(Quaternion holoLensRotation)
        {
            return holoLensRotation *= Quaternion.Euler(Vector3.up * bearingDiff);
        }
    }

}
