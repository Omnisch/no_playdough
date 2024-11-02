using UnityEngine;

namespace Omnis.Playdough
{
    public abstract class Statistics : ScriptableObject
    {
        #region Serialized fields
        [Header("One Round")]
        public static int perfectCount;
        public static int qualifiedCount;
        public static int unqualifiedCount;
        public static int tooMuchCount;
        public static int missCount;
        public static float averageTime;
        public static float playedTime;

        [Header("Total")]
        public static int totalPerfectCount;
        public static int totalQualifiedCount;
        public static int totalUnqualifiedCount;
        public static int totalTooMuchCount;
        public static int totalMissCount;
        public static float totalAverageTime;
        public static float totalPlayedTime;
        public static int totalRoundCount;
        #endregion

        #region Properties
        // One round.
        public static int FinishedCount => perfectCount + qualifiedCount;
        public static int UnfinishedCount => unqualifiedCount + tooMuchCount + missCount;
        public static int OneRoundCount => FinishedCount + UnfinishedCount;
        public static float Accuracy => 100f * FinishedCount / (float)OneRoundCount;
        // Total.
        public static int TotalFinishedCount => totalPerfectCount + totalQualifiedCount;
        public static int TotalUnfinishedCount =>  totalUnqualifiedCount + totalTooMuchCount + totalMissCount;
        public static int TotalCount => TotalFinishedCount + TotalUnfinishedCount;
        public static float TotalAccuracy => 100f * TotalFinishedCount / (float)TotalCount;
        #endregion

        #region Functions
        public static void ResetOneRoundStatistics()
        {
            perfectCount = 0;
            qualifiedCount = 0;
            unqualifiedCount = 0;
            tooMuchCount = 0;
            missCount = 0;
            averageTime = 0f;
            playedTime = 0f;
        }

        public static void AddOneRoundToTotal()
        {
            totalRoundCount++;
            totalPerfectCount += perfectCount;
            totalQualifiedCount += qualifiedCount;
            totalUnqualifiedCount += unqualifiedCount;
            totalTooMuchCount += tooMuchCount;
            totalMissCount += missCount;
            totalAverageTime = (totalAverageTime * (totalRoundCount - 1) + averageTime) / (float)totalRoundCount;
            totalPlayedTime += playedTime;
        }
        #endregion
    }
}
