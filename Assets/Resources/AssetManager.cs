using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Assets.HelperClasses;

namespace Assets.Resources
{
    public  class AssetManager : MonoSingleton<AssetManager>
    {
        public StringTextDictionary config;
        public StringGameObjectDictionary objects;
    }
}
