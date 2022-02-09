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

        public abstract void OnDestroy();

        protected bool IsInInfoItems(InfoItem i)
        {
            return infoItems.ContainsKey(i.Key);
        }
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
                if (IsInInfoItems(infoItem))
                {
                    MergeInfoItems(infoItem);
                }
                else
                {
                    AddNewInfoItem(infoItem);
                }
                if (infoItem.IsTarget) Debug.Log($"TARGET {infoItem.Key}");

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
    }

    class InjectedInfoCategory : InfoCategory
    {
        Func<Dictionary<string, InfoItem>> InfoItemInjector;

        public InjectedInfoCategory(
            Player aligner,
            DataType dataType, DisplayArea displayArea,
            Func<Dictionary<string, InfoItem>> InfoItemInjector)
            : base(aligner, dataType, displayArea)
        {
            this.InfoItemInjector = InfoItemInjector;
        }

        protected override void RetrieveInfoItems()
        {
            Dictionary<string, InfoItem> newInfoItems = this.InfoItemInjector();

            foreach(KeyValuePair<string, InfoItem> entry in newInfoItems)
            {
                HandleNewInfoItem(entry.Value);
            }


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

        private void HandleNewInfoItem(InfoItem infoItem)
        {
            if (infoItem.IsTarget)
            {
                if (IsInInfoItems(infoItem))
                {
                    // Update infoitem
                    infoItems[infoItem.Key].InjectNewDTO(infoItem.GetDTO);
                } else
                {
                    InfoItem newInfoItem = new AISInfoItem(
                        infoItem.GetDTO, 
                        dataType, 
                        displayArea);
                    newInfoItem.Update();

                    newInfoItem.LinkTargetHandler(infoItem);
                    newInfoItem.IsTarget = infoItem.IsTarget;
                    infoItems[infoItem.Key] = newInfoItem;
                }
            } else
            {
                if (IsInInfoItems(infoItem))
                {
                    infoItems[infoItem.Key].DestroyMesh();
                    infoItems.Remove(infoItem.Key);
                }
            }
        }
    }
}
