using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StageCreator))]
public class StageCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Draw Script field at the top, read-only (default Unity behavior)
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
        EditorGUI.EndDisabledGroup();

        // Draw New Stage header
        EditorGUILayout.PropertyField(serializedObject.FindProperty("newStageName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("stageDimensions"));

        // Button placed between stageDimensions and tileSize
        EditorGUILayout.Space();
        if (GUILayout.Button("Create New Stage"))
        {
            StageCreator creator = (StageCreator)target;
            creator.CreateNewStage();
        }
        EditorGUILayout.Space();

        // Draw the rest of the fields normally
        DrawRemainingProperties(
            serializedObject,
            "m_Script",
            "newStageName",
            "stageDimensions"
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