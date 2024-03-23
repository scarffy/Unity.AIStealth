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

        private bool _isChasing;
        private Vector3 _playerPosition;

        [SerializeField] private bool _caughtPlayer;

        [Header("Unity Events")]
        [Space]
        public UnityEvent OnPlayerMissing;

        private void Awake()
        {
            if (_guardController != null)
                _navMeshAgent = _guardController.GetNavMeshAgent;
        }

        private void Update()
        {
            if (_isChasing)
                Chasing();
        }

        public void SetChase(bool setChase)
        {
            _isChasing = setChase;

            if (!_isChasing)
            {
                _navMeshAgent.isStopped = true;
                // _guardController.GetGuardIdle.OnTimeFinished.AddListener(OnIdleFinished);
            }
            else
            {
                _navMeshAgent.isStopped = false;
                _navMeshAgent.speed = _guardController.RunSpeed;
                // _guardController.GetGuardIdle.OnTimeFinished.RemoveListener(OnIdleFinished);
            }
        }

        public void Chasing()
        {
            //! Move to player position
            if (!_caughtPlayer && _isChasing)
            {
                _navMeshAgent.SetDestination(_guardController.GetPlayer);
            }

            if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance + 1)
            {
                //! If player distance is more than x distance
                if (Vector3.Distance(transform.position, _guardController.GetPlayer) >= 5f)
                {
                    //! Stop chasing player
                    SetChase(false);

                    //! Idle for a while

                    //! Invoke player missing
                    OnPlayerMissing.Invoke();
                }
                //! Continue Chase last player position
                else
                {
                    if (Vector3.Distance(transform.position, _guardController.GetPlayer) >= 2.5f)
                        SetChase(false);
                    // _navMeshAgent.SetDestination(_guardController.GetPlayer);
                }
            }
        }

        private void OnIdleFinished()
        {

        }
    }
}