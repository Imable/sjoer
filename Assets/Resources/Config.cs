using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System;
using System.Reflection;
using Newtonsoft.Json;

namespace Assets.Resources
{
    class Config : Assets.HelperClasses.CSSingleton<Config>
    {
        public Conf conf;
        public BarentsConf barentswatch;
        public Config()
        {
            conf = JsonConvert.DeserializeObject<Conf>(AssetManager.Instance.config["generic"].text);
            barentswatch = JsonConvert.DeserializeObject<BarentsConf>(AssetManager.Instance.config["barentswatch"].text);
        }
    }

    [Serializable]
    public class Conf
    {
        public bool VesselMode;
        public Dictionary<string, double> DataSettings;
        // Double typed vessel settings
        public Dictionary<string, double> VesselSettingsD;
        // String typed vessel settings
        public Dictionary<string, string> VesselSettingsS;
        public Dictionary<string, double> NonVesselSettings;
        public Dictionary<string, string> DbCreds;
        public Dictionary<string, int> SceneSettings;
        public Dictionary<string, int> CalibrationSettings;
    }

    public class BarentsConf
    {
        public string token_url;
        public string ais_url;
        public string auth_format;
        public string client_id;
        public string client_secret;
    }
}
