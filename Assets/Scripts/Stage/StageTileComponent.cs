using UnityEngine;

public class StageTileComponent : MonoBehaviour
{
    [Header("Generatation")]
    [SerializeField] private StageTileType _tileType;
    public void SetTileType(StageTileType tileType)
    {
        _tileType = tileType;
    }

    [SerializeField] private GameObject _generatedObject;

    [Header("Prefabs")]
    [SerializeField] private StageTilePrefabLibrary _prefabLibrary;
    public void SetPrefabLibrary(StageTilePrefabLibrary prefabLibrary)
    {
        _prefabLibrary = prefabLibrary;
    }

#if UNITY_EDITOR
    public void ApplyTile(Quaternion rotation)
    {
        RemoveGeneratedObject();

        transform.localRotation = rotation;

        GameObject prefab = _prefabLibrary.GetPrefab(_tileType);

        if (prefab == null)
            return;

        _generatedObject = (GameObject)
            UnityEditor.PrefabUtility.InstantiatePrefab(prefab, transform);

        _generatedObject.transform.localPosition = Vector3.zero;
        _generatedObject.transform.localRotation = Quaternion.identity;
    }

    public void RemoveGeneratedObject()
    {
        if (_generatedObject == null)
            return;

        DestroyImmediate(_generatedObject);
        _generatedObject = null;
    }
#endif
}
