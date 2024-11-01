using UnityEngine;
using UnityEngine.UI;

namespace Omnis.Playdough
{
    public class FloatScore : TTLMonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private Text floatText;
        [SerializeField] private float offset;
        #endregion

        #region Fields
        private Vector3 rootPosition;
        #endregion

        #region Interfaces
        public float Score
        {
            set => floatText.text = value >= 0f ? "+" + value.ToString("F2") : value.ToString("F2");
        }
        public Color Color
        {
            set => floatText.color = value;
        }
        #endregion

        #region Life Span
        protected override void OnStart()
        {
            rootPosition = transform.position;
            OnLifeSpan = OnFloatScoreTextLifeSpan;
        }
        private void OnFloatScoreTextLifeSpan(float value)
        {
            floatText.color = new(floatText.color.r, floatText.color.g, floatText.color.b, value);
            transform.position = rootPosition + offset * value * Vector3.up;
        }
        #endregion
    }
}
