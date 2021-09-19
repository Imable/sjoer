using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System;
using System.Reflection;

class Config : Assets.HelperClasses.Singleton<Config>
{
    public Conf conf;
    public Config()
    {
        
        string path = Path.Combine(Directory.GetCurrentDirectory(), "Assets\\Config\\conf.json");
        Debug.Log(path);
        conf = JsonUtility.FromJson<Conf>(File.ReadAllText(path));
    }
}

[Serializable]
class Conf
{
    public bool VesselMode;
    public Dictionary<string, int> VesselSettings;
    public Dictionary<string, int> NonVesselSettings;
    public Dictionary<string, string> DbCreds;
}
