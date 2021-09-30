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
            foreach (AISDTO aisDTO in dto.vessels)
            {
                Debug.Log($"Lat: {aisDTO.Latitude}  Lon: {aisDTO.Longitude}");
            }
        }
    }
}
