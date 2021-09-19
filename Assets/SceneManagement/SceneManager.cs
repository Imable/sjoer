using UnityEngine;
using Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.SceneManagement
{
    public class SceneManager : HelperClasses.Singleton<SceneManager>
    {
        private Scenes currentScene;
        private Scenes nextScene;

        public void setNextScene(Scenes scene)
        {
            
        }

        // This is called from the SpeechInputHandler_Global when the command 'Calibrate North' is given 
        public void setNextSceneToCalibration()
        {
            this.setNextScene(Scenes.Calibration);
        }

        // TODO: Implement Scene transitions here
    }
}
