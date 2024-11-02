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
        #endregion

        #region Fields
        private float countdown;
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
        private void SetBonusMult()
        {
            bonusMult = GameSettings.difficulty switch
            {
                Difficulty.Easy => 1.2f,
                Difficulty.Normal => 1f,
                Difficulty.Hard => 0.8f,
                _ => 1f,
            };
        }

        private void ValuateBonus()
        {
            float bonusTime = vGrade switch
            {
                Grade.Perfect => bonusMult,
                Grade.Qualified => GameSettings.difficulty switch
                {
                    Difficulty.Easy => bonusMult * Mathf.Sqrt(vQualifiedPoint),
                    Difficulty.Normal => bonusMult * vQualifiedPoint,
                    Difficulty.Hard => bonusMult * vQualifiedPoint,
                    _ => bonusMult * vQualifiedPoint,
                },
                Grade.Unqualified => 0f,
                Grade.TooMuch => -1f,
                Grade.Miss => -1f,
                _ => 0f,
            };

            Countdown += bonusTime;

            // Add visual Effects.
            if (GameSettings.EnablePhantoms)
                SpawnPhantom(vVisualColor);
            var floatScore = Instantiate(floatScoreTextPrefab, textCountdown.transform).GetComponent<FloatScore>();
            floatScore.Score = bonusTime;
            floatScore.Color = vVisualColor;
        }
        #endregion
    }
}
