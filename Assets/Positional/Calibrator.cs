using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Positional
{
    public class Calibrator : MonoBehaviour
    {
        public GameObject player;

        private Camera mainCamera;
        private HelperClasses.Timer steadyTimer;
        private Quaternion rot = Quaternion.identity;
        private Vector3 pos = Vector3.zero;

        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("Calibrating. Hold head steady for 3 seconds.");

            mainCamera = player.GetComponent<WorldAligner>().mainCamera;
            steadyTimer = new HelperClasses.Timer(
                (float)Config.Instance.conf.CalibrationSettings["SteadyTime"],
                calibrate
                );
        }

        // Update is called once per frame
        void Update()
        {
            if (isShakyRot(mainCamera.transform.rotation)
                || isShakyPos(mainCamera.transform.position))
            {
                steadyTimer.restart();
            }
        }

        private bool isShakyRot(Quaternion incoming)
        {
            Quaternion delta = incoming * Quaternion.Inverse(rot);
            return delta.eulerAngles.magnitude < Config.Instance.conf.CalibrationSettings["RotationThreshold"];
        }

        private bool isShakyPos(Vector3 incoming)
        {
            Vector3 delta = incoming - pos;
            return delta.magnitude < Config.Instance.conf.CalibrationSettings["PositionThreshold"];
        }

        private void calibrate()
        {
            player.GetComponent<WorldAligner>().unitytoTrueNorth();
        }
    }
}
