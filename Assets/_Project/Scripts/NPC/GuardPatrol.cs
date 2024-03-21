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

        private void Start()
        {
            _navMeshAgent = _guardController.GetNavMeshAgent;
            _waypoints = _guardController.Waypoints;

            _navMeshAgent.speed = _guardController.WalkSpeed;
        }

        private void Update()
        {
            patrolling();
        }

        private void StartPatrol()
        {
            if (_waypoints.Length == 0)
            {
                Debug.LogWarning($"No waypoints assigned to patrol");
                return;
            }
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(_waypoints[_currentWaypointIndex].position);
        }

        private void StopPatrol()
        {
            _navMeshAgent.isStopped = true;
        }

        private void patrolling()
        {
            if (!_canPatrol)
                return;

            if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            {
                //! Implement Idle then move to next waypoint

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