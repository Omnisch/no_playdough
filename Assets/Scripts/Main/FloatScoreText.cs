using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Omnis.Playdough
{
    public class FloatScoreText : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private Text floatText;
        [SerializeField][Min(0.1f)] private float lifeSpeed = 1f;
        [SerializeField] private float offset;
        #endregion

        #region Interfaces
        public float Score
        {
            set => floatText.text = value >= 0f ? "+" + value.ToString("F2") : value.ToString("F2");
        }
        #endregion

        #region Life Span
        private void Start() => StartCoroutine(LifeSpan());

        private IEnumerator LifeSpan()
        {
            var rootPosition = transform.position;
            float life = 1f;
            while (life > 0f)
            {
                floatText.color = new(floatText.color.r, floatText.color.g, floatText.color.b, life);
                transform.position = rootPosition + offset * life * Vector3.up;
                life -= lifeSpeed * Time.deltaTime;
                yield return 0;
            }
            Destroy(gameObject);
        }
        #endregion
    }
}
