using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HelperClasses
{
    class Timer : MonoBehaviour
    {
        private float delay;
        private float time;
        private float totalTime;
        private bool done = false;


        public Timer(float time, Action onComplete)
        {
            this.time = time;
            this.totalTime = time;
        }

        private void Start()
        {

        }

        private void Update()
        {
            if (!done)
            {
                if (time > 0)
                {
                    time -= Time.deltaTime;

                    if (time < 0) time = 0;
                }

                if (time == 0)
                {
                    done = true;
                }
            }

        }

        public void restart()
        {
            done = false;
            time = totalTime;
        }

    }
}
