using UnityEngine;
using UnityEngine.AI;

namespace Stealth.AI
{
    [RequireComponent(typeof(GuardController))]
    public class GuardPatrol : MonoBehaviour
    {
        [SerializeField] private GuardController _guardController;
        [Header("Guard Waypoint")]
        private Transform[] _waypoints;
        private NavMeshAgent _navMeshAgent;
        private int _currentWaypointIndex = 0;
        private bool _canPatrol = false;
        private bool _isIdle = false;

        [Header("Settings")]
        [SerializeField] private bool _canDoIdle = false;

        private void Awake()
        {
            Initialize();
        }

        private void Update()
        {
            Patrolling();
        }

        private void Initialize()
        {
            if (_guardController != null)
            {
                _navMeshAgent = _guardController.GetNavMeshAgent;
                _waypoints = _guardController.Waypoints;
                _navMeshAgent.speed = _guardController.WalkSpeed;
            }
            else
            {
                Debug.Log($"[AI] Guard controller is null");
            }
        }

        private void StartPatrol()
        {
            if (_waypoints.Length == 0 || !_canPatrol)
                return;

            _navMeshAgent.speed = _guardController.WalkSpeed;
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(_waypoints[_currentWaypointIndex].position);

            if (!_isIdle && _guardController != null)
            {
                _guardController.GetGuardIdle.OnTimeFinished.AddListener(OnIdleFinished);
            }
        }

        private void StopPatrol()
        {
            if (_guardController != null && _isIdle)
            {
                _guardController.GetGuardIdle.OnTimeFinished.RemoveListener(OnIdleFinished);
                _isIdle = false;
            }
            _navMeshAgent.isStopped = true;
        }

        private void Patrolling()
        {
            if (!_canPatrol || _canDoIdle && _isIdle && _guardController != null)
                return;

            if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            {
                if (_canDoIdle && _guardController != null)
                {
                    _guardController.GetGuardIdle.SetIdle();
                    _navMeshAgent.isStopped = true;
                }

                _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
                _navMeshAgent.SetDestination(_waypoints[_currentWaypointIndex].position);
            }
        }

        public void SetPatrol(bool setPatrol)
        {
            _canPatrol = setPatrol;

            if (_canPatrol)
                StartPatrol();
            else
                StopPatrol();
        }

        private void OnIdleFinished()
        {
            _navMeshAgent.isStopped = false;
        }
    }
}