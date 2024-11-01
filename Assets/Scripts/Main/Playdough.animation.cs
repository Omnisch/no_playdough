using System.Collections;
using UnityEngine;

namespace Omnis.Playdough
{
    public partial class Playdough
    {
        #region Serialized fields
        [SerializeField] private float lerpSpeed;
        #endregion

        #region Functions
        public void SlideIn(Vector3 fromPosition)
        {
            StopAllCoroutines();
            StartCoroutine(ISlideIn(fromPosition));
        }
        private IEnumerator ISlideIn(Vector3 fromPosition)
        {
            Position = fromPosition;
            Color = ColorTweaker.rgbx(Color, 0f);
            while (Position.sqrMagnitude > 0.01f)
            {
                Position = Vector3.Lerp(Position, Vector3.zero, lerpSpeed);
                Color = ColorTweaker.LerpFromColorToColor(Color, ColorTweaker.rgbx(Color, 1f), lerpSpeed);
                yield return 0;
            }
            Position = Vector3.zero;
            Color = ColorTweaker.rgbx(Color, 1f);
        }

        public void SlideOut(Vector3 toPosition)
        {
            StopAllCoroutines();
            StartCoroutine(ISlideOut(toPosition));
        }
        private IEnumerator ISlideOut(Vector3 toPosition)
        {
            while ((toPosition - Position).sqrMagnitude > 0.01f)
            {
                Position = Vector3.Lerp(Position, toPosition, lerpSpeed);
                Color = ColorTweaker.LerpFromColorToColor(Color, ColorTweaker.rgbx(Color, 0f), lerpSpeed);
                yield return 0;
            }
            Position = toPosition;
            Color = ColorTweaker.rgbx(Color, 0f);
        }

        public void SlideOutAndDestroy(Vector3 toPosition)
        {
            StopAllCoroutines();
            StartCoroutine(ISlideOutAndDestroy(toPosition));
        }
        private IEnumerator ISlideOutAndDestroy(Vector3 toPosition)
        {
            yield return ISlideOut(toPosition);
            Destroy(gameObject);
        }
        #endregion
    }
}
