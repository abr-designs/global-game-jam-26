using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StageTileComponent))]
public class StageTileComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Draw Script field at the top, read-only (default Unity behavior)
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("tileType"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("generatedObject"));

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Regenerate Tile Type"))
        {
            StageTileComponent tile = (StageTileComponent)target;
            tile.ApplyTile(tile.transform.localRotation);
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        DrawRemainingProperties(
            serializedObject,
            "m_Script",
            "tileType",
            "generatedObject"
        );

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawRemainingProperties(SerializedObject obj, params string[] exclude)
    {
        SerializedProperty prop = obj.GetIterator();
        bool enterChildren = true;

        while (prop.NextVisible(enterChildren))
        {
            enterChildren = false;

            bool skip = false;
            foreach (string ex in exclude)
            {
                if (prop.name == ex)
                {
                    skip = true;
                    break;
                }
            }

            if (!skip)
            {
                EditorGUILayout.PropertyField(prop, true);
            }
        }
    }
}
