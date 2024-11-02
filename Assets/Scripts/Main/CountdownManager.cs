using UnityEngine;

namespace Omnis.Playdough
{
    public partial class CountdownManager : RoundManager
    {
        #region Serialized Fields
        [Header("Input")]
        [SerializeField] private Vector3 slideInPosition;
        [SerializeField] private Vector3 slideOutPosition;
        #endregion

        #region Fields
        #endregion

        #region Properties
        public override bool IsLeftPressed
        {
            get => base.IsLeftPressed;
            set
            {
                base.IsLeftPressed = value;
                if (!value)
                {
                    ValuateBonus();
                    NextPlaydough();
                }
            }
        }
        #endregion

        #region Functions
        private void NextPlaydough()
        {
            if (activeDough)
                activeDough.SlideOut(Vector3.zero, slideOutPosition, destroy: true);

            activeDough = SpawnPlaydough();
            activeDough.SlideIn(slideInPosition, Vector3.zero);
        }
        #endregion

        #region Unity Methods
        protected override void Start()
        {
            base.Start();
            SetBonusMult();
            Countdown = 10f;
            NextPlaydough();
        }

        protected override void Update()
        {
            base.Update();
            if (!Interactable) return;

            Countdown -= Time.deltaTime;
            if (Countdown <= 0f) GameOver();
        }
        #endregion
    }
}
