using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.AI;

namespace Stealth.AI
{
    [RequireComponent(typeof(GuardController))]
    public class GuardPatrol : MonoBehaviour
    {
        [SerializeField] private GuardController _guardController;

        [Header("Waypoints")]
        [SerializeField] private Transform[] _waypoints;

        [Header("Debug")]
        [SerializeField] private int _currentWaypointIndex = 0;
        private NavMeshAgent _navMeshAgent;

        private bool _isIdle;

        [SerializeField] private bool _canPatrol;
        public bool canPatrol
        {
            get => _canPatrol;
            private set
            {
                Debug.Log("Setting Patrol State");
                _canPatrol = value;
                if (canPatrol)
                {
                    Debug.Log("Can Patrol");
                    StartPatrol();
                }
                else
                    StopPatrol();
            }
        }

        private void Awake()
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
                Debug.LogError("No waypoints are set");
                return;
            }

            _navMeshAgent.speed = _guardController.WalkSpeed;
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(_waypoints[_currentWaypointIndex].position);

            _guardController.GetGuardIdle.OnTimeFinished.AddListener(OnIdleFinished);
            Debug.Log("Start Patrol");
        }

        private void StopPatrol()
        {
            _guardController.GetGuardIdle.OnTimeFinished.RemoveListener(OnIdleFinished);
            _navMeshAgent.isStopped = true;
            Debug.Log("Stop Patrol");
        }

        private void patrolling()
        {
            if (!_canPatrol)
                return;

            if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            {
                _isIdle = true;
                _guardController.GetGuardIdle.SetIdle();
                _navMeshAgent.isStopped = true;

                _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
                _navMeshAgent.SetDestination(_waypoints[_currentWaypointIndex].position);
            }
        }

        public void SetPatrol(bool setPatrol)
        {
            canPatrol = setPatrol;
        }

        private void OnIdleFinished()
        {
            _isIdle = false;
            _navMeshAgent.isStopped = false;
        }
    }
}