using UnityEngine;
using Assets.InfoItems;

namespace Assets.SceneManagement
{
    public class DefaultScene : MonoBehaviour
    {
        private InfoItem[] infoItems;

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
            foreach (InfoItem infoItem in infoItems)
            {
                if (infoItem.isConnected())
                {
                    infoItem.Update(new string[]
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
