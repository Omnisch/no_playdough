using System.Collections;
using UnityEngine;

namespace Omnis.Playdough
{
    public partial class Playdough
    {
        #region Functions
        private IEnumerator ISlide(Vector3 fromPos, Vector3 toPos, SlideMode slideMode)
        {
            yield return YieldTweaker.Lerp((value) =>
            {
                transform.position = Vector3.Lerp(fromPos, toPos, value);
                Color = ColorTweaker.Lerp(
                    ColorTweaker.rgbx(Color, slideMode switch
                    {
                        SlideMode.Appear => 0f,
                        SlideMode.Fade => 1f,
                        _ => Color.a,
                    }),
                    ColorTweaker.rgbx(Color, slideMode switch
                    {
                        SlideMode.Appear => 1f,
                        SlideMode.Fade => 0f,
                        _ => Color.a,
                    }), value);
            }, GameSettings.lerpMult);
        }

        public void SlideIn(Vector3 fromPos, Vector3 toPos)
        {
            StopAllCoroutines();
            StartCoroutine(ISlide(fromPos, toPos, SlideMode.Appear));
        }
        public void SlideOut(Vector3 fromPos, Vector3 toPos, bool destroy = false)
        {
            StopAllCoroutines();
            if (destroy)
                StartCoroutine(ISlideOutAndDestroy(fromPos, toPos));
            else
                StartCoroutine(ISlide(fromPos, toPos, SlideMode.Fade));
        }
        private IEnumerator ISlideOutAndDestroy(Vector3 fromPos, Vector3 toPos)
        {
            yield return ISlide(fromPos, toPos, SlideMode.Fade);
            Destroy(gameObject);
        }
        #endregion

        public enum SlideMode { Opaque, Appear, Fade, };
    }
}
