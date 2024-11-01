using System.Collections.Generic;
using UnityEngine;

namespace Omnis.Playdough
{
    [RequireComponent(typeof(LineRenderer))]
    public class Playdough : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private PlaydoughShape shape;
        #endregion

        #region Fields
        private LineRenderer lineRenderer;
        private List<Vector3> originalCopy;
        private List<Vector3> vertices;
        private float scale;
        private float rotation;
        private float aspectRatio;
        #endregion

        #region Interfaces
        public PlaydoughShape Shape
        {
            get => shape;
            set
            {
                shape = value;
                originalCopy = new(ShapeStore.GetVertices(shape));
                vertices = new(originalCopy);
                lineRenderer.positionCount = vertices.Count;
                UpdateLinePositions();
            }
        }
        public float Scale
        {
            get => scale;
            set
            {
                scale = Mathf.Clamp(value, 0f, 3f);
                CalculateVertices();
            }
        }
        public float Rotation
        {
            get => rotation;
            set
            {
                rotation = value;
                CalculateVertices();
            }
        }
        public float AspectRatio
        {
            get => aspectRatio;
            set
            {
                aspectRatio = Mathf.Clamp(value, -1f, 1f);
                CalculateVertices();
            }
        }
        public Color Color
        {
            get => lineRenderer.startColor;
            set => lineRenderer.startColor = lineRenderer.endColor = value;
        }

        public void CopyTo(Playdough copy)
        {
            copy.Shape = Shape;
            copy.Scale = Scale;
            copy.Rotation = Rotation;
            copy.AspectRatio = AspectRatio;
            copy.Color = Color;
        }
        #endregion

        #region Functions
        private void CalculateVertices()
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                var scaled = scale * originalCopy[i];
                var rotated = Quaternion.AngleAxis(rotation, Vector3.forward) * scaled;
                vertices[i] = new(rotated.x * Mathf.Exp(AspectRatio), rotated.y * Mathf.Exp(-AspectRatio), rotated.z);
            }
            UpdateLinePositions();
        }

        private void UpdateLinePositions() => lineRenderer.SetPositions(vertices.ToArray());
        #endregion

        #region Unity Methods
        private void Awake()
        {
            scale = 1f;
            rotation = 0f;
            aspectRatio = 0f;
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.loop = true;
        }
        #endregion
    }
}
