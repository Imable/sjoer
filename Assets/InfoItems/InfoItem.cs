using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.DataManagement;
using UnityEngine;
using Unity;
using Assets.Positional;
using Assets.Graphics;

namespace Assets.InfoItems
{
    class InfoItem : MonoBehaviour
    {
        DataRetriever dataRetriever;
        Graphic graphic;

        public InfoItem(DataConnections connection, DataAdapters adapter, ParameterExtractors extractor, GraphicTypes graphicType, DisplayArea displayArea, Player aligner)
        {
            this.dataRetriever = new DataRetriever(connection, adapter, extractor, aligner);
            this.graphic       = new Graphic(graphicType, displayArea, aligner);
        }

        public bool isConnected()
        {
            return dataRetriever.isConnected();
        }

        public void Start()
        {
            
        }

        public virtual void Update()
        {
            Tick();
        }


        protected async void Tick()
        {
            // Stop execution if not connected yet
            if (!dataRetriever.isConnected()) return;

            DTO dto = await UpdateData();
            Draw(dto);
        }

        protected Task<DTO> UpdateData()
        {
            return dataRetriever.fetch();
        }

        protected void Draw(DTO dto)
        {
            graphic.Display(dto);
        }
    }

     class DelayedInfoItem : InfoItem
    {
        float delay = 0;
        private DateTime lastDataUpdate = DateTime.Now;

        public DelayedInfoItem(DataConnections connection, DataAdapters adapter, ParameterExtractors extractor, GraphicTypes graphicType, DisplayArea displayArea, Player aligner, float delay) : base(connection, adapter, extractor, graphicType, displayArea, aligner)
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
