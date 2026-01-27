using System;
using System.Collections.Generic;
using Prototype.Alex.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class CrowdController : MonoBehaviour
{
    [SerializeField] 
    public Transform goal;
    public List<CrowdAgent> agents;

    [Header("Crowd")]
    [SerializeField]
    private int crowdSize = 1;
    [SerializeField]
    private CrowdAgent crowdAgentPrefab;

    //Agent Properties
    //================================================================================================================//
    [Header("Agent Movement")]
    public float baseMoveSpeed = 3f;
    public float rotationSpeed = 8f;

    [Header("Agent Separation")]
    public float separationRadius = 1.5f;
    public float separationStrength = 1.2f;

    [Header("Agent Alignment")]
    public float alignmentRadius = 2.5f;
    public float alignmentStrength = 0.5f;

    [Header("Agent Wander")]
    public float wanderStrength = 0.3f;
    public float wanderSpeed = 0.5f;

    [Header("Agent Goal")]
    public float goalOffsetRadius = 2f;

    //================================================================================================================//

    private void OnEnable()
    {
        CrowdControllerManager.RegisterController(this);
    }

    private void Start()
    {
        GenerateCrowd();
    }

    public void CustomUpdate()
    {
        foreach (var agent in agents)
        {
            agent.CustomUpdate();
        }
    }

    //================================================================================================================//

    private void GenerateCrowd()
    {
        for (int i = 0; i < crowdSize; i++)
        {
            var spawnPos = goal.position + Random.insideUnitSphere * goalOffsetRadius;
            spawnPos.y = 0f;
            
            var agent = Instantiate(crowdAgentPrefab, spawnPos, Quaternion.identity, transform);
            Register(agent);
            agent.Init(this);
        }
    }
    
    private void Register(CrowdAgent agent)
    {
        agents ??= new List<CrowdAgent>();
        agents.Add(agent);
    }
}
