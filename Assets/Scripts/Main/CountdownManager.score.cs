using UnityEngine;
using UnityEngine.UI;

namespace Omnis.Playdough
{
    public partial class CountdownManager
    {
        #region Serialized Fields
        [Header("Scores")]
        [SerializeField] private Text textCountdown;
        [SerializeField] private GameObject floatScoreTextPrefab;
        [SerializeField] private StatsPanelInRound statsPanel;
        #endregion

        #region Fields
        private float countdown;
        /// <summary>Qualified line (Absolute).</summary>
        private float Q;
        /// <summary>Perfect line (Absolute).</summary>
        private float P;
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
                    Q = 0.06f;
                    P = 0.02f;
                    bonusMult = 1.2f;
                    break;
                default:
                case Difficulty.Normal:
                    Q = 0.03f;
                    P = 0.01f;
                    bonusMult = 1f;
                    break;
                case Difficulty.Hard:
                    Q = 0.01f;
                    P = 0.005f;
                    bonusMult = 0.8f;
                    break;
            }
        }

        private void SettleScore(float endAspectRatio)
        {
            float solvingTime = Time.realtimeSinceStartup - startTime;
            float bonusTime;
            Color visualColor;

            // Perfect.
            if (Mathf.Abs(endAspectRatio) < P)
            {
                bonusTime = bonusMult;
                visualColor = Color.green;
                Statistics.perfectCount++;
            }
            // Qualified.
            else if (Mathf.Abs(endAspectRatio) < Q)
            {
                var rawScore = (Q - Mathf.Abs(endAspectRatio)) / Q;
                bonusTime = GameSettings.difficulty switch
                {
                    Difficulty.Easy => bonusMult * Mathf.Sqrt(rawScore),
                    Difficulty.Normal => bonusMult * rawScore,
                    Difficulty.Hard => bonusMult * rawScore,
                    _ => bonusMult * rawScore,
                };
                visualColor = ColorTweaker.Lerp(ColorTweaker.orange, Color.green, rawScore);
                Statistics.qualifiedCount++;
            }
            // Too much.
            else if (endAspectRatio * startAspectRatio < 0f)
            {
                bonusTime = -1f;
                visualColor = Color.red;
                Statistics.tooMuchCount++;
            }
            // Unqualified.
            else if (Mathf.Abs(endAspectRatio) < Mathf.Abs(startAspectRatio))
            {
                bonusTime = 0f;
                visualColor = ColorTweaker.amber;
                Statistics.unqualifiedCount++;
            }
            // Miss.
            else
            {
                bonusTime = -1f;
                visualColor = Color.gray;
                Statistics.missCount++;
            }

            // Settle the score.
            Countdown += bonusTime;
            Statistics.averageTime = (Statistics.averageTime * (Statistics.OneRoundCount - 1) + solvingTime) / Statistics.OneRoundCount;

            // Add visual Effects.
            SpawnPhantom(visualColor);
            var floatScore = Instantiate(floatScoreTextPrefab, textCountdown.transform).GetComponent<FloatScore>();
            floatScore.Score = bonusTime;
            floatScore.Color = visualColor;
        }
        #endregion
    }
}
