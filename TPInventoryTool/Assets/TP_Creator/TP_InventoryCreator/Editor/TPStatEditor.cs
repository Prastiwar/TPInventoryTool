using TP_Inventory;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TPStat))]
public class TPStatEditor : ScriptlessEditor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Statistic");
        base.OnInspectorGUI();

        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Open Inventory Manager", GUILayout.Height(20)))
        {
            TPInventoryDesigner.OpenWindow();
        }
    }
}