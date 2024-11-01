using System;
using UnityEngine;
using UnityEngine.UI;

namespace Omnis.Playdough
{
    public class StartupManager : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private Button startButton;
        #endregion

        #region Interfaces
        public void StartGame() => UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("MainScene");

        public void ToggleShapeToPool(string shapeName)
        {
            if (Enum.TryParse(typeof(PlaydoughShape), shapeName, out var result))
            {
                if (GameManager.Instance.shapePool.Contains((PlaydoughShape)result))
                    GameManager.Instance.shapePool.Remove((PlaydoughShape)result);
                else
                    GameManager.Instance.shapePool.Add((PlaydoughShape)result);
                startButton.interactable = !IsShapePoolEmpty();
            }
        }
        #endregion

        #region Functions
        private bool IsShapePoolEmpty() => GameManager.Instance.shapePool.Count == 0;
        #endregion

        #region Unity Methods
        private void Start()
        {
            GameManager.Instance.shapePool.Clear();
            GameManager.Instance.RandomScale = false;
            GameManager.Instance.RandomRotation = false;
            GameManager.Instance.EnableCrosshair = false;
            startButton.interactable = !IsShapePoolEmpty();
        }
        #endregion
    }
}
