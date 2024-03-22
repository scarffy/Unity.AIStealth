using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Stealth.AI
{
    public class GuardInvestigate : MonoBehaviour
    {
        [SerializeField] private GuardController _guardController;

        private NavMeshAgent _navMeshMeshAgent;

        private void Start()
        {
            _navMeshMeshAgent = _guardController.GetNavMeshAgent;
        }

        private bool _isInvestigating;
        public bool IsInvestigating
        {
            get => _isInvestigating;
            set
            {
                _isInvestigating = value;
                Investigating();
            }
        }

        private void Investigating()
        {
            _navMeshMeshAgent.isStopped = false;
            _navMeshMeshAgent.speed = _guardController.WalkSpeed;
            _navMeshMeshAgent.SetDestination(_guardController.GetPlayer);
        }
    }
}