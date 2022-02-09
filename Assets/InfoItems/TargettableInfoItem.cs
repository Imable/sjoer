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
        private TargettableInfoItem link = null;

        public bool IsTarget
        {
            get { return target; }
            set { target = value; }
        }

        private bool HasLinkedInfoItem()
        {
            return link != null;
        }

        public void SetLink(TargettableInfoItem link)
        {
            this.link = link;
        }

        public void DestroyLink()
        {
            this.link = null;
        }

        private void OnClick()
        {
            target = !target;
            Debug.Log("target is now " + target);
            if (HasLinkedInfoItem()) link.IsTarget = target;
        }

        public override void OnInputDown(InputEventData eventData)
        {
            if (eventData.MixedRealityInputAction == selectAction)
            {
                Debug.Log("Select!");
                OnClick();
            }
        }
    }
}