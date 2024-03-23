using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stealth.AI
{
    public class GuardVisual : MonoBehaviour
    {
        [SerializeField] private GuardDetection _guardDetection;

        [Header("Settings")]
        [SerializeField] private Material _viewConeMaterial;
        private float _viewRadius;
        private float _viewAngle;
        [SerializeField] private LayerMask _viewLayerMask;
        [SerializeField] private int _viewConeResolution = 120;
        private Mesh _viewConeMesh;
        private MeshFilter _meshFilter;

        void Start()
        {
            gameObject.AddComponent<MeshRenderer>().material = _viewConeMaterial;
            _meshFilter = gameObject.AddComponent<MeshFilter>();
            _viewConeMesh = new Mesh();
            _viewAngle *= Mathf.Deg2Rad;

            _viewRadius = _guardDetection._viewRadius;
            _viewAngle = _guardDetection._viewAngle;
        }

        void Update()
        {
            CreateVisionCone();
        }

        void CreateVisionCone()
        {
            int[] triangles = new int[(_viewConeResolution - 1) * 3];
            Vector3[] vertices = new Vector3[_viewConeResolution + 1];
            vertices[0] = Vector3.zero;
            float currentAngle = -_viewAngle / 2;
            float angleIncrement = _viewAngle / (_viewConeResolution - 1);
            float sine;
            float cosine;

            for (int i = 0; i < _viewConeResolution; i++)
            {
                sine = Mathf.Sin(currentAngle);
                cosine = Mathf.Cos(currentAngle);
                Vector3 raycastDirection = (transform.forward * cosine) + (transform.right * sine);
                Vector3 vertorForward = (Vector3.forward * cosine) + (Vector3.right * sine);
                if (Physics.Raycast(transform.position, raycastDirection, out RaycastHit hit, _viewRadius, _viewLayerMask))
                {
                    vertices[i + 1] = vertorForward * hit.distance;
                }
                else
                {
                    vertices[i + 1] = vertorForward * _viewRadius;
                }


                currentAngle += angleIncrement;
            }
            for (int i = 0, j = 0; i < triangles.Length; i += 3, j++)
            {
                triangles[i] = 0;
                triangles[i + 1] = j + 1;
                triangles[i + 2] = j + 2;
            }

            _viewConeMesh.Clear();
            _viewConeMesh.vertices = vertices;
            _viewConeMesh.triangles = triangles;
            _meshFilter.mesh = _viewConeMesh;
        }


    }
}