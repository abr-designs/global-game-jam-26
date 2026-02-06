using UnityEngine;

[CreateAssetMenu(
    fileName = "StageTilePrefabLibrary",
    menuName = "Stage/Tile Prefab Library"
)]
public class StageTilePrefabLibrary : ScriptableObject
{
    public GameObject emptyPrefab;
    public GameObject floorPrefab;
    public GameObject floorOneWallPrefab;
    public GameObject floorCornerWallsPrefab;
    public GameObject wallNoFloorPrefab;
    public GameObject cornerNoFloorPrefab;

    public GameObject GetPrefab(StageTileType type)
    {
        return type switch
        {
            StageTileType.Floor => floorPrefab,
            StageTileType.FloorOneWall => floorOneWallPrefab,
            StageTileType.FloorCornerWalls => floorCornerWallsPrefab,
            StageTileType.WallNoFloor => wallNoFloorPrefab,
            StageTileType.CornerNoFloor => cornerNoFloorPrefab,
            _ => emptyPrefab
        };
    }
}