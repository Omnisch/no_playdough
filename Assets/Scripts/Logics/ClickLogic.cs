// author: Omnistudio
// version: 2024.10.28

using UnityEngine.Events;

namespace Omnis
{
    public class ClickLogic : InteractBase
    {
        #region Serialized Fields
        public UnityEvent enterCallback;
        public UnityEvent pressCallback;
        public UnityEvent releaseCallback;
        public UnityEvent exitCallback;
        #endregion

        #region Interfaces
        public override bool IsPointed
        {
            get => base.IsPointed;
            set
            {
                base.IsPointed = value;
                if (value) enterCallback?.Invoke();
                else exitCallback?.Invoke();
            }
        }
        public override bool IsLeftPressed
        {
            get => base.IsLeftPressed;
            set
            {
                base.IsLeftPressed = value;
                if (value) pressCallback?.Invoke();
                else releaseCallback?.Invoke();
            }
        }
        #endregion
    }
}
