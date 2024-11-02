using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Omnis.Playdough
{
    [RequireComponent(typeof(Playdough))]
    public class PlaydoughButton : InteractBase
    {
        #region Serialized Fields
        [SerializeField] private float sensitivity;
        [SerializeField] private float lerpSpeed = 0.02f;
        [SerializeField] private float confirmRatio = 0.1f;
        [SerializeField] private UnityEvent callback;
        #endregion

        #region Fields
        private Playdough playdough;
        private float originalAspectRatio;
        private Vector2 startPointerPosition;
        private bool confirmable;
        #endregion

        #region Properties
        public override bool Interactable
        {
            get => base.Interactable;
            set
            {
                base.Interactable = value;
                if (value) playdough.Color = ColorTweaker.appleBlack;
                else playdough.Color = Color.gray;
            }
        }
        public override bool IsLeftPressed
        {
            get => base.IsLeftPressed;
            set
            {
                base.IsLeftPressed = value;
                StopAllCoroutines();
                if (value)
                {
                    startPointerPosition = InputHandler.PointerPosition;
                }
                else
                {
                    if (confirmable) callback?.Invoke();
                    StartCoroutine(BounceBack());
                }
            }
        }

        private float AspectRatio
        {
            get => playdough.AspectRatio;
            set
            {
                if (Mathf.Abs(value) < Mathf.Abs(confirmRatio))
                {
                    if (IsLeftPressed)
                        playdough.AspectRatio = Mathf.Lerp(playdough.AspectRatio, 0f, lerpSpeed);
                    else
                        playdough.AspectRatio = value;
                    playdough.Color = ColorTweaker.LerpFromColorToColor(playdough.Color, ColorTweaker.chartreuse, lerpSpeed);
                    confirmable = true;
                }
                else
                {
                    playdough.AspectRatio = value;
                    playdough.Color = ColorTweaker.LerpFromColorToColor(playdough.Color, ColorTweaker.appleBlack, lerpSpeed);
                    confirmable = false;
                }
            }
        }
        #endregion

        #region Functions
        private IEnumerator BounceBack()
        {
            while (Mathf.Abs(AspectRatio - originalAspectRatio) > Mathf.Epsilon)
            {
                AspectRatio = Mathf.Lerp(AspectRatio, originalAspectRatio, 10f * Time.deltaTime);
                yield return 0;
            }
            AspectRatio = originalAspectRatio;
        }
        #endregion

        #region Unity Methods
        protected override void Start()
        {
            base.Start();
            playdough = GetComponent<Playdough>();
            originalAspectRatio = playdough.AspectRatio;
        }

        private void Update()
        {
            if (!Interactable) return;

            if (IsLeftPressed)
            {
                AspectRatio = originalAspectRatio
                    + Mathf.Sign(startPointerPosition.x - Screen.width / 2) * (0.001f * sensitivity) * (InputHandler.PointerPosition.x - startPointerPosition.x)
                    + Mathf.Sign(Screen.height / 2 - startPointerPosition.y) * (0.001f * sensitivity) * (InputHandler.PointerPosition.y - startPointerPosition.y);
            }
            #endregion
        }
    }
}
