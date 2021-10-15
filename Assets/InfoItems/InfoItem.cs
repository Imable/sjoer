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

        public InfoItem(DataSources dataSource, GraphicTypes graphicType, WorldAligner aligner)
        {
            this.dataRetriever = new DataRetriever(dataSource, aligner);
            this.graphic       = new Graphic(graphicType, aligner);
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
            graphic.display(dto);
        }
    }

     class DelayedInfoItem : InfoItem
    {
        float delay = 0;
        private DateTime lastDataUpdate = DateTime.Now;

        public DelayedInfoItem(DataSources dataSource, GraphicTypes graphicType, WorldAligner aligner, float delay) : base(dataSource, graphicType, aligner)
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
