using UnityEngine;

namespace Omnis.Playdough
{
    public partial class PlaydoughManager : InteractBase
    {
        #region Serialized Fields
        [SerializeField] private GameObject playdoughPrefab;
        [Header("Input")]
        [SerializeField] private GameObject crosshair;
        [SerializeField] private float sensitivity;
        #endregion

        #region Fields
        private Playdough playdough;
        private Vector2 startPointerPosition;
        private float startAspectRatio;
        #endregion

        #region Interfaces
        public override bool IsLeftPressed
        {
            get => base.IsLeftPressed;
            set
            {
                base.IsLeftPressed = value;
                if (value)
                {
                    startPointerPosition = InputHandler.PointerPosition;
                    if (playdough) startAspectRatio = playdough.AspectRatio;
                }
                else
                {
                    if (playdough) SettleScore(playdough.AspectRatio);
                    SpawnPlaydough();
                }
            }
        }
        #endregion

        #region Functions
        private void SpawnPlaydough()
        {
            if (playdough)
                Destroy(playdough.gameObject);
            playdough = Instantiate(playdoughPrefab).GetComponent<Playdough>();
            playdough.Shape = GameManager.Instance.GetRandomShapeFromPool();
            if (GameManager.Instance.RandomScale) playdough.Scale = Random.Range(1f, 2f);
            if (GameManager.Instance.RandomRotation) playdough.Rotation = Random.Range(0f, 360f);
            playdough.AspectRatio = Mathf.Sign(Random.Range(-1f, 1f)) * Random.Range(0.2f, 0.5f);
        }
        #endregion

        #region Unity Methods
        protected override void Start()
        {
            base.Start();
            Countdown = 10f;
            crosshair.SetActive(GameManager.Instance.EnableCrosshair);
            SpawnPlaydough();
        }

        private void Update()
        {
            if (!Interactable) return;

            Countdown -= Time.deltaTime;
            if (Countdown <= 0f)
            {
                Interactable = false;
                Debug.Log("Game over.");
            }

            if (IsLeftPressed)
            {
                playdough.AspectRatio = startAspectRatio
                    + Mathf.Sign(startPointerPosition.x - Screen.width / 2) * (0.001f * sensitivity) * (InputHandler.PointerPosition.x - startPointerPosition.x)
                    + Mathf.Sign(Screen.height / 2 - startPointerPosition.y) * (0.001f * sensitivity) * (InputHandler.PointerPosition.y - startPointerPosition.y);
            }
        }
        #endregion
    }
}
