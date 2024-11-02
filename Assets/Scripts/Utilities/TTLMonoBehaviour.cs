// author: Omnistudio
// version: 2024.11.02

using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Omnis
{
    /// <summary>
    /// After Start(), live <i>lifeTime</i> seconds and then destroy gameObject.<br />
    /// Use <i>OnLifeSpan</i> to set callbacks.
    /// </summary>
    public class TTLMonoBehaviour : MonoBehaviour
    {
        [SerializeField] private float lifeTime = 1f;
        public TTLMonoBehaviour SetLifeTime(float value)
        {
            lifeTime = value;
            return this;
        }

        /// <summary>
        /// The float value would be 0 at the start, 1 at the end.
        /// </summary>
        public UnityAction<float> OnLifeSpan {  private get; set; }

        private float life;
        private float Life
        {
            get => life;
            set
            {
                life = Mathf.Clamp01(value);
                OnLifeSpan?.Invoke(value);
            }
        }


        #region Life Span
        protected virtual void OnStart() { }
        private void Start()
        {
            Life = 0f;
            OnStart();
            StartCoroutine(LifeSpan());
        }

        private IEnumerator LifeSpan()
        {
            var lifeTimeFixed = lifeTime;
            while (Life < 1f)
            {
                Life += Time.deltaTime / lifeTimeFixed;
                yield return 0;
            }
            Destroy(gameObject);
        }
        #endregion
    }
}
