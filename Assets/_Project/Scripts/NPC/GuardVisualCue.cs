using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stealth.AI
{
    public class GuardVisualCue : MonoBehaviour
    {
        [SerializeField] private GuardDetection _guardDetection;

        private void Start()
        {
            if (_guardDetection == null)
                Debug.Log($"[Visual] Guard detection reference is not found");

        }

        private void Update()
        {
            Vector3 directionToCamera = Camera.main.transform.position - transform.position;

            // Project the direction onto the XZ plane (Y axis)
            directionToCamera.y = 0f;

            // Rotate the object to face the camera
            transform.rotation = Quaternion.LookRotation(directionToCamera);
        }

        private void SetVisualCue()
        {

        }
    }
}