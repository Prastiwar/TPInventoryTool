using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Stat))]
public class StatEditor : Editor
{
    private static readonly string[] scriptField = new string[] { "m_Script"};

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Statistic");
        DrawPropertiesExcluding(serializedObject, scriptField);

        serializedObject.ApplyModifiedProperties();
    }
}
