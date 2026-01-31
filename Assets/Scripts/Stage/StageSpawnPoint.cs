using UnityEditor;
using UnityEngine;

public class StageSpawnPoint : MonoBehaviour
{

    private float arrowLength = 2f;
    private float arrowHeadLength = 1f;
    private float arrowHeadAngle = 25f;
    private float sphereRadius = 0.5f;
    private Color gizmoColor = Color.red;

    [SerializeField] private Vector3 environmentOffset;

    private void OnDrawGizmos()
    {
        DrawArrow(transform.position, transform.forward);
    }

    private void DrawArrow(Vector3 position, Vector3 direction)
    {
        Gizmos.color = gizmoColor;

        Vector3 end = position + direction.normalized * arrowLength;
        Gizmos.DrawLine(environmentOffset + position, environmentOffset + end);

        // Arrow head
        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0f, 180f + arrowHeadAngle, 0f) * Vector3.forward;
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0f, 180f - arrowHeadAngle, 0f) * Vector3.forward;

        Gizmos.DrawLine(environmentOffset + end, environmentOffset + end + right * arrowHeadLength);
        Gizmos.DrawLine(environmentOffset + end, environmentOffset + end + left * arrowHeadLength);

        Gizmos.DrawWireSphere(environmentOffset + transform.position, sphereRadius);
    }
}
