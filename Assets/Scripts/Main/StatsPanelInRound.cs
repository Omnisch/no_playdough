using System;
using UnityEngine;
using UnityEngine.UI;

namespace Omnis.Playdough
{
    public class StatsPanelInRound : StatsPanel
    {
        #region Serialized fields
        [SerializeField] private Text difficulty;
        [SerializeField] private Text randomScale;
        [SerializeField] private Text randomRotation;
        [SerializeField] private Text enableCrosshair;
        [SerializeField] private Text enablePhantoms;
        [SerializeField] private Text shapeCount;
        #endregion

        #region Unity methods
        private void OnEnable()
        {
            perfect.text = Statistics.perfectCount.ToString();
            qualified.text = Statistics.qualifiedCount.ToString();
            unqualified.text = Statistics.unqualifiedCount.ToString();
            tooMuch.text = Statistics.tooMuchCount.ToString();
            miss.text = Statistics.missCount.ToString();
            total.text = Statistics.OneRoundCount.ToString();
            accuracy.text = Statistics.Accuracy.ToString("F0") + "%";
            avgTime.text = Statistics.averageTime.ToString("F2") + "s";
            playedTime.text = Statistics.playedTime.ToString("F2") + "s";

            difficulty.text = Enum.GetName(typeof(Difficulty), GameSettings.difficulty);
            randomScale.text = GameSettings.RandomScale.ToString();
            randomRotation.text = GameSettings.RandomRotation.ToString();
            enableCrosshair.text = GameSettings.EnableCrosshair.ToString();
            enablePhantoms.text = GameSettings.EnablePhantoms.ToString();
            shapeCount.text = GameSettings.shapePool.Count.ToString();
        }
        #endregion
    }
}
