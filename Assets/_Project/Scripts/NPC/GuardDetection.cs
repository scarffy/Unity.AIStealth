using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Stealth.AI
{
    public class GuardDetection : MonoBehaviour
    {
        public float _viewRadius = 15f;
        [Range(0, 360)]
        public float _viewAngle = 90f;

        [Header("Layer Mask")]
        [SerializeField] private LayerMask _playerMask;
        [SerializeField] private LayerMask _obstacleMask;

        [Header("Player in range")]
        public bool _playerInRange = false;
        public Transform _playerPosition;

        private Collider[] colliders;

        public UnityEvent OnPlayerDetected;
        public UnityEvent OnPlayerMissing;

        private void Update()
        {
            DetectionView();
        }

        /// <summary>
        /// Check if player is in view
        /// </summary>
        private void DetectionView()
        {
            colliders = Physics.OverlapSphere(transform.position, _viewRadius, _playerMask);

            bool playerDetected = false; // Flag to track if any player is detected

            for (int i = 0; i < colliders.Length; i++)
            {
                Transform player = colliders[i].transform;
                Vector3 playerDirection = (player.position - transform.position).normalized;

                float angle = Vector3.Angle(transform.forward, playerDirection);

                //! Detect if player is in front of guard
                if (angle < _viewAngle / 2)
                {
                    float playerDistance = Vector3.Distance(transform.position, player.position);
                    if (!Physics.Raycast(transform.position, playerDirection, playerDistance, _obstacleMask))
                    {
                        //! player is in range
                        //! stop patrol
                        //! Invoke PlayerDetection
                        //! Cache playerPosition
                        _playerInRange = true;
                        OnPlayerDetected.Invoke();
                        playerDetected = true; // Set the flag to true if player is detected
                        _playerPosition = player;
                    }
                }
            }

            //! if no player is detected, invoke OnPlayerMissing event
            if (!playerDetected)
            {
                _playerInRange = false;
                OnPlayerMissing.Invoke();
            }
        }
    }
}