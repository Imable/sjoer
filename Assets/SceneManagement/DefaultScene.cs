using UnityEngine;
using Assets.InfoItems;
using System;
using Assets.Positional;

namespace Assets.SceneManagement
{
    public class DefaultScene : MonoBehaviour
    {
        [SerializeField]
        public GameObject player;

        private InfoItem[] infoItems;

        void Start()
        {
            WorldAligner aligner = player.GetComponent<WorldAligner>();

            //DelayedInfoItem: Config.Instance.conf.DataSettings["UpdateInterval"]
            this.infoItems = new InfoItem[] {
                new DelayedInfoItem(DataManagement.DataSources.AIS, Graphics.GraphicTypes.Point3D, aligner, (float) Config.Instance.conf.DataSettings["UpdateInterval"])
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
                infoItem.Update();
            }
        }
    }
}
