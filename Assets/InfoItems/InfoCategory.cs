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
            Tick();
        }

        protected void Tick()
        {
            RetrieveInfoItems();
            foreach (InfoItem infoItem in infoItems.Values)
            {
                infoItem.Update();
            }

            Debug.Log($"There are {infoItems.Count} InfoItems in the scene.");
        }

        public async void RetrieveInfoItems()
        {
            if (!dataRetriever.isConnected()) return;

            DTO dto = await this.dataRetriever.fetch();
            HandleNewInfoItems(dto);
        }

        private void HandleNewInfoItems(DTO dto)
        {
            if (((AISDTOs)dto).vessels.Length > 0)
            {
                Vector2 LatLon = Player.Instance.GetLatLon;
                ((AISDTOs)dto).vessels[0] = new AISDTO
                {
                    Valid = true,
                    SOG = 0,
                    COG = 0,
                    Draught = 0,
                    Name = "NORTH",
                    Key = "NORTH",
                    Target = true,
                    Latitude = LatLon.x + 1,
                    Longitude = LatLon.y
                };
            }

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

    class DelayedInfoCategory : InfoCategory
    {
        float delay = 0;
        private DateTime lastDataUpdate = DateTime.Now;

        public DelayedInfoCategory(DataConnections connection, DataAdapters adapter, ParameterExtractors extractor,
            Player aligner,
            DataType dataType, DisplayArea displayArea,
            float delay): base(connection, adapter, extractor, aligner, dataType, displayArea)
        {
            this.delay = delay;
        }

        public override void Update()
        {
            // Only update data every `UpdateInterval` seconds
            DateTime now = DateTime.Now;
            if ((now - lastDataUpdate).TotalSeconds > delay)
            {
                lastDataUpdate = now;
                Tick();
            }
        }
    }
}
