// author: Omnistudio
// version: 2024.10.28

using System.Collections.Generic;
using UnityEngine;

namespace Omnis
{
    [RequireComponent(typeof(Collider))]
    public abstract class InteractBase : MonoBehaviour
    {
        #region Serialized Fields
        public bool opaque;
        #endregion

        #region Fields
        private bool interactable;
        private bool isPointed;
        private bool isLeftPressed;
        private bool isRightPressed;
        private bool isMiddlePressed;
        #endregion

        #region Interfaces
        public virtual bool Interactable
        {
            get => interactable;
            set => interactable = value;
        }
        public virtual bool IsLeftPressed
        {
            get => isLeftPressed;
            set => isLeftPressed = value;
        }
        public virtual bool IsRightPressed
        {
            get => isRightPressed;
            set => isRightPressed = value;
        }
        public virtual bool IsMiddlePressed
        {
            get => isMiddlePressed;
            set => isMiddlePressed = value;
        }
        public virtual bool IsPointed
        {
            get => isPointed;
            set => isPointed = value;
        }
        #endregion

        #region Functions
        protected virtual void OnInteracted(List<Collider> hits) {}
        #endregion

        #region Unity Methods
        protected virtual void Start()
        {
            interactable = true;
            isPointed = false;
        }
        #endregion

        #region Handlers
        protected void OnInteract(List<Collider> hits) { if (Interactable) OnInteracted(hits); }
        private void OnLeftPress()      { if (Interactable) IsLeftPressed = true; }
        private void OnLeftRelease()    { if (Interactable) IsLeftPressed = false; }
        private void OnRightPress()     { if (Interactable) IsRightPressed = true; }
        private void OnRightRelease()   { if (Interactable) IsRightPressed = false; }
        private void OnMiddlePress()    { if (Interactable) IsMiddlePressed = true; }
        private void OnMiddleRelease()  { if (Interactable) IsMiddlePressed = false; }
        private void OnPointerEnter()   { if (Interactable) IsPointed = true; }
        private void OnPointerExit()    { if (Interactable) IsPointed = false; }
        #endregion
    }
}
