using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Stealth.AI
{ 
    [RequireComponent(typeof(GuardController))]
    public class GuardPatrol : MonoBehaviour
    {
        [SerializeField] private GuardController _guardController;

        [Header("Waypoints")]
        private Transform[] _waypoints;
        [SerializeField] private float _patrolSpeed = 2f;

        [Header("Debug")]
        [SerializeField] private int _currentWaypointIndex = 0;
        private NavMeshAgent _navMeshAgent;

        [SerializeField] private bool _canPatrol;
        public bool canPatrol
        {
            get => _canPatrol;
            private set
            {
                _canPatrol = value;
                if (canPatrol)
                    StartPatrol();
                else
                    StopPatrol();
            }
        }

        void Start()
        {
            _navMeshAgent = _guardController.GetNavMeshAgent;
            _waypoints = _guardController.Waypoints;

            _navMeshAgent.speed = _patrolSpeed;
        }

        private void Update()
        {
            patrolling();
        }

        public void StartPatrol()
        {
            if (_waypoints.Length == 0)
            {
                Debug.LogWarning($"No waypoints assigned to patrol");
                return;
            }
            Debug.Log("Start patrol");

            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(_waypoints[_currentWaypointIndex].position);
        }

        private void StopPatrol()
        {
            Debug.Log("Stop patrol");
            _navMeshAgent.isStopped = true;
        }

        private void patrolling()
        {
            if (!_canPatrol)
                return;

            if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            {
                //! Implement Idle

                _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
                _navMeshAgent.SetDestination(_waypoints[_currentWaypointIndex].position);
            }
        }

        public void SetPatrol(bool setPatrol)
        {
            canPatrol = setPatrol;
        }
    }
}