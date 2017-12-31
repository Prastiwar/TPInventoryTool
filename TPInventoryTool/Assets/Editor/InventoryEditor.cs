using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InventoryCreator))]
public class InventoryEditor : Editor
{
    [MenuItem("TP_Inventory/Create Inventory Manager")]
    static void InventoryCreator()
    {
        GameObject creator = new GameObject("InventoryCreator");
        creator.AddComponent<InventoryCreator>();
    }

    private static readonly string[] scriptField = new string[] { "m_Script" };

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("This script allows you to manage your Inventory");
        DrawPropertiesExcluding(serializedObject, scriptField);

        serializedObject.ApplyModifiedProperties();
    }
}
