using UnityEngine;
using Assets.InfoItems;
using System;

namespace Assets.SceneManagement
{
    public class DefaultScene : MonoBehaviour
    {
        private InfoItem[] infoItems;
        private DateTime lastDataUpdate = DateTime.Now;

        void Start()
        {
            infoItems = new InfoItem[] {
                new InfoItem(DataManagement.DataSources.AIS)
            };

            foreach (InfoItem infoItem in infoItems)
            {
                infoItem.Start();
            }
        }

        void Update()
        {
            // Only update data every `UpdateInterval` seconds
            DateTime now = DateTime.Now;
            if ((now - lastDataUpdate).TotalSeconds > Config.Instance.conf.DataSettings["UpdateInterval"])
            {
                lastDataUpdate = now;
                foreach (InfoItem infoItem in infoItems)
                {
                    if (infoItem.isConnected())
                    {
                        infoItem.UpdateData(new string[]
                        {
                            "5.259668",
                            "5.333057",
                            "60.393674",
                            "60.404000"
                        });
                    }
                }
            }
        }
    }
}
