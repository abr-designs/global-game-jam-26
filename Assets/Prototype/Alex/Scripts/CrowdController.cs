using UnityEngine;

public class CrowdController : MonoBehaviour
{
    [SerializeField]
    private int crowdSize;
    [SerializeField]
    private CrowdAgent crowdAgenPrefab;
    
    [Header("Movement")]
    public float baseMoveSpeed = 3f;
    public float rotationSpeed = 8f;

    [Header("Separation")]
    public float neighborRadius = 1.5f;
    public float separationStrength = 1.2f;

    [Header("Alignment")]
    public float alignmentRadius = 2.5f;
    public float alignmentStrength = 0.5f;

    [Header("Wander")]
    public float wanderStrength = 0.3f;
    public float wanderSpeed = 0.5f;

    [Header("Goal")]
    public float goalOffsetRadius = 2f;

    [SerializeField]
    private Transform goal;

    private void Start()
    {
        GenerateCrowd();
    }

    private void GenerateCrowd()
    {
        
    }

    private void CustomUpdate()
    {
        
    }

}
