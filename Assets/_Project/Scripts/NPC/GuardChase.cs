using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Stealth.AI
{
    [RequireComponent(typeof(GuardController))]
    public class GuardChase : MonoBehaviour
    {
        [SerializeField] private GuardController _guardController;
        private NavMeshAgent _navMeshAgent;

        [Header("Debug")]
        [SerializeField] private bool _isChasing;
        [SerializeField] private Vector3 _playerLastPosition;
        [SerializeField] private Vector3 _playerPosition;

        [SerializeField] private bool _caughtPlayer;

        public UnityEvent OnPlayerMissing;

        private void Start()
        {
            _navMeshAgent = _guardController.GetNavMeshAgent;
        }

        public void SetChase(bool setChase)
        {
            _isChasing = setChase;
        }

        public void Chasing()
        {
            _playerLastPosition = Vector3.zero;

            //! Move to player position
            if (!_caughtPlayer)
            {
                _navMeshAgent.isStopped = false;
                _navMeshAgent.speed = _guardController.RunSpeed;
                _navMeshAgent.SetDestination(_playerPosition);
            }

            if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            {
                //! If player distance is more than x distance
                if (Vector3.Distance(transform.position, _guardController.GetPlayer) >= 6f)
                {
                    _navMeshAgent.isStopped = true;

                    //! Do Idle
                    _guardController.SetBehaviour(GuardController.EGuardState.Idle);

                    //! Invoke player missing
                    OnPlayerMissing.Invoke();
                }
                //! Continue Chase last player position
                else
                {
                    _navMeshAgent.SetDestination(_guardController.GetPlayer);
                }
            }
        }
    }
}