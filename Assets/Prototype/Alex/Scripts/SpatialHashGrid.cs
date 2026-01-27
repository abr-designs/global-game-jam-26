using System.Collections.Generic;
using UnityEngine;

public class SpatialHashGrid
{
    private readonly float m_cellSize;
    private readonly Dictionary<Vector2Int, List<CrowdAgent>> m_cells = new();

    public SpatialHashGrid(float cellSize)
    {
        m_cellSize = cellSize;
    }

    private Vector2Int GetCell(Vector3 position)
    {
        return new Vector2Int(
            Mathf.FloorToInt(position.x / m_cellSize),
            Mathf.FloorToInt(position.z / m_cellSize)
        );
    }

    public void Clear()
    {
        m_cells.Clear();
    }

    public void AddAgent(CrowdAgent agent)
    {
        Vector2Int cell = GetCell(agent.transform.position);

        if (!m_cells.TryGetValue(cell, out var list))
        {
            list = new List<CrowdAgent>();
            m_cells[cell] = list;
        }

        list.Add(agent);
    }

    public List<CrowdAgent> GetNeighbors(Vector3 position)
    {
        Vector2Int center = GetCell(position);
        List<CrowdAgent> neighbors = new List<CrowdAgent>();

        // Check surrounding 3x3 cells
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2Int cell = center + new Vector2Int(x, y);

                if (m_cells.TryGetValue(cell, out var list))
                {
                    neighbors.AddRange(list);
                }
            }
        }

        return neighbors;
    }
}