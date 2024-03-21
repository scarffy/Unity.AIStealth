using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEditorInternal;

namespace Stealth.AI
{
    /// <summary>
    /// Holds all relevant data related to Guard
    /// </summary>
    [DisallowMultipleComponent]
    public class GuardController : MonoBehaviour
    {
        [SerializeField] private GuardIdle _guardIdle;

        [Serializable]
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
        [SerializeField] private float _walkSpeed = 2f;
        [SerializeField] private float _runSpeed = 4f;

        [Header("Waypoints")]
        [SerializeField] private Transform[] _waypoints;

        [Header("Guard State")]
        public UnityEvent OnIdleState;
        public UnityEvent OnPatrolState;
        public UnityEvent OnInvestigateSound;
        public UnityEvent OnChaseState;

        [Header("Reference")]
        [SerializeField] private Vector3 _player;

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

                case EGuardState.Detect:
                    OnInvestigateSound.Invoke();
                    break;

                case EGuardState.Chase:
                    OnChaseState.Invoke();
                    break;
            }
            Debug.Log(GuardState);
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