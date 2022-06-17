using UnityEngine;
using Assets.InfoItems;
using Assets.Positional;
using Assets.Resources;
using Assets.DataManagement;
using Assets.Graphics;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

namespace Assets.SceneManagement
{
    public class DefaultScene : MonoBehaviour
    {
        [SerializeField]
        public GameObject player;

        Player aligner;

        private InfoCategory[] infoCategories;
        private Dictionary<string, List<InfoItem>> allInfoItems;

        private DateTime lastUpdate;

        void Start()
        {
            lastUpdate = DateTime.Now;
            Player aligner = player.GetComponent<Player>();
            GraphicFactory.Instance.aligner ??= aligner;

            infoCategories = new InfoCategory[2]
            {
                new ConnectedInfoCategory(
                    "AISHorizon",
                    aligner, 
                    DataType.AIS, DisplayArea.HorizonPlane,
                    DataConnections.BarentswatchAIS, DataAdapters.BarentswatchAIS, ParameterExtractors.BarentswatchAIS),
                new InjectedInfoCategory(
                    "AISSky",
                    aligner,
                    DataType.AIS, DisplayArea.SkyArea,
                    () => allInfoItems["AISHorizon"])
            };

            this.InitAllInfoItems();
        }

        private void InitAllInfoItems()
        {
            this.allInfoItems = new Dictionary<string, List<InfoItem>>();

            foreach (InfoCategory infoCategory in this.infoCategories)
            {
                this.allInfoItems[infoCategory.Name] = new List<InfoItem>();
            }
        }

        void Update()
        {
            DateTime now = DateTime.Now;
            if ((now - lastUpdate).TotalSeconds > Config.Instance.conf.DataSettings["UpdateInterval"])
            {
                lastUpdate = now;
                UpdateInfoCategoriesInOrder();
            }
        }

        void UpdateInfoCategoriesInOrder()
        {
            foreach (InfoCategory infoCategory in infoCategories)
            {
                allInfoItems[infoCategory.Name] = infoCategory.Update();
                GraphicFactory.Instance
                    .GetPostProcessor(infoCategory.DataType, infoCategory.DisplayArea)
                    .PostProcess(allInfoItems[infoCategory.Name]);
            }
        }
        
        public void ShowClosestVessel()
        {
            List<InfoItem> allVessels = new List<InfoItem>();
            
            foreach (InfoCategory infoCategory in infoCategories)
            {
                allVessels.AddRange(allInfoItems[infoCategory.Name]);
            }

            if (allVessels.Count > 0)
            {
                int closestVesselID = 0;
                double haversine;
                double closestVesselDistance = 1000000;
                double vesselDistance;
                //double Lat = 60.403029; double Lon = 5.32279;
                double Lat = Player.Instance.GetLatLon.x; double Lon = Player.Instance.GetLatLon.y;

                for (int i = 0; i < allVessels.Count; i++)
                {
                    AISDTO dto = (AISDTO)allVessels[i].GetDTO;
                    haversine = Math.Pow(Math.Sin((Lat - dto.Latitude) * Math.PI / 360), 2) +
                        Math.Cos(Lat * Math.PI / 180) * Math.Cos(dto.Latitude * Math.PI / 180) * 
                        Math.Pow(Math.Sin((Lon - dto.Longitude) * Math.PI / 360), 2);
                    vesselDistance = 12742000 * Math.Atan2(Math.Sqrt(haversine), Math.Sqrt(1 - haversine));

                    Debug.Log($"Ship number {i}'s name and distance are {dto.Key} and {vesselDistance}");

                    if (vesselDistance < closestVesselDistance)
                    {
                        closestVesselDistance = vesselDistance;
                        closestVesselID = i;
                    }
                }



                Debug.Log($"The closest Vessel is {allVessels[closestVesselID].Key} {closestVesselDistance} meters away " +
                    $"with ");
            }
            else
            {
                Debug.Log("There is no vessel in sight");
            }
        }

        void OnApplicationQuit()
        {
            OnDestroy();
        }

        void OnDisable()
        {
            OnDestroy();
        }

        void OnEnable()
        {
            SceneManager.activeSceneChanged += OnSceneLoaded;
        }

        void OnSceneLoaded(Scene o, Scene i)
        {
            OnDestroy();
        }

        void OnDestroy()
        {
            if (infoCategories != null)
            {
                foreach (InfoCategory i in infoCategories)
                {
                    i.OnDestroy();
                }

            }
        }
    }
}
