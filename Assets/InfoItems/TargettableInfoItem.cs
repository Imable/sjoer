using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using UnityEngine;

// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Assets.InfoItems
{
    public class TargettableInfoItem : BaseInputHandler, IMixedRealityInputHandler
    {
        [SerializeField]
        private MixedRealityInputAction selectAction = MixedRealityInputAction.None;

        private bool target = false;

        public bool IsTarget
        {
            get { return target; }
            set { target = value; }
        }

        public bool ChangedThisRender
        {
            set { target = value; }
        }

        protected override void RegisterHandlers()
        {
            CoreServices.InputSystem?.RegisterHandler<IMixedRealityInputHandler>(this);
        }

        protected override void UnregisterHandlers()
        {
            CoreServices.InputSystem?.UnregisterHandler<IMixedRealityInputHandler>(this);
        }

        public void OnInputUp(InputEventData eventData)
        {
        }

        public void OnInputDown(InputEventData eventData)
        {
            if (eventData.MixedRealityInputAction == selectAction)
            {
                Debug.Log("Select!");
                target = !target;
            }
        }
    }
}