using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Stealth.AI
{
    [DisallowMultipleComponent]
    public class GuardController : MonoBehaviour
    {
        [SerializeField] private GuardIdle _guardIdle;

        public enum EGuardState
        {
            Idle,
            Patrol,
            Detect,
            Chase
        }

        public EGuardState GuardState = EGuardState.Idle;

        [Header("Navigation")]
        [SerializeField] private NavMeshAgent _navMeshAgent;

        [Header("Waypoints")]
        [SerializeField] private Transform[] _waypoints;

        [Header("Guard State")]
        public UnityEvent OnIdleState;
        public UnityEvent OnPatrolState;
        public UnityEvent OnInvestigateSound;
        public UnityEvent OnChaseState;

        private void Start()
        {
            UpdateBehaviour();
        }

        /// <summary>
        /// Set guard behaviour state
        /// </summary>
        /// <param name="state">Guard state enum</param>
        public void SetBehaviour(int state)
        {
            GuardState = (EGuardState)state;
            UpdateBehaviour();
        }

        private void UpdateBehaviour()
        {
            //! Update behaviour state here
            //! Maybe use coroutine instead
            switch(GuardState)
            {
                case EGuardState.Idle:
                    Debug.Log("Invoke Idle state");
                    OnIdleState.Invoke();
                    break;

                case EGuardState.Patrol:
                    OnPatrolState.Invoke();
                    break;

                case EGuardState.Detect:
                    OnInvestigateSound.Invoke();
                    break;

                case EGuardState.Chase:
                    OnChaseState.Invoke();
                    break;
            }
        }


        public NavMeshAgent GetNavMeshAgent => _navMeshAgent;
        public Transform[] Waypoints => _waypoints;

        public GuardIdle GetGuardIdle => _guardIdle;
    }
}