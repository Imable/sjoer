using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.DataManagement;
using Assets.Graphics;
using Assets.Positional;
using UnityEngine;

namespace Assets.InfoItems
{
    class InfoCategory
    {
        private Dictionary<string, InfoItem> infoItems = new Dictionary<string, InfoItem>();

        private DataRetriever dataRetriever;
        private DataType dataType;
        private DisplayArea displayArea;

        private DTO lastDTO;

        public InfoCategory(DataConnections connection, DataAdapters adapter, ParameterExtractors extractor, 
            Player aligner,
            DataType dataType, DisplayArea displayArea)
        {
            this.dataRetriever = new DataRetriever(connection, adapter, extractor, aligner);
            this.dataType = dataType;
            this.displayArea = displayArea;
        }

        public virtual void Update()
        {
            RetrieveInfoItems();
            Tick();
        }

        protected void Tick()
        {
            foreach (InfoItem infoItem in infoItems.Values)
            {
                infoItem.Update();
            }

            Debug.Log($"There are {infoItems.Count} InfoItems in the scene.");
        }

        public async void RetrieveInfoItems()
        {
            DTO dto = null;
            // If we can connect, get data
            if (dataRetriever.isConnected()) dto = await this.dataRetriever.fetch();
            // If we could connect and data is valid, store that data
            if (dto != null && dto.Valid) lastDTO = dto;
            // Process new data is that was available, otherwise process old data if that is available
            if (lastDTO != null) HandleNewInfoItems(dto);
        }

        private void HandleNewInfoItems(DTO dto)
        {
            //if (((AISDTOs)dto).vessels.Length > 0)
            //{
            //    Vector2 LatLon = Player.Instance.GetLatLon;
            //    ((AISDTOs)dto).vessels[0] = new AISDTO
            //    {
            //        Valid = true,
            //        SOG = 0,
            //        COG = 0,
            //        Draught = 0,
            //        Name = "NORTH",
            //        Key = "NORTH",
            //        Target = true,
            //        Latitude = LatLon.x + 1,
            //        Longitude = LatLon.y
            //    };
            //}

            foreach (InfoItem infoItem in AISInfoItem.Generate(dto, dataType, displayArea))
            {
                if (infoItems.ContainsKey(infoItem.Key))
                {
                    MergeInfoItems(infoItem);
                } else
                {
                    AddNewInfoItem(infoItem);
                }
            }
        }

        private void AddNewInfoItem(InfoItem infoItem)
        {
            infoItems[infoItem.Key] = infoItem;
        }

        private void MergeInfoItems(InfoItem infoItem)
        {
            // Inject the gameobject of the old infoItem into the new one
            infoItem.Merge(infoItems[infoItem.Key]);
            AddNewInfoItem(infoItem);
        }

        public void OnDestroy()
        {
            dataRetriever.OnDestroy();
        }
    }
}
