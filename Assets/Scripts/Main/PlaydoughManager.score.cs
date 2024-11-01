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

        public void BackToStartScene() => UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("StartScene");
        #endregion

        #region Functions
        private void SettleScore(float endAspectRatio)
        {
            float score;
            if (endAspectRatio == 0f)
            {
                score = 100f;
                Debug.Log("Perfect! But is it really possible?");
            }
            else if (endAspectRatio * startAspectRatio < 0f)
            {
                score = 0f;
                Debug.Log("That's too far.");
            }
            else if (Mathf.Abs(endAspectRatio) > Mathf.Abs(startAspectRatio))
            {
                score = -5f;
                Debug.Log("Are you serious?");
            }
            else
            {
                score = Mathf.Lerp(0f, 1f, Mathf.Abs(startAspectRatio - endAspectRatio));
                score = score * score * GameManager.Instance.bonusMult;
                Debug.Log("Good.");
            }
            Countdown += score;
            Instantiate(floatScoreTextPrefab, textCountdown.transform).GetComponent<FloatScoreText>().Score = score;
        }
        #endregion
    }
}
