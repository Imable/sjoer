using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.DataManagement;
using UnityEngine;

namespace Assets.InfoItems
{
    class InfoItem
    {
        DataRetriever dataRetriever;
        AISDTOs latestVessels;
        
        public InfoItem(DataSources dataSource)
        {
            dataRetriever = new DataRetriever(dataSource);
        }

        public bool isConnected()
        {
            return dataRetriever.isConnected();
        }

        public void Start()
        {
        }

        public async void UpdateData(params string[] param)
        {
            AISDTOs dto = (AISDTOs) await dataRetriever.fetch(param);
            latestVessels = dto;

            // v Debug purposes v
            foreach (AISDTO aisDTO in dto.vessels)
            {
                Debug.Log($"Lat: {aisDTO.Latitude}  Lon: {aisDTO.Longitude}");
            }
        }

        public void Draw()
        {
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
        }
    }
}
