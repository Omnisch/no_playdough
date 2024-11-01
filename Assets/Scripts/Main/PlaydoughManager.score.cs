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
        [SerializeField] private StatsPanelInRound statsPanel;
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
            float solvingTime = Time.realtimeSinceStartup - startTime;
            float bonusTime;
            Color visualColor;

            // Not even close.
            if (Mathf.Abs(endAspectRatio) > Mathf.Abs(startAspectRatio))
            {
                bonusTime = -1f;
                visualColor = Color.red;
                Statistics.missCount++;
            }
            // Too much.
            else if (endAspectRatio * startAspectRatio < 0f)
            {
                bonusTime = 0f;
                visualColor = Color.red;
                Statistics.tooMuchCount++;
            }
            // Unqualified.
            else if (Mathf.Abs(endAspectRatio) > qualifiedRatioAbs)
            {
                bonusTime = 0f;
                visualColor = Color.gray;
                Statistics.unqualifiedCount++;
            }
            // Qualified.
            else if (Mathf.Abs(endAspectRatio) > perfectRatioAbs)
            {
                var rawScore = (qualifiedRatioAbs - Mathf.Abs(endAspectRatio)) / qualifiedRatioAbs;
                bonusTime = GameSettings.difficulty switch
                {
                    Difficulty.Easy => bonusMult * Mathf.Sqrt(rawScore),
                    Difficulty.Normal => bonusMult * rawScore,
                    Difficulty.Hard => bonusMult * rawScore,
                    _ => bonusMult * rawScore,
                };
                visualColor = ColorTweaker.LerpFromColorToColor(ColorTweaker.orange, ColorTweaker.chartreuse, rawScore);
                Statistics.qualifiedCount++;
            }
            // Perfect.
            else
            {
                bonusTime = bonusMult;
                visualColor = Color.green;
                Statistics.perfectCount++;
            }

            // Settle the score.
            Countdown += bonusTime;
            if (bonusTime > 0f)
                Statistics.averageTime = (Statistics.averageTime * (Statistics.OneRoundCount - 1) + solvingTime) / Statistics.OneRoundCount;

            // Add visual Effects.
            SpawnPerfectPhantom(visualColor);
            var floatScore = Instantiate(floatScoreTextPrefab, textCountdown.transform).GetComponent<FloatScore>();
            floatScore.Score = bonusTime;
            floatScore.Color = visualColor;
        }
        #endregion
    }
}
