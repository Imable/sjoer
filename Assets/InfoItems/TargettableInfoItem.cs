using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using UnityEngine;

// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Assets.InfoItems
{
    public class Targettable : BaseInputHandler, IMixedRealityInputHandler
    {
        [SerializeField]
        protected MixedRealityInputAction selectAction = MixedRealityInputAction.None;

        protected override void RegisterHandlers()
        {
            CoreServices.InputSystem?.RegisterHandler<IMixedRealityInputHandler>(this);
        }

        protected override void UnregisterHandlers()
        {
            CoreServices.InputSystem?.UnregisterHandler<IMixedRealityInputHandler>(this);
        }

        public virtual void OnInputDown(InputEventData eventData)
        {
        }

        public virtual void OnInputUp(InputEventData eventData)
        {
        }
    }

    public class TargettableInfoItem : Targettable
    {
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

        public override void OnInputDown(InputEventData eventData)
        {
            if (eventData.MixedRealityInputAction == selectAction)
            {
                Debug.Log("Select!");
                target = !target;
            }
        }
    }
}