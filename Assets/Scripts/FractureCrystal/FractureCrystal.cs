using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

namespace Project.Scripts.FractureCrystal
{
    public class FractureCrystal : MonoBehaviour
    {
        // Extrusion
        [SerializeField, Range(0f, 2f)] private float extrusionFactor = 1f;
        [SerializeField, Range(1f, 20f)] private float voronoiDensity = 3f;
        [SerializeField, Range(0f, 20f)] private float voronoiOffset = 3f;

        // Fracture
        [SerializeField] private int chunkCount = 500;
        [SerializeField] private Material insideMaterial;
        [SerializeField] private Material outsideMaterial;

        [SerializeField] private Transform chunkPivotParent;
        [SerializeField] private Transform vanishingTrigger;

        private Mesh mesh = null;

        private List<Vector3> vertexPositionsCache = new List<Vector3>();
        private List<Vector3> vertexNormalsCache = new List<Vector3>();

        private System.Random rng = new System.Random();

        void Start()
        {
            mesh = GetComponent<MeshFilter>().mesh;
            for (int i = 0; i < mesh.vertexCount; i++)
            {
                vertexPositionsCache.Add(mesh.vertices[i]);
                vertexNormalsCache.Add(mesh.normals[i]);
            }

            Extrusion();
            Destruction();

            vanishingTrigger.DOLocalMoveY(-3f, 12f);
        }

        public void Extrusion()
        {
            Vector3 localScale = transform.localScale;
            mesh.SetVertices(
                vertexPositionsCache.Select(
                    (pos, i) =>
                        pos + MathUtil.Divide3D(vertexNormalsCache[i], localScale) *
                        MathUtil.Voronoi(MathUtil.Multiply3D(pos, localScale), voronoiDensity, voronoiOffset) * extrusionFactor
                ).ToArray()
            );
        }

        public void Destruction()
        {
            var seed = rng.Next();
            Fracture.FractureGameObject(
                gameObject,
                chunkPivotParent,
                seed,
                chunkCount,
                insideMaterial,
                outsideMaterial
            );
            gameObject.SetActive(false);
        }

        public void StartVanishing(Collider other)
        {
            Transform parent = other.transform.parent;
            parent.DOScale(Vector3.zero, 1f);
            parent.localRotation = Quaternion.Euler(
                new Vector3(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f))
            );
            parent.DOLocalMoveY(2f, 3f);
        }
    }
}
