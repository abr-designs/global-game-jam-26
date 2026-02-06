using UnityEngine;

public class StageTileComponent : MonoBehaviour
{
    [Header("Generatation")]
    public StageTileType tileType;
    public GameObject generatedObject;

    [Header("Prefabs")]
    public StageTilePrefabLibrary prefabLibrary;

#if UNITY_EDITOR
    public void ApplyTile(Quaternion rotation)
    {
        RemoveGeneratedObject();

        transform.localRotation = rotation;

        GameObject prefab = prefabLibrary.GetPrefab(tileType);

        if (prefab == null)
            return;

        generatedObject = (GameObject)
            UnityEditor.PrefabUtility.InstantiatePrefab(prefab, transform);

        generatedObject.transform.localPosition = Vector3.zero;
        generatedObject.transform.localRotation = Quaternion.identity;
    }

    public void RemoveGeneratedObject()
    {
        if (generatedObject == null)
            return;

        DestroyImmediate(generatedObject);
        generatedObject = null;
    }
#endif
}
