using System.Collections.Generic;
using UnityEngine;

namespace Omnis.Playdough
{
    [CreateAssetMenu(menuName = "Omnis/Game Settings")]
    public class GameSettings : ScriptableObject
    {
        public static bool RandomScale { get; set; }
        public static bool RandomRotation { get; set; }
        public static bool EnableCrosshair { get; set; }
        public static bool EnablePhantoms { get; set; }

        public static Difficulty difficulty = Difficulty.Normal;

        public static List<PlaydoughShape> shapePool = new();
        public static PlaydoughShape GetRandomShapeFromPool() => shapePool[Random.Range(0, shapePool.Count)];
    }

    public enum Difficulty
    {
        Default,
        Easy,
        Normal,
        Hard,
    }
}
