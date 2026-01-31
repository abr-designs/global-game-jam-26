#if UNITY_EDITOR
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using Utilities.Debugging;

namespace GGJ.EditorUtility
{
    public class ZooLayout : MonoBehaviour
    {
        private readonly string[] COLORS = new[]
        {
            "#dc6250",
            "#deada5",
            "#dad4c9",
            "#ffd183",
            "#eeb24a",
            "#55927f",
            "#21525a",
            "#272a32",
            "#2152a5",
            "#5a8bde",
            "#b89ce9",
            "#844790",
        
        };
        [SerializeField]
        private DefaultAsset[] prefabFolders;

        [SerializeField, Min(1f)]
        private float spacing;
        [SerializeField, Min(1)]
        private int maxColumns;

        [SerializeField, ReadOnly]
        private List<GameObject> generatedPrefabs;
    
        [SerializeField, ReadOnly]
        private List<GameObject> floors;

        [SerializeField]
        private SpriteRenderer rendererPrefab;
    
        [Button]
        private void GenerateZoo()
        {
            generatedPrefabs ??= new List<GameObject>();
            floors ??= new List<GameObject>();
            ClearZoo();
        
            var row = 0;
            var column = 0;
            var colorIndex = 0;
            foreach (var prefabFolder in prefabFolders)
            {
                var prefabs = GetAllPrefabsIncludingModels(prefabFolder);

                ColorUtility.TryParseHtmlString(COLORS[colorIndex], out var collectionColor);
            
                foreach (var prefab in prefabs)
                {
                    var position = new Vector3(column * spacing, 0f, row * spacing);
                    var floor = Instantiate(rendererPrefab, position, rendererPrefab.transform.rotation);
                    floor.gameObject.name = $"Floor[{column}, {row}]";
                    floor.enabled = true;
                    floor.transform.SetParent(transform);
                    floor.transform.localScale = Vector3.one * spacing;
                    floor.color = collectionColor;
                
                    var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

                    instance.transform.position = position;
                    instance.transform.SetParent(transform);
                
                    if (column + 1 >= maxColumns)
                    {
                        column = 0;
                        row++;
                    }
                    else column++;

                    floors.Add(floor.gameObject);
                    generatedPrefabs.Add(instance);
                }

                colorIndex++;
                column = 0;
                row+=2;
            }
        }

        [Button]
        private void ClearZoo()
        {
            for (var i = generatedPrefabs.Count - 1; i >= 0; i--)
            {
                DestroyImmediate(generatedPrefabs[i]);
            }
            generatedPrefabs.Clear();
        
            for (var i = floors.Count - 1; i >= 0; i--)
            {
                DestroyImmediate(floors[i]);
            }
            floors.Clear();
        }

        private static List<GameObject> GetAllPrefabsIncludingModels(DefaultAsset folder)
        {
            var results = new List<GameObject>();

            if (folder == null)
                return results;

            var folderPath = AssetDatabase.GetAssetPath(folder);

            if (!AssetDatabase.IsValidFolder(folderPath))
                return results;

            var guids = AssetDatabase.FindAssets("t:Prefab t:model", new[] { folderPath });

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                if (prefab == null)
                    continue;

                var type = PrefabUtility.GetPrefabAssetType(prefab);

                if (type == PrefabAssetType.Regular ||
                    type == PrefabAssetType.Variant ||
                    type == PrefabAssetType.Model)
                {
                    results.Add(prefab);
                }
            }

            return results;
        }

        private void OnDrawGizmos()
        {
            if (generatedPrefabs == null)
                return; 
        
            foreach (var generatedPrefab in generatedPrefabs)
            {
                var pos = generatedPrefab.transform.position;
                Draw.Label(pos, generatedPrefab.name);
            }
        }
    }
}
#endif
