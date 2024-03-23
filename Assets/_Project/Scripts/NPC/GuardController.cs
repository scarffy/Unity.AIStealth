using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Stealth.AI
{
    /// <summary>
    /// Holds all relevant data related to Guard
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(NavMeshAgent))]
    public class GuardController : MonoBehaviour
    {
        [SerializeField] private GuardIdle _guardIdle;

        [Serializable]
        public enum EGuardState
        {
            Idle,
            Patrol,
            Investigate,
            Chase
        }

        public EGuardState GuardState = EGuardState.Idle;

        [Header("Navigation")]
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private float _walkSpeed = 2f;
        [SerializeField] private float _runSpeed = 4f;

        [Header("Patrol Waypoints")]
        [SerializeField] private Transform[] _waypoints;

        [Header("Guard Event State")]
        [Space]
        public UnityEvent OnIdleState;
        public UnityEvent OnPatrolState;
        public UnityEvent OnInvestigateSound;
        public UnityEvent OnChaseState;

        [Header("Reference")]
        private Vector3 _player;

        private void Start()
        {
            UpdateBehaviour();
        }

        public void SetBehaviour(int state)
        {
            GuardState = (EGuardState)state;
            UpdateBehaviour();
        }

        public void SetBehaviour(EGuardState state)
        {
            GuardState = state;
            UpdateBehaviour();
        }

        private void UpdateBehaviour()
        {
            //! Update behaviour state here
            switch (GuardState)
            {
                case EGuardState.Idle:
                    OnIdleState.Invoke();
                    break;

                case EGuardState.Patrol:
                    OnPatrolState.Invoke();
                    break;

                case EGuardState.Investigate:
                    OnInvestigateSound.Invoke();
                    break;

                case EGuardState.Chase:
                    OnChaseState.Invoke();
                    break;
            }
        }

        public void SetPlayerLastPosition(Vector3 player)
        {
            _player = player;
        }

        public GuardIdle GetGuardIdle => _guardIdle;

        public NavMeshAgent GetNavMeshAgent => _navMeshAgent;
        public Transform[] Waypoints => _waypoints;
        public float WalkSpeed => _walkSpeed;
        public float RunSpeed => _runSpeed;

        public Vector3 GetPlayer => _player;

    }
}