// author: Omnistudio
// version: 2024.11.02

using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Omnis
{
    public class YieldTweaker
    {
        #region Static Functions
        /// <summary>
        /// It takes <i>time</i> seconds to accumulate from 0 to 1.
        /// </summary>
        public static IEnumerator Linear(UnityAction<float> action, float time = 1f)
        {
            var life = 0f;
            while (life < 1f)
            {
                action?.Invoke(life);
                life += Time.deltaTime / time;
                yield return 0;
            }
            life = 1f;
            action?.Invoke(life);
        }

        /// <summary>
        /// It continuously lerps from 0 to 1 by <i>t</i>.
        /// </summary>
        public static IEnumerator Lerp(UnityAction<float> action, float speed = 1f)
        {
            var life = 0f;
            while (1f - life > Mathf.Epsilon)
            {
                action?.Invoke(life);
                life = Mathf.Lerp(life, 1f, speed * Time.deltaTime);
                yield return 0;
            }
            life = 1f;
            action?.Invoke(life);
        }
        #endregion
    }
}
