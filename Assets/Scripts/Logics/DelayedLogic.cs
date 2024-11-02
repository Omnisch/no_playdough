// author: Omnistudio
// version: 2024.11.02

using UnityEngine;

namespace Omnis
{
    /// <summary>
    /// When Invoke() is called, delay for <i>delayTime</i> seconds first.
    /// </summary>
    public class DelayedLogic : Logic
    {
        [Tooltip("In seconds.")]
        [Min(0)] public float delayTime;

        public override void Invoke() => StartCoroutine(InvokingCoroutine());

        private System.Collections.IEnumerator InvokingCoroutine()
        {
            yield return new WaitForSecondsRealtime(delayTime);
            callback.Invoke();
        }
    }
}
