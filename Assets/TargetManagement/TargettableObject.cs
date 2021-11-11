using Microsoft.MixedReality.Toolkit.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.TargetManagement
{
    class TargettableObject : MonoBehaviour, IMixedRealityPointerHandler
    {
        private bool target = false;

        public void OnPointerClicked(MixedRealityPointerEventData eventData)
        {
            target = !target;
        }

        public void OnPointerDown(MixedRealityPointerEventData eventData)
        {
        }

        public void OnPointerDragged(MixedRealityPointerEventData eventData)
        {
        }

        public void OnPointerUp(MixedRealityPointerEventData eventData)
        {
        }
    }
}
