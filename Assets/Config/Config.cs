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
    public BarentsConf barentswatch;
    public Config()
    {
        conf         = JsonConvert.DeserializeObject<Conf>(File.ReadAllText(path("conf.json")));
        barentswatch = JsonConvert.DeserializeObject<BarentsConf>(File.ReadAllText(path("barentswatch_conf.json")));
    }

    private string path(string fname)
    {
        string path = Path.Combine(Directory.GetCurrentDirectory(), $"Assets\\Config\\{fname}");
        return path;
    }
}

[Serializable]
class Conf
{
    public bool VesselMode;
    public Dictionary<string, double> DataSettings;
    public Dictionary<string, double> VesselSettings;
    public Dictionary<string, double> NonVesselSettings;
    public Dictionary<string, string> DbCreds;
    public Dictionary<string, int> SceneSettings;
    public Dictionary<string, int> CalibrationSettings;
}

class BarentsConf
{
    public string token_url;
    public string ais_url;
    public string auth_format;
    public string client_id;
    public string client_secret;

}
