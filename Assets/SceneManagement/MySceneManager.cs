using UnityEngine;
using Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Assets.SceneManagement
{
    public class MySceneManager : HelperClasses.Singleton<MySceneManager>
    {
        // TODO: Write loop here, that loops when not calibrating.
        private bool looping = true;

        public void setNewScene(Scenes scene)
        {
            Debug.Log($"Setting next scene to {nameof(scene)}");
            SceneManager.LoadScene((int) scene);
        }

        // This is called from the SpeechInputHandler_Global when the command 'Calibrate North' is given 
        public void setNextSceneToCalibration()
        {
            this.setNewScene(Scenes.Calibration);
        }

        // TODO: Implement Scene transitions here
    }
}
