// author: Omnistudio
// version: 2024.10.31

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Omnis
{
    /// <summary>
    /// Hooked with .inputactions in the new Input System.
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    public class InputHandler : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] protected UnityEvent debugLogic;
        #endregion

        #region Fields
        private PlayerInput playerInput;
        private List<Collider> PointerHits;
        #endregion

        #region Interfaces
        public void SetInputEnabled(bool value)
        {
            enabled = value;
            Cursor.visible = value;
        }

        public static Vector2 PointerPosition {  get; private set; }
        #endregion

        #region Functions
        protected void ForwardMessage(string methodName, object value = null)
        {
            foreach (var hit in PointerHits)
            {
                hit.SendMessage("OnInteract", PointerHits, SendMessageOptions.DontRequireReceiver);
                hit.SendMessage(methodName, value, SendMessageOptions.DontRequireReceiver);
                if (hit.GetComponent<InteractBase>() && hit.GetComponent<InteractBase>().opaque) break;
            }
        }
        #endregion

        #region Unity Methods
        private void Start()
        {
            playerInput = GetComponent<PlayerInput>();

            foreach (var map in playerInput.actions.actionMaps)
                map.Enable();

            PointerHits = new();
        }
        private void OnEnable()
        {
            if (playerInput) playerInput.enabled = true;
        }
        private void OnDisable()
        {
            if (playerInput) playerInput.enabled = false;
        }
        #endregion

        #region Handlers
        protected virtual void OnLeftPress() => ForwardMessage("OnLeftPress");
        protected virtual void OnLeftRelease() => ForwardMessage("OnLeftRelease");
        protected virtual void OnRightPress() => ForwardMessage("OnRightPress");
        protected virtual void OnRightRelease() => ForwardMessage("OnRightRelease");
        protected virtual void OnMiddlePress() => ForwardMessage("OnMiddlePress");
        protected virtual void OnMiddleRelease() => ForwardMessage("OnMiddleRelease");
        protected virtual void OnScroll(InputValue value) => ForwardMessage("OnScroll", value.Get<float>());
        protected virtual void OnPointer(InputValue value)
        {
            PointerPosition = value.Get<Vector2>();
            Ray r = Camera.main.ScreenPointToRay(value.Get<Vector2>());
            var rawHits = Physics.RaycastAll(r);
            System.Array.Sort(rawHits, (a, b) => a.distance.CompareTo(b.distance));
            List<Collider> newHits = rawHits.Select(hit => hit.collider).ToList();
            foreach (var hit in PointerHits.Except(newHits).ToList())
                if (hit)
                {
                    hit.SendMessage("OnPointerExit", options: SendMessageOptions.DontRequireReceiver);
                    if (hit.GetComponent<InteractBase>() && hit.GetComponent<InteractBase>().opaque) break;
                }
            foreach (var hit in newHits.Except(PointerHits).ToList())
                if (hit)
                {
                    hit.SendMessage("OnPointerEnter", options: SendMessageOptions.DontRequireReceiver);
                    if (hit.GetComponent<InteractBase>() && hit.GetComponent<InteractBase>().opaque) break;
                }
            PointerHits = newHits;
        }
        protected virtual void OnSave() => ForwardMessage("OnSave");
        protected virtual void OnLoad() => ForwardMessage("OnLoad");
        protected virtual void OnDebugTest() => debugLogic?.Invoke();
        protected virtual void OnEscape()
        {
#if UNITY_STANDALONE
            Application.Quit();
#elif UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
        #endregion
    }
}
