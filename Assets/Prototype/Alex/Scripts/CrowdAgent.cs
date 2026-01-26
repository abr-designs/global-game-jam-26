using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CrowdAgent : MonoBehaviour
{
    public float baseMoveSpeed = 3f;
    public float rotationSpeed = 8f;

    public float neighborRadius = 1.5f;
    public float separationStrength = 1.2f;

    public float alignmentRadius = 2.5f;
    public float alignmentStrength = 0.5f;

    public float wanderStrength = 0.3f;
    public float wanderSpeed = 0.5f;

    public float goalOffsetRadius = 2f;

    private Vector3 m_goalOffset;

    private float m_preferredSpeed;
    private float m_wanderOffset;

    private void Start()
    {
        // Everyone aims slightly differently
        m_goalOffset = Random.insideUnitSphere * goalOffsetRadius;
        m_goalOffset.y = 0f;

        // Everyone walks differently
        m_preferredSpeed = baseMoveSpeed * Random.Range(0.85f, 1.15f);

        // Desync wander noise
        m_wanderOffset = Random.value * 100f;
    }

    private void Update()
    {
        Vector3 directionToGoal = ComputeGoalDirection();
        Vector3 separation = ComputeSeparation();
        Vector3 alignment = ComputeAlignment();
        Vector3 wander = ComputeWander();

        Vector3 finalDirection =
            directionToGoal +
            separation * separationStrength +
            alignment * alignmentStrength +
            wander;

        finalDirection.y = 0f;

        if (finalDirection.sqrMagnitude > 0.001f)
        {
            Vector3 moveDir = finalDirection.normalized;

            transform.position += moveDir * (m_preferredSpeed * Time.deltaTime);

            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    private Vector3 ComputeGoalDirection()
    {
        Vector3 target = goal.position + m_goalOffset;
        return (target - transform.position).normalized;
    }

    private Vector3 ComputeSeparation()
    {
        Vector3 force = Vector3.zero;
        Collider[] neighbors = Physics.OverlapSphere(transform.position, neighborRadius);

        foreach (Collider col in neighbors)
        {
            if (col.transform == transform) continue;

            Vector3 diff = transform.position - col.transform.position;
            float dist = diff.magnitude;

            if (dist > 0f)
                force += diff.normalized / dist;
        }

        return force;
    }

    private Vector3 ComputeAlignment()
    {
        Vector3 avgDir = Vector3.zero;
        int count = 0;

        Collider[] neighbors = Physics.OverlapSphere(transform.position, alignmentRadius);

        foreach (Collider col in neighbors)
        {
            if (col.transform == transform) continue;

            CrowdAgent other = col.GetComponent<CrowdAgent>();
            if (other == null) continue;

            avgDir += other.transform.forward;
            count++;
        }

        if (count == 0) return Vector3.zero;
        return (avgDir / count).normalized;
    }

    private Vector3 ComputeWander()
    {
        float noise =
            Mathf.PerlinNoise(Time.time * wanderSpeed, m_wanderOffset) - 0.5f;

        return transform.right * (noise * wanderStrength);
    }
}
