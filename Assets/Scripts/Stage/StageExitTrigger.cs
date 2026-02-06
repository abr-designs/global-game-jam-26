using System;
using UnityEngine;

[RequireComponent (typeof(Collider))]
public class StageExitTrigger : MonoBehaviour
{
    public event Action PlayerReachedExit;

    private bool _isExitingStage;

    // gizmo
    private SphereCollider m_sphereCollider;
    private float sphereRadius = 2f;
    private Color gizmoColor = Color.cyan;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
            return;

        // prevent queuing multiple exists
        if (_isExitingStage)
            return;

        _isExitingStage = true;

        PlayerReachedExit?.Invoke();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, sphereRadius);
    }

    private void OnDrawGizmosSelected()
    {
        m_sphereCollider = GetComponent<SphereCollider>();
        if (m_sphereCollider == null)
            return;

        sphereRadius = m_sphereCollider.radius;
    }
}