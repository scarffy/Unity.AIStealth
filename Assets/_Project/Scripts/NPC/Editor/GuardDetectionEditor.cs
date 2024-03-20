using UnityEditor;
using UnityEngine;

namespace Stealth.AI
{
    [CustomEditor(typeof(GuardDetection))]
    public class GuardDetectionEditor : Editor
    {
        private void OnSceneGUI()
        {
            GuardDetection fov = (GuardDetection)target;

            Handles.color = Color.white;
            Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov._viewRadius);

            Vector3 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov._viewAngle / 2);
            Vector3 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov._viewAngle / 2);

            Handles.color = Color.yellow;
            Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov._viewRadius);
            Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov._viewRadius);

            if (fov._playerInRange)
            {
                Handles.color = Color.green;
                Handles.DrawLine(fov.transform.position, fov._playerPosition.position);
            }
        }

        private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
        {
            angleInDegrees += eulerY;
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
    }
}