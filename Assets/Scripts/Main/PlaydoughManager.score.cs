using UnityEngine;
using UnityEngine.UI;

namespace Omnis.Playdough
{
    public partial class PlaydoughManager
    {
        #region Serialized Fields
        [Header("Scores")]
        [SerializeField] private Text textCountdown;
        [SerializeField] private GameObject floatScoreTextPrefab;
        #endregion

        #region Fields
        private float countdown;
        #endregion

        #region Interfaces
        public float Countdown
        {
            get => countdown;
            set
            {
                countdown = Mathf.Clamp(value, 0f, 999f);
                textCountdown.text = countdown.ToString("F2");
            }
        }
        #endregion

        #region Functions
        private void SettleScore(float endAspectRatio)
        {
            float score;
            if (endAspectRatio * startAspectRatio < 0f)
            {
                score = 0f;
                SpawnPerfectPhantom(Color.yellow);
            }
            else if (Mathf.Abs(endAspectRatio) > Mathf.Abs(startAspectRatio))
            {
                score = -5f;
                SpawnPerfectPhantom(Color.red);
            }
            else
            {
                score = Mathf.Lerp(0f, 1f, Mathf.Abs(startAspectRatio - endAspectRatio));
                score = score * score * GameSettings.bonusMult;
                SpawnPerfectPhantom(Color.green);
            }
            Countdown += score;
            Instantiate(floatScoreTextPrefab, textCountdown.transform).GetComponent<FloatScoreText>().Score = score;
        }
        #endregion
    }
}
