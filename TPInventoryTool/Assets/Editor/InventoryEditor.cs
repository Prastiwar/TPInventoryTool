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
    SerializedProperty slots;
    bool Toggle = false;

    void OnEnable()
    {
        slots = serializedObject.FindProperty("slots");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("This script allows you to manage your Inventory");
        DrawPropertiesExcluding(serializedObject, scriptField);

        if (GUILayout.Button("Check loaded slots", GUILayout.Width(200)))
        {
            Toggle = !Toggle;
        }

        if (Toggle)
        {
            EditorGUILayout.PropertyField(slots, new GUIContent("Slots Loaded: "), true);
            foreach (SerializedProperty item in slots)
            {
                EditorGUILayout.ObjectField(item);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
