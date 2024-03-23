using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stealth.AI
{
    public class GuardVisualCue : MonoBehaviour
    {
        [SerializeField] private GuardController _guardController;

        [SerializeField] private GameObject[] _visualCueList;

        private void Update()
        {
            Vector3 directionToCamera = Camera.main.transform.position - transform.position;

            // Project the direction onto the XZ plane (Y axis)
            directionToCamera.y = 0f;

            // Rotate the object to face the camera
            transform.rotation = Quaternion.LookRotation(directionToCamera);
        }

        public void SetVisualCue(int behaviour)
        {
            SetVisualCue((GuardController.EGuardState)behaviour);
        }

        public void SetVisualCue(GuardController.EGuardState behaviour)
        {
            foreach (var item in _visualCueList)
                item.SetActive(false);


            switch (behaviour)
            {
                case GuardController.EGuardState.Idle:
                    _visualCueList[0].SetActive(true);
                    break;
                case GuardController.EGuardState.Patrol:
                    _visualCueList[0].SetActive(true);
                    break;
                case GuardController.EGuardState.Investigate:
                    _visualCueList[1].SetActive(true);
                    break;
                case GuardController.EGuardState.Chase:
                    _visualCueList[2].SetActive(true);
                    break;
            }
        }
    }
}