using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Stealth.AI
{
    public class GuardIdle : MonoBehaviour
    {
        private const float _idleTime = 2.5f;
        [SerializeField] private float _idleTimeCount;
        [SerializeField] private bool _isIdle = false;

        [Header("Unity Events")]
        public UnityEvent OnTimeFinished;

        private void Update()
        {
            if (_isIdle)
            {
                if (_idleTimeCount > 0.0f)
                {
                    _idleTimeCount -= Time.deltaTime;
                }
                else
                {
                    // Reset idle state and time count
                    _isIdle = false;
                    _idleTimeCount = _idleTime;
                    OnTimeFinished.Invoke();
                }
            }
        }

        public void SetIdle()
        {
            _isIdle = true;
            _idleTimeCount = _idleTime;
        }

        public void StopIdle()
        {
            _isIdle = false;
        }

        public bool IsIdling => _isIdle;
    }
}