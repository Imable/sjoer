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

        public bool IsClosestVesselShowing = false;

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

            if (IsClosestVesselShowing)
            {
                List<InfoItem> allVessels = allInfoItems[infoCategories[0].Name];

                if (allVessels.Count > 0)
                {
                    int closestVesselID = 0;
                    double haversine;
                    double closestVesselDistance = 1000000;
                    double vesselDistance;
                    double Lat = 60.397908; double Lon = 5.317065;
                    //double Lat = Player.Instance.GetLatLon.x; double Lon = Player.Instance.GetLatLon.y;

                    for (int i = 0; i < allVessels.Count; i++)
                    {
                        AISDTO dto = (AISDTO)allVessels[i].GetDTO;
                        haversine = Math.Pow(Math.Sin((Lat - dto.Latitude) * Math.PI / 360), 2) +
                            Math.Cos(Lat * Math.PI / 180) * Math.Cos(dto.Latitude * Math.PI / 180) *
                            Math.Pow(Math.Sin((Lon - dto.Longitude) * Math.PI / 360), 2);
                        vesselDistance = 12742000 * Math.Atan2(Math.Sqrt(haversine), Math.Sqrt(1 - haversine));

                        Debug.Log($"Ship number {i}'s name and distance are {dto.Key} and {vesselDistance} meters");

                        if (vesselDistance < closestVesselDistance)
                        {
                            closestVesselDistance = vesselDistance;
                            closestVesselID = i;
                        }
                    }

                    ((AISDTO)allVessels[closestVesselID].GetDTO).Target = true;

                    Debug.Log($"The closest Vessel is {allVessels[closestVesselID].Key}. It is {closestVesselDistance} meters away " +
                        $"and has state \"{allVessels[closestVesselID].CurrentState}\"");
                }
                else
                {
                    Debug.Log("There is no vessel in sight");
                }
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
            IsClosestVesselShowing = !IsClosestVesselShowing;

            //List<InfoItem> allVessels = allInfoItems[infoCategories[0].Name];

            //if (allVessels.Count > 0)
            //{
            //    int closestVesselID = 0;
            //    double haversine;
            //    double closestVesselDistance = 1000000;
            //    double vesselDistance;
            //    double Lat = 60.397908; double Lon = 5.317065;
            //    //double Lat = Player.Instance.GetLatLon.x; double Lon = Player.Instance.GetLatLon.y;

            //    for (int i = 0; i < allVessels.Count; i++)
            //    {
            //        AISDTO dto = (AISDTO)allVessels[i].GetDTO;
            //        haversine = Math.Pow(Math.Sin((Lat - dto.Latitude) * Math.PI / 360), 2) +
            //            Math.Cos(Lat * Math.PI / 180) * Math.Cos(dto.Latitude * Math.PI / 180) * 
            //            Math.Pow(Math.Sin((Lon - dto.Longitude) * Math.PI / 360), 2);
            //        vesselDistance = 12742000 * Math.Atan2(Math.Sqrt(haversine), Math.Sqrt(1 - haversine));

            //        Debug.Log($"Ship number {i}'s name and distance are {dto.Key} and {vesselDistance} meters");

            //        if (vesselDistance < closestVesselDistance)
            //        {
            //            closestVesselDistance = vesselDistance;
            //            closestVesselID = i;
            //        }
            //    }

            //    ((AISDTO)allVessels[closestVesselID].GetDTO).Target = true;

            //    Debug.Log($"The closest Vessel is {allVessels[closestVesselID].Key}. It is {closestVesselDistance} meters away " +
            //        $"and has state \"{allVessels[closestVesselID].CurrentState}\"");
            //}
            //else
            //{
            //    Debug.Log("There is no vessel in sight");
            //}
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
