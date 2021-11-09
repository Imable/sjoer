using UnityEngine;
using Assets.InfoItems;
using Assets.Positional;
using Assets.Resources;
using Assets.DataManagement;

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

            this.infoItems = new InfoItem[] {
                new DelayedInfoItem(DataConnections.BarentswatchAIS, DataAdapters.BarentswatchAIS, ParameterExtractors.BarentswatchAIS, Graphics.GraphicTypes.Point3D, aligner, (float) Config.Instance.conf.DataSettings["UpdateInterval"])
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
