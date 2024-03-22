using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stealth.AI
{
    public class PlayerCreateSound : MonoBehaviour
    {
        [Header("Settings")]
        public float _viewRadius = 15f;

        [Range(0, 360)]
        public float _viewAngle = 360f;

        [Header("Layer Mask")]
        [SerializeField] private LayerMask _guardMask;
        [SerializeField] private LayerMask _obstacleMask;

        [Header("Guard Position")]
        [SerializeField] private Collider[] _guardColliders;
        public Transform[] _guardList;
        public bool _guardInRange;

        private void Update()
        {
            EmitSound();
        }

        void EmitSound()
        {
            _guardColliders = Physics.OverlapSphere(transform.position, _viewRadius, _guardMask);
            Transform[] transforms = new Transform[_guardColliders.Length];

            for (int i = 0; i < _guardColliders.Length; i++)
            {
                transforms[i] = _guardColliders[i].transform;

                Vector3 guardDirection = (_guardColliders[i].transform.position - transform.position).normalized;

                //! Checking angle                
                if (Vector3.Angle(transform.forward, guardDirection) < _viewAngle / 2)
                {
                    float guardDistance = Vector3.Distance(transform.position, _guardColliders[i].transform.position);
                    //! Check if nothing is blocking
                    if (!Physics.Raycast(transform.position, guardDirection, guardDistance, _obstacleMask))
                    {
                        _guardInRange = true;
                        //! Set Player positions

                        Debug.DrawLine(transform.position, _guardColliders[i].transform.position, Color.green);
                        if (transforms[i].TryGetComponent<GuardController>(out GuardController controller))
                        {
                            controller.SetBehaviour(GuardController.EGuardState.Investigate);
                            controller.SetPlayerLastPosition(transform.position);
                            Debug.Log("Found player Guard Component");
                        }
                        else
                        {
                            Debug.Log("No player Guard Component");
                        }
                    }
                    else
                    {
                        Debug.DrawLine(transform.position, _guardColliders[i].transform.position, Color.red);
                    }
                }
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, _viewRadius);
        }

        private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
        {
            angleInDegrees += eulerY;
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
    }
}