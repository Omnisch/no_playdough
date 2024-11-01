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
        private float qualifiedRatioAbs;
        private float perfectRatioAbs;
        private float bonusMult;
        #endregion

        #region Interfaces
        public float Countdown
        {
            get => countdown;
            set
            {
                countdown = Mathf.Clamp(value, 0f, 1000f);
                textCountdown.text = countdown.ToString("F2");
            }
        }
        #endregion

        #region Functions
        private void SetQualifiedScore()
        {
            switch (GameSettings.difficulty)
            {
                case Difficulty.Easy:
                    qualifiedRatioAbs = 0.15f;
                    perfectRatioAbs = 0.05f;
                    bonusMult = 1.5f;
                    break;
                default:
                case Difficulty.Normal:
                    qualifiedRatioAbs = 0.125f;
                    perfectRatioAbs = 0.02f;
                    bonusMult = 1f;
                    break;
                case Difficulty.Hard:
                    qualifiedRatioAbs = 0.12f;
                    perfectRatioAbs = 0.01f;
                    bonusMult = 0.75f;
                    break;
            }
        }

        private void SettleScore(float endAspectRatio)
        {
            float score;
            Color visualColor;

            // Not even close.
            if (Mathf.Abs(endAspectRatio) > Mathf.Abs(startAspectRatio))
            {
                score = -1f;
                visualColor = Color.red;
            }
            // Too much.
            else if (endAspectRatio * startAspectRatio < 0f)
            {
                score = 0f;
                visualColor = Color.gray;
            }
            // Not qualified.
            else if (Mathf.Abs(endAspectRatio) > qualifiedRatioAbs)
            {
                score = 0f;
                visualColor = Color.red;
            }
            // Qualified.
            else if (Mathf.Abs(endAspectRatio) > perfectRatioAbs)
            {
                var rawScore = (qualifiedRatioAbs - Mathf.Abs(endAspectRatio)) / qualifiedRatioAbs;
                score = GameSettings.difficulty switch
                {
                    Difficulty.Easy => bonusMult * Mathf.Sqrt(rawScore),
                    Difficulty.Normal => bonusMult * rawScore,
                    Difficulty.Hard => bonusMult * rawScore,
                    _ => bonusMult * rawScore,
                };
                visualColor = ColorTweaker.LerpFromColorToColor(ColorTweaker.orange, ColorTweaker.chartreuse, rawScore);
            }
            // Perfect.
            else
            {
                score = bonusMult;
                visualColor = Color.green;
            }

            // Settle the score.
            Countdown += score;

            // Add visual Effects.
            SpawnPerfectPhantom(visualColor);
            var floatScore = Instantiate(floatScoreTextPrefab, textCountdown.transform).GetComponent<FloatScore>();
            floatScore.Score = score;
            floatScore.Color = visualColor;
        }
        #endregion
    }
}
