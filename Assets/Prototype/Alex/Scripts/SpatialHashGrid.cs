using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Utilities.Debugging;

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

    public int GetDensityAtPosition(Vector3 worldPosition)
    {
        var cell = GetCell(worldPosition);

        return !m_cells.TryGetValue(cell, out var list) ? 0 : list.Count;
    }


    [Conditional("DEBUG")]
    public void GizmosDrawGrid()
    {
        var size = new Vector3(m_cellSize, 0.01f, m_cellSize);
        foreach (var keyValuePair in m_cells)
        {
            var count = keyValuePair.Value.Count;
            var position = GetCellWorldPosition(keyValuePair.Key);
            Gizmos.color = Color.Lerp(Color.green, Color.red, count / 5f);
            Gizmos.DrawCube(position, size);
            
            Draw.Label(position, $"{count}");
        }
        
        Vector3 GetCellWorldPosition(Vector2Int cell)
        {
            return new Vector3(
                (cell.x + 0.5f) * m_cellSize,
                0f,
                (cell.y + 0.5f) * m_cellSize
            );
        }
    }
}