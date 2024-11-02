using System.Collections.Generic;
using UnityEngine;

namespace Omnis.Playdough
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(LineRenderer))]
    public partial class Playdough : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private PlaydoughShape shape;
        [SerializeField] private float scale;
        [SerializeField] private float rotation;
        [SerializeField] private float aspectRatio;
        #endregion

        #region Fields
        private LineRenderer lineRenderer;
        private List<Vector3> originalCopy;
        private List<Vector3> vertices;
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
        [ContextMenu("Update Shape")]
        private void InitFromHierarchy()
        {
            Shape = shape;
            Scale = scale;
            Rotation = rotation;
            AspectRatio = aspectRatio;
        }

        private void CalculateVertices()
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                var scaled = scale * originalCopy[i];
                var rotated = Quaternion.AngleAxis(rotation, Vector3.forward) * scaled;
                var stretched = new Vector3(rotated.x * Mathf.Exp(AspectRatio), rotated.y * Mathf.Exp(-AspectRatio), rotated.z);
                vertices[i] = stretched;
            }
            UpdateLinePositions();
        }

        private void UpdateLinePositions() => lineRenderer.SetPositions(vertices.ToArray());
        #endregion

        #region Unity Methods
        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.loop = true;
            InitFromHierarchy();
        }
        #endregion
    }
}
