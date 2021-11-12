using Microsoft.MixedReality.Toolkit.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.InfoItems
{
    class TargettableInfoItem : MonoBehaviour, IMixedRealityPointerHandler
    {
        private bool target = false;

        public bool IsTarget
        {
            get { return target; }
            set { target = value; }
        }

        public void OnPointerClicked(MixedRealityPointerEventData eventData)
        {
            Debug.Log("Clicked on!");
            target = !target;
        }

        public void OnPointerDown(MixedRealityPointerEventData eventData)
        {
            Debug.Log("Pointed down!");
        }

        public void OnPointerDragged(MixedRealityPointerEventData eventData)
        {
        }

        public void OnPointerUp(MixedRealityPointerEventData eventData)
        {
        }
    }
}
