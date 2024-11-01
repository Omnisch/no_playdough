namespace Omnis.Playdough
{
    public class StatsPanelTotal : StatsPanel
    {
        #region Unity methods
        private void OnEnable()
        {
            perfect.text = Statistics.totalPerfectCount.ToString();
            qualified.text = Statistics.totalQualifiedCount.ToString();
            unqualified.text = Statistics.totalUnqualifiedCount.ToString();
            tooMuch.text = Statistics.totalTooMuchCount.ToString();
            miss.text = Statistics.totalMissCount.ToString();
            total.text = Statistics.TotalCount.ToString();
            accuracy.text = Statistics.TotalAccuracy.ToString("F0") + "%";
            avgTime.text = Statistics.totalAverageTime.ToString("F2");
            playedTime.text = Statistics.totalPlayedTime.ToString("F2");
        }
        #endregion
    }
}
