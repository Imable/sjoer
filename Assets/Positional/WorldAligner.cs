using Assets.DataManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Positional
{
    public class WorldAligner : MonoBehaviour
    {
        public Camera mainCamera;
        private DataRetriever dataRetriever;       

        // Start is called before the first frame update
        void Start()
        {
            dataRetriever = new DataRetriever(DataSources.GPSInfo);
        }

        // Update is called once per frame
        void Update()
        {
            GPSInfoDTO dto = (GPSInfoDTO) dataRetriever.fetch();
            Debug.Log(dto.Latitude);
            Debug.Log(mainCamera.transform.position);
        }
    }

}
