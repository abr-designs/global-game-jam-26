using UnityEngine;

public class StageTile : MonoBehaviour
{
    public StageTileType tileType;

    [Header("Prefabs")]
    public GameObject emptyPrefab;
    public GameObject floorPrefab;
    public GameObject floorOneWallPrefab;
    public GameObject floorCornerWallsPrefab;
    public GameObject wallNoFloorPrefab;
    public GameObject cornerNoFloorPrefab;

    private GameObject _currentInstance;

    public void ApplyTile(Quaternion rotation)
    {
        if (_currentInstance != null)
        {
            DestroyImmediate(_currentInstance);
        }

        transform.localRotation = rotation;

        GameObject prefab = tileType switch
        {
            StageTileType.Floor => floorPrefab,
            StageTileType.FloorOneWall => floorOneWallPrefab,
            StageTileType.FloorCornerWalls => floorCornerWallsPrefab,
            StageTileType.WallNoFloor => wallNoFloorPrefab,
            StageTileType.CornerNoFloor => cornerNoFloorPrefab,
            _ => emptyPrefab
        };

        if (prefab == null)
            return;

        _currentInstance = Instantiate(prefab, transform);
        _currentInstance.transform.localPosition = Vector3.zero;
        _currentInstance.transform.localRotation = Quaternion.identity;
    }
}
