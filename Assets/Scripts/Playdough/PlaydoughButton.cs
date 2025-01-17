using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Omnis.Playdough
{
    [RequireComponent(typeof(Playdough))]
    public class PlaydoughButton : InteractBase
    {
        #region Serialized Fields
        [SerializeField] private float confirmRatio = 0.05f;
        [SerializeField] private UnityEvent callback;
        #endregion

        #region Fields
        private Playdough playdough;
        private float originalAspectRatio;
        private Vector2 startPointerPos;
        private Vector2 direction;
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
        public override bool IsPointed
        {
            get => base.IsPointed;
            set
            {
                base.IsPointed = value;
                if (!value && IsLeftPressed) IsLeftPressed = false;
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
                    startPointerPos = InputHandler.PointerPosition;
                    direction = new(
                        +Mathf.Sign(startPointerPos.x - Camera.main.WorldToScreenPoint(transform.position).x),
                        -Mathf.Sign(startPointerPos.y - Camera.main.WorldToScreenPoint(transform.position).y));
                }
                else
                {
                    if (Confirmed) callback?.Invoke();
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
                        playdough.AspectRatio = Mathf.Lerp(playdough.AspectRatio, 0f, GameSettings.lerpMult * Time.deltaTime);
                    else
                        playdough.AspectRatio = value;
                    playdough.Color = ColorTweaker.Lerp(playdough.Color, ColorTweaker.chartreuse, GameSettings.lerpMult * Time.deltaTime);
                }
                else
                {
                    playdough.AspectRatio = value;
                    playdough.Color = ColorTweaker.Lerp(playdough.Color, ColorTweaker.appleBlack, GameSettings.lerpMult * Time.deltaTime);
                }
            }
        }
        private bool Confirmed => Mathf.Abs(AspectRatio) < Mathf.Abs(confirmRatio);
        #endregion

        #region Functions
        private IEnumerator BounceBack()
        {
            var fromAspectRatio = AspectRatio;
            yield return YieldTweaker.Lerp((value) =>
            {
                AspectRatio = Mathf.Lerp(fromAspectRatio, originalAspectRatio, value);
            }, GameSettings.lerpMult);
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
                AspectRatio = originalAspectRatio + 0.001f * GameSettings.mouseSensitivity * (
                    direction.x * (InputHandler.PointerPosition.x - startPointerPos.x) +
                    direction.y * (InputHandler.PointerPosition.y - startPointerPos.y));
            }
            #endregion
        }
    }
}
