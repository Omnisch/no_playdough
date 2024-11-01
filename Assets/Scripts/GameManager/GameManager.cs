using System.Collections.Generic;
using UnityEngine;

namespace Omnis.Playdough
{
    public partial class GameManager : MonoBehaviour
    {
        #region Serialized Fields
        public SoundEffectSettings SeSettings;
        public List<PlaydoughShape> shapePool;
        public float bonusMult;
        #endregion

        #region Interfaces
        public bool RandomScale { get; set; }
        public bool RandomRotation { get; set; }
        public bool EnableCrosshair { get; set; }
        public PlaydoughShape GetRandomShapeFromPool()
            => shapePool[Random.Range(0, shapePool.Count)];
        #endregion
    }
}
