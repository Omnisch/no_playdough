using System.Collections.Generic;
using UnityEngine;

namespace Omnis.Playdough
{
    public enum PlaydoughShape
    {
        // Circle //
        Circle,
        // Regular polygons //
        Triangle,
        Rectangle,
        Pentagon,
        Hexagon,
        // Non-convex polygons //
        FourPointedStar,
        FivePointedStar,
        SixPointedStar,
    };

    public class ShapeStore : ScriptableObject
    {
        public static readonly Dictionary<PlaydoughShape, Vector3[]> verticesList = new()
        {
            {
                PlaydoughShape.Circle, new Vector3[]
                {
                    new(Mathf.Cos(0f * Mathf.PI / 8f), Mathf.Sin(0f * Mathf.PI / 8f)),
                    new(Mathf.Cos(1f * Mathf.PI / 8f), Mathf.Sin(1f * Mathf.PI / 8f)),
                    new(Mathf.Cos(2f * Mathf.PI / 8f), Mathf.Sin(2f * Mathf.PI / 8f)),
                    new(Mathf.Cos(3f * Mathf.PI / 8f), Mathf.Sin(3f * Mathf.PI / 8f)),
                    new(Mathf.Cos(4f * Mathf.PI / 8f), Mathf.Sin(4f * Mathf.PI / 8f)),
                    new(Mathf.Cos(5f * Mathf.PI / 8f), Mathf.Sin(5f * Mathf.PI / 8f)),
                    new(Mathf.Cos(6f * Mathf.PI / 8f), Mathf.Sin(6f * Mathf.PI / 8f)),
                    new(Mathf.Cos(7f * Mathf.PI / 8f), Mathf.Sin(7f * Mathf.PI / 8f)),
                    new(Mathf.Cos(8f * Mathf.PI / 8f), Mathf.Sin(8f * Mathf.PI / 8f)),
                    new(Mathf.Cos(9f * Mathf.PI / 8f), Mathf.Sin(9f * Mathf.PI / 8f)),
                    new(Mathf.Cos(10f * Mathf.PI / 8f), Mathf.Sin(10f * Mathf.PI / 8f)),
                    new(Mathf.Cos(11f * Mathf.PI / 8f), Mathf.Sin(11f * Mathf.PI / 8f)),
                    new(Mathf.Cos(12f * Mathf.PI / 8f), Mathf.Sin(12f * Mathf.PI / 8f)),
                    new(Mathf.Cos(13f * Mathf.PI / 8f), Mathf.Sin(13f * Mathf.PI / 8f)),
                    new(Mathf.Cos(14f * Mathf.PI / 8f), Mathf.Sin(14f * Mathf.PI / 8f)),
                    new(Mathf.Cos(15f * Mathf.PI / 8f), Mathf.Sin(15f * Mathf.PI / 8f)),
                }
            },
            {
                PlaydoughShape.Triangle, new Vector3[]
                {
                    new(-1f, -0.5774f, 0f),
                    new(1f, -0.5774f, 0f),
                    new(0f, 1.1547f, 0f),
                }
            },
            {
                PlaydoughShape.Rectangle, new Vector3[]
                {
                    VectorTweaker.lb,
                    VectorTweaker.lt,
                    VectorTweaker.rt,
                    VectorTweaker.rb,
                }
            },
        };

        public static Vector3[] GetVertices(PlaydoughShape shape) => verticesList[shape];
    }
}
