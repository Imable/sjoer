using UnityEngine;
using Assets.InfoItems;
using System;
using Assets.Positional;

namespace Assets.SceneManagement
{
    public class DefaultScene : MonoBehaviour
    {
        public GameObject markerObject;
        public GameObject player;

        private InfoItem[] infoItems;
        private DateTime lastDataUpdate = DateTime.Now;
        private WorldAligner playerAligner;

        void Start()
        {
            playerAligner = player.GetComponent<WorldAligner>();
            this.infoItems = new InfoItem[] {
                new InfoItem(DataManagement.DataSources.AIS, markerObject, playerAligner)
            };

            foreach (InfoItem infoItem in infoItems)
            {
                infoItem.Start();
            }
        }

        void Update()
        {
            FetchData();
            Draw();
        }

        void FetchData()
        {
            // Only update data every `UpdateInterval` seconds
            DateTime now = DateTime.Now;
            if ((now - lastDataUpdate).TotalSeconds > Config.Instance.conf.DataSettings["UpdateInterval"])
            {
                lastDataUpdate = now;
                Tuple<Vector2, Vector2> latLonArea = playerAligner.GetCurrentLatLonArea();
                Debug.Log($"Lat min {latLonArea.Item1.x} Lon min {latLonArea.Item1.y}");
                Debug.Log($"Lat max {latLonArea.Item2.x} Lon max {latLonArea.Item2.y}");

                foreach (InfoItem infoItem in infoItems)
                {
                    if (infoItem.isConnected())
                    {
                        infoItem.UpdateData(new string[]
                        {
                            //latmin lonmin latmax lonmax
                            latLonArea.Item1.x.ToString(),
                            latLonArea.Item1.y.ToString(),
                            latLonArea.Item2.x.ToString(),
                            latLonArea.Item2.y.ToString()
                        });
                    }
                }
            }
        }

        void Draw()
        {

        }
    }
}
