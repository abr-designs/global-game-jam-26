using UnityEngine;

public class StageCreator : MonoBehaviour
{
    [Header("New Stage")]
    public string newStageName = "New Stage";
    public Vector3Int stageDimensions = new Vector3Int(5, 2, 5);

    [Header("References")]
    public Vector3 tileSize = new Vector3(4f, 4f, 4f);
    public GameObject emptyPrefab;
    public GameObject floorPrefab;
    public GameObject floorOneWallPrefab;
    public GameObject floorCornerWallsPrefab;
    public GameObject wallNoFloorPrefab;
    public GameObject cornerNoFloorPrefab;

    [Header("Rotation Settings")]
    public float _rotationDirection = -90f;
    public float _wallRotationOffset = -90f;
    public float _cornerRotationOffset = -90f;

    [ContextMenu("Create New Stage")]
    public void CreateNewStage()
    {
        GameObject stageRoot = new GameObject(newStageName);
        stageRoot.transform.localPosition = Vector3.zero;
        StageController stageController = stageRoot.AddComponent<StageController>();
        stageController.levelName = newStageName;

        for (int y = 0; y < stageDimensions.y; y++)
        {
            GameObject floor = new GameObject($"Floor {y}");
            floor.transform.SetParent(stageRoot.transform);
            floor.transform.localPosition = new Vector3(0, y * tileSize.y, 0);

            for (int x = 0; x < stageDimensions.x; x++)
            {
                //GameObject row = new GameObject($"Row_{z}");
                //row.transform.SetParent(floor.transform);
                //row.transform.localPosition = new Vector3(0, 0, z * tileSize.z);

                for (int z = 0; z < stageDimensions.z; z++)
                {
                    GameObject tile = new GameObject($"Tile [{x}, {z}]");
                    //tile.transform.SetParent(row.transform);
                    tile.transform.SetParent(floor.transform);
                    tile.transform.localPosition = new Vector3(x * tileSize.x, 0, z * tileSize.z);

                    StageTile stageTile = tile.AddComponent<StageTile>();

                    stageTile.emptyPrefab = emptyPrefab;
                    stageTile.floorPrefab = floorPrefab;
                    stageTile.floorOneWallPrefab = floorOneWallPrefab;
                    stageTile.floorCornerWallsPrefab = floorCornerWallsPrefab;
                    stageTile.wallNoFloorPrefab = wallNoFloorPrefab;
                    stageTile.cornerNoFloorPrefab = cornerNoFloorPrefab;

                    StageTileType type = GetTileType(x, y, z, out Quaternion rotation);

                    stageTile.tileType = type;
                    stageTile.ApplyTile(rotation);

                }
            }
        }
    }

    private StageTileType GetTileType(
    int x,
    int y,
    int z,
    out Quaternion rotation)
    {
        rotation = Quaternion.identity;

        bool minX = x == 0;
        bool maxX = x == stageDimensions.x - 1;
        bool minZ = z == 0;
        bool maxZ = z == stageDimensions.z - 1;

        bool isEdgeX = minX || maxX;
        bool isEdgeZ = minZ || maxZ;
        bool isCorner = isEdgeX && isEdgeZ;

        // GROUND FLOOR
        if (y == 0)
        {
            if (isCorner)
            {
                rotation = GetCornerRotation(minX, maxX, minZ, maxZ);
                return StageTileType.FloorCornerWalls;
            }

            if (isEdgeX || isEdgeZ)
            {
                rotation = GetWallRotation(minX, maxX, minZ, maxZ);
                return StageTileType.FloorOneWall;
            }

            return StageTileType.Floor;
        }

        // UPPER FLOORS
        if (isCorner)
        {
            rotation = GetCornerRotation(minX, maxX, minZ, maxZ);
            return StageTileType.CornerNoFloor;
        }

        if (isEdgeX || isEdgeZ)
        {
            rotation = GetWallRotation(minX, maxX, minZ, maxZ);
            return StageTileType.WallNoFloor;
        }

        return StageTileType.Empty;
    }
    private Quaternion GetWallRotation(
    bool minX,
    bool maxX,
    bool minZ,
    bool maxZ)
    {
        if (minZ) return Quaternion.Euler(0f, _wallRotationOffset + _rotationDirection * 0f, 0f);
        if (maxX) return Quaternion.Euler(0f, _wallRotationOffset + _rotationDirection * 1f, 0f);
        if (maxZ) return Quaternion.Euler(0f, _wallRotationOffset + _rotationDirection * 2f, 0f);
        if (minX) return Quaternion.Euler(0f, _wallRotationOffset + _rotationDirection * 3f, 0f);

        return Quaternion.identity;
    }
    private Quaternion GetCornerRotation(
    bool minX,
    bool maxX,
    bool minZ,
    bool maxZ)
    {
        if (minX && minZ) return Quaternion.Euler(0f, _cornerRotationOffset + _rotationDirection * 0f, 0f);
        if (maxX && minZ) return Quaternion.Euler(0f, _cornerRotationOffset + _rotationDirection * 1f, 0f);
        if (maxX && maxZ) return Quaternion.Euler(0f, _cornerRotationOffset + _rotationDirection * 2f, 0f);
        if (minX && maxZ) return Quaternion.Euler(0f, _cornerRotationOffset + _rotationDirection * 3f, 0f);

        return Quaternion.identity;
    }

}
