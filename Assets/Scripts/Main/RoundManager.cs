using UnityEngine;

namespace Omnis.Playdough
{
    public partial class RoundManager : InteractBase
    {
        #region Serialized Fields
        [SerializeField] protected GameObject playdoughPrefab;
        [SerializeField] private GameObject crosshair;
        #endregion

        #region Fields
        protected Playdough activeDough;
        private float startTime;
        private float startAspectRatio;
        private Vector2 startPointerPos;
        private Vector2 direction;
        #endregion

        #region Properties
        public override bool IsLeftPressed
        {
            get => base.IsLeftPressed;
            set
            {
                base.IsLeftPressed = value;
                if (value)
                {
                    startTime = Time.realtimeSinceStartup;
                    startPointerPos = InputHandler.PointerPosition;
                    if (activeDough)
                    {
                        startAspectRatio = activeDough.AspectRatio;
                        direction = new(
                            +Mathf.Sign(startPointerPos.x - Camera.main.WorldToScreenPoint(activeDough.transform.position).x),
                            -Mathf.Sign(startPointerPos.y - Camera.main.WorldToScreenPoint(activeDough.transform.position).y));
                    }
                }
                else
                {
                    if (activeDough) Valuate(activeDough.AspectRatio);
                }
            }
        }
        #endregion

        #region Functions
        protected Playdough SpawnPlaydough()
        {
            var playdough = Instantiate(playdoughPrefab).GetComponent<Playdough>();
            playdough.Shape = GameSettings.GetRandomShapeFromPool();
            if (GameSettings.RandomScale)
                playdough.Scale = Random.Range(1f, 2f);
            else
                playdough.Scale = 1.5f;
            if (GameSettings.RandomRotation)
                playdough.Rotation = Random.Range(0f, 360f);
            playdough.AspectRatio = Mathf.Sign(Random.Range(-1f, 1f)) * Random.Range(0.25f, 0.5f);
            return playdough;
        }

        protected Playdough SpawnPhantom(Color color)
        {
            if (!activeDough) return null;

            var phantom = Instantiate(playdoughPrefab).GetComponent<Playdough>();
            activeDough.CopyTo(phantom);
            phantom.Color = color;
            phantom.gameObject.AddComponent<TTLMonoBehaviour>().SetLifeTime(3f).OnLifeSpan = (value) =>
            {
                phantom.Color = new(phantom.Color.r, phantom.Color.g, phantom.Color.b, (1f - value) / 2f);
            };
            return phantom;
        }

        public void GameOver()
        {
            Interactable = false;
            Statistics.AddOneRoundToTotal();
            statsPanel.ShowPanel();
        }

        public void BackToMenu() => UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Menu");
        #endregion

        #region Unity Methods
        protected override void Start()
        {
            base.Start();
            SetQualifiedLine();
            Statistics.ResetOneRoundStatistics();
            crosshair.SetActive(GameSettings.EnableCrosshair);
        }

        protected virtual void Update()
        {
            if (!Interactable) return;

            Statistics.playedTime += Time.deltaTime;

            if (IsLeftPressed)
            {
                activeDough.AspectRatio = startAspectRatio + GameSettings.mouseSensitivity * 0.001f * (
                    direction.x * (InputHandler.PointerPosition.x - startPointerPos.x) +
                    direction.y * (InputHandler.PointerPosition.y - startPointerPos.y));
            }
        }
        #endregion
    }
}
