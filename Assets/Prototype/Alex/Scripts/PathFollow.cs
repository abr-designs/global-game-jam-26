using System;
using System.Linq;
using UnityEngine;

public class PathFollow : MonoBehaviour
{
    [SerializeField]
    private Transform[] navPoints;

    [SerializeField]
    private bool looping;

    [SerializeField]
    private Transform movingObject;

    [SerializeField]
    private float moveSpeed;

    [SerializeField, Min(0)]
    private int startingIndex;
    
    private int m_currentIndex;
    private int m_targetIndex;

    private Vector3 CurrentPoint => navPoints[m_currentIndex].position;
    private Vector3 TargetPoint => navPoints[m_targetIndex].position;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if (movingObject == null)
        {
            gameObject.SetActive(false);
            return;
        }
        
        m_currentIndex = startingIndex;
        m_targetIndex = GetNextTargetIndex(m_currentIndex);
        movingObject.forward = (TargetPoint - CurrentPoint).normalized;

        movingObject.position = navPoints[m_currentIndex].position;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Vector3.Distance(movingObject.position, navPoints[m_targetIndex].position) < 0.1f)
        {
            m_currentIndex = GetNextTargetIndex(m_currentIndex);
            m_targetIndex = GetNextTargetIndex(m_currentIndex);
            movingObject.forward = (TargetPoint - CurrentPoint).normalized;
        }


        movingObject.position = Vector3.MoveTowards(movingObject.position, TargetPoint, moveSpeed * Time.deltaTime);
    }

    private int GetNextTargetIndex(int index)
    {
        if (index + 1 >= navPoints.Length)
            return 0;

        return index + 1;
    }
    
    
    
    //================================================================================================================//

    private void OnDrawGizmos()
    {
        if (navPoints == null || navPoints.Length == 0)
            return;
        
        var positions = new ReadOnlySpan<Vector3>(navPoints.Select(x => x.position).ToArray());
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawLineStrip(positions, looping);
        
        Gizmos.color = Color.white;
        for (int i = 0; i < positions.Length; i++)
        {
            Gizmos.DrawWireSphere(positions[i], 0.25f);
        }
    }
}
