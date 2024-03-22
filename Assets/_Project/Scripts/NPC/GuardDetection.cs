using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Stealth.AI
{
    [RequireComponent(typeof(GuardController))]
    public class GuardDetection : MonoBehaviour
    {
        [SerializeField] private GuardController _guardController;

        [Header("Settings")]
        public float _viewRadius = 15f;
        [Range(0, 360)]
        public float _viewAngle = 90f;

        [Header("Layer Mask")]
        [SerializeField] private LayerMask _playerMask;
        [SerializeField] private LayerMask _obstacleMask;

        [Header("Player range detected")]
        public bool _playerInRange = false;
        private bool _isPlayerMissing = false;

        [Header("Player position")]
        public Transform _playerPosition;
        public Vector3 _playerLastPosition;

        [SerializeField] private Collider[] _colliders;

        public UnityEvent OnPlayerDetected;
        public UnityEvent OnPlayerMissing;

        private void Update()
        {
            DetectionView();
        }

        /// <summary>
        /// Check surrounding
        /// </summary>
        private void DetectionView()
        {
            _colliders = Physics.OverlapSphere(transform.position, _viewRadius, _playerMask);

            for (int i = 0; i < _colliders.Length; i++)
            {
                Transform player = _colliders[i].transform;
                Vector3 playerDirection = (player.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, playerDirection) < _viewAngle / 2)
                {
                    float playerDistance = Vector3.Distance(transform.position, player.position);

                    //! If nothing is blocking
                    if (!Physics.Raycast(transform.position, playerDirection, playerDistance, _obstacleMask))
                    {
                        _playerInRange = true;
                        _playerPosition = player;
                        _playerLastPosition = player.position;

                        _guardController.SetPlayerLastPosition(_playerLastPosition);

                        PlayerFound();
                    }
                    else
                    {
                        PlayerMissing();
                    }
                }

                if (Vector3.Distance(transform.position, player.position) > _viewRadius)
                {
                    PlayerMissing();
                }
            }
        }

        private void PlayerMissing()
        {
            if (_isPlayerMissing)
                return;

            _isPlayerMissing = true;
            _playerInRange = false;
            _guardController.SetPlayerLastPosition(_playerLastPosition);
            _guardController.SetBehaviour(GuardController.EGuardState.Patrol);
            OnPlayerMissing.Invoke();
        }

        private void PlayerFound()
        {
            if (!_isPlayerMissing)
                return;

            _guardController.SetBehaviour(GuardController.EGuardState.Chase);
            OnPlayerDetected.Invoke();

            _isPlayerMissing = false;
        }
    }
}