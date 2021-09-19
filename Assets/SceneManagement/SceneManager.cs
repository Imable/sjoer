using UnityEngine;
using Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.SceneManagement
{
    class SceneManager : HelperClasses.Singleton<SceneManager>
    {
        private Scenes currentScene;
        private Scenes nextScene;
    }
}
