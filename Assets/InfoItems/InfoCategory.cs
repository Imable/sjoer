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
    abstract class InfoCategory
    {
        protected Dictionary<string, InfoItem> infoItems = new Dictionary<string, InfoItem>();

        protected DataType dataType;
        protected DisplayArea displayArea;

        protected DTO lastDTO;

        public InfoCategory(
            Player aligner,
            DataType dataType, DisplayArea displayArea)
        {
            this.dataType = dataType;
            this.displayArea = displayArea;
        }

        public Dictionary<string, InfoItem> Update()
        {
            RetrieveInfoItems();
            Tick();
            return infoItems;
        }

        protected void Tick()
        {
            foreach (InfoItem infoItem in infoItems.Values)
            {
                infoItem.Update();
            }

            Debug.Log($"There are {infoItems.Count} InfoItems in the scene.");
        }

        protected abstract void RetrieveInfoItems();

        protected void HandleNewInfoItems(DTO dto)
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

        public abstract void OnDestroy();
    }

    class ConnectedInfoCategory : InfoCategory
    {
        private DataRetriever dataRetriever;

        public ConnectedInfoCategory(
            Player aligner,
            DataType dataType, DisplayArea displayArea,
            DataConnections connection, DataAdapters adapter, ParameterExtractors extractor)
            : base(aligner, dataType, displayArea)
        {
            this.dataRetriever = new DataRetriever(connection, adapter, extractor, aligner);
        }

        protected override async void RetrieveInfoItems()
        {
            DTO dto = null;
            // If we can connect, get data
            if (dataRetriever.isConnected()) dto = await this.dataRetriever.fetch();

            // If we could connect and data is valid, store that data
            if (dto != null && dto.Valid) {
                lastDTO = dto;
                // Process new data is that was available, otherwise process old data if that is available
                HandleNewInfoItems(dto);
            }
        }

        public override void OnDestroy()
        {
            dataRetriever.OnDestroy();
        }
    }

    class InjectedInfoCategory : InfoCategory
    {
        Func<List<InfoItem>> InfoItemInjector;

        public InjectedInfoCategory(
            Player aligner,
            DataType dataType, DisplayArea displayArea,
            Func<List<InfoItem>> InfoItemInjector)
            : base(aligner, dataType, displayArea)
        {
            this.InfoItemInjector = InfoItemInjector;
        }

        protected override async void RetrieveInfoItems()
        {
            //DTO dto = null;
            //// If we can connect, get data
            //if (dataRetriever.isConnected()) dto = await this.dataRetriever.fetch();
            //// If we could connect and data is valid, store that data
            //if (dto != null && dto.Valid) lastDTO = dto;
            //// Process new data is that was available, otherwise process old data if that is available
            //if (lastDTO != null) HandleNewInfoItems(dto);
        }

        public override void OnDestroy()
        {
            //dataRetriever.OnDestroy();
        }
    }
}
