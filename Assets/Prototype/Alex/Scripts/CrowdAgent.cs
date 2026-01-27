using UnityEngine;
using System.Collections.Generic;
using Prototype.Alex.Scripts;

public class CrowdAgent : MonoBehaviour
{
    private Transform m_goal;
    private Vector3 m_goalOffset;

    private float m_preferredSpeed;
    private float m_wanderOffset;

    private CrowdController m_controller;

    private float BaseMoveSpeed => m_controller.baseMoveSpeed;
    private float RotationSpeed  => m_controller.rotationSpeed;
    private float SeparationRadius  => m_controller.separationRadius;
    private float SeparationStrength  => m_controller.separationStrength;
    private float AlignmentRadius  => m_controller.alignmentRadius;
    private float AlignmentStrength  => m_controller.alignmentStrength;
    private float WanderStrength  => m_controller.wanderStrength;
    private float WanderSpeed  => m_controller.wanderSpeed;
    private float GoalOffsetRadius  => m_controller.goalOffsetRadius;

    private bool setup;

    public void Init(CrowdController crowdController)
    {
        m_controller = crowdController;
        m_goal = crowdController.goal;
        
        m_goalOffset = Random.insideUnitSphere * GoalOffsetRadius;
        m_goalOffset.y = 0f;

        m_preferredSpeed = BaseMoveSpeed * Random.Range(0.85f, 1.15f);
        m_wanderOffset = Random.value * 100f;
        setup = true;
    }

    public void CustomUpdate()
    {
        if (!setup)
            return;
        
        Vector3 directionToGoal = ComputeGoalDirection();

        List<CrowdAgent> neighbors = CrowdControllerManager.GetNeighbors(transform.position);

        Vector3 separation = ComputeSeparation(neighbors);
        Vector3 alignment = ComputeAlignment(neighbors);
        Vector3 wander = ComputeWander();

        Vector3 finalDirection =
            directionToGoal +
            separation * SeparationStrength +
            alignment * AlignmentStrength +
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
                RotationSpeed * Time.deltaTime
            );
        }
    }

    private Vector3 ComputeGoalDirection()
    {
        Vector3 target = m_goal.position + m_goalOffset;
        return (target - transform.position).normalized;
    }

    private Vector3 ComputeSeparation(List<CrowdAgent> neighbors)
    {
        Vector3 force = Vector3.zero;

        foreach (var other in neighbors)
        {
            if (other == this) continue;

            Vector3 diff = transform.position - other.transform.position;
            float dist = diff.magnitude;

            if (dist > 0f && dist < SeparationRadius)
                force += diff.normalized / dist;
        }

        return force;
    }

    private Vector3 ComputeAlignment(List<CrowdAgent> neighbors)
    {
        Vector3 avgDir = Vector3.zero;
        int count = 0;

        foreach (var other in neighbors)
        {
            if (other == this) continue;

            float dist =
                Vector3.Distance(transform.position, other.transform.position);

            if (dist > AlignmentRadius) continue;

            avgDir += other.transform.forward;
            count++;
        }

        if (count == 0) return Vector3.zero;
        return (avgDir / count).normalized;
    }

    private Vector3 ComputeWander()
    {
        float noise =
            Mathf.PerlinNoise(Time.time * WanderSpeed, m_wanderOffset) - 0.5f;

        return transform.right * (noise * WanderStrength);
    }
}
