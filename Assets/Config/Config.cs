using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System;
using System.Reflection;
using Newtonsoft.Json;

class Config : Assets.HelperClasses.Singleton<Config>
{
    public Conf conf;
    public Config()
    {
        
        string path = Path.Combine(Directory.GetCurrentDirectory(), "Assets\\Config\\conf.json");
        conf = JsonConvert.DeserializeObject<Conf>(File.ReadAllText(path));
        Debug.Log(conf);
    }
}

[Serializable]
class Conf
{
    public bool VesselMode;
    public Dictionary<string, double> VesselSettings;
    public Dictionary<string, double> NonVesselSettings;
    public Dictionary<string, string> DbCreds;
    public Dictionary<string, int> SceneSettings;
    public Dictionary<string, int> CalibrationSettings;
}
