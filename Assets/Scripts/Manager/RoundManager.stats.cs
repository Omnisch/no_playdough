using UnityEngine;

namespace Omnis.Playdough
{
    public partial class RoundManager
    {
        #region Serialized Fields
        [Header("Stats")]
        [SerializeField] private StatsPanelInRound statsPanel;
        #endregion

        #region Fields
        /// <summary>Qualified line (Absolute).</summary>
        private float Q;
        /// <summary>Perfect line (Absolute).</summary>
        private float P;
        /// <summary>Updated after Valuate().</summary>
        protected Grade vGrade;
        /// <summary>Updated after Valuate(). Only use when qualified.</summary>
        protected float vQualifiedPoint;
        /// <summary>Update after Valuate().</summary>
        protected Color vVisualColor;
        #endregion

        #region Functions
        private void SetQualifiedLine()
        {
            switch (GameSettings.difficulty)
            {
                case Difficulty.Easy:
                    Q = 0.06f;
                    P = 0.02f;
                    break;
                default:
                case Difficulty.Normal:
                    Q = 0.03f;
                    P = 0.01f;
                    break;
                case Difficulty.Hard:
                    Q = 0.01f;
                    P = 0.005f;
                    break;
            }
        }

        protected void Valuate(float endAspectRatio)
        {
            float solvingTime = Time.realtimeSinceStartup - startTime;

            // Perfect.
            if (Mathf.Abs(endAspectRatio) < P)
            {
                vGrade = Grade.Perfect;
                vVisualColor = Color.green;
                Statistics.perfectCount++;
            }
            // Qualified.
            else if (Mathf.Abs(endAspectRatio) < Q)
            {
                vGrade = Grade.Qualified;
                vQualifiedPoint = (Q - Mathf.Abs(endAspectRatio)) / Q;
                vVisualColor = ColorTweaker.Lerp(ColorTweaker.orange, Color.green, vQualifiedPoint);
                Statistics.qualifiedCount++;
            }
            // Too much.
            else if (endAspectRatio * startAspectRatio < 0f)
            {
                vGrade = Grade.TooMuch;
                vVisualColor = Color.red;
                Statistics.tooMuchCount++;
            }
            // Unqualified.
            else if (Mathf.Abs(endAspectRatio) < Mathf.Abs(startAspectRatio))
            {
                vGrade = Grade.Unqualified;
                vVisualColor = ColorTweaker.amber;
                Statistics.unqualifiedCount++;
            }
            // Miss.
            else
            {
                vGrade = Grade.Miss;
                vVisualColor = Color.gray;
                Statistics.missCount++;
            }

            Statistics.averageTime = (Statistics.averageTime * (Statistics.OneRoundCount - 1) + solvingTime) / Statistics.OneRoundCount;
        }
        #endregion

        public enum Grade
        {
            Perfect,
            Qualified,
            Unqualified,
            TooMuch,
            Miss,
        }
    }
}
