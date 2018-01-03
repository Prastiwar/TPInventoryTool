using TP_Inventory;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TPInventoryCreator))]
public class TPInventoryEditor : ScriptlessEditor
{
    //[MenuItem("TP_Inventory/Create Inventory Manager")]
    //static void InventoryCreator()
    //{
    //    GameObject creator = new GameObject("InventoryCreator");
    //    creator.AddComponent<TPInventoryCreator>();
    //    creator.AddComponent<TPInventorySaveLoad>();
    //}

    //private static readonly string[] scriptField = new string[] { "m_Script" };
    //SerializedProperty slots;
    //bool Toggle = false;
    //bool isSaving = false;
    //TPInventoryCreator inventoryCreator;

    //void OnEnable()
    //{
    //    inventoryCreator = target as TPInventoryCreator;
    //    slots = serializedObject.FindProperty("Slots");
    //}

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("This script allows you to manage your Inventory");
        //DrawPropertiesExcluding(serializedObject, scriptField);

        //GUILayout.Space(10);
        //GUILayout.BeginHorizontal();

        //if (GUILayout.Button("Check loaded slots"))
        //{
        //    Toggle = !Toggle;
        //}

        //GUILayout.Toggle(isSaving, "", GUILayout.Width(10));
        //if (GUILayout.Button("Do you want to Save/Load?"))
        //{
        //    isSaving = !isSaving;
        //    inventoryCreator.isSaving = isSaving;
        //}

        //GUILayout.EndHorizontal();

        //if (Toggle)
        //{
        //    EditorGUILayout.PropertyField(slots, new GUIContent("Slots Loaded: "), true);
        //    foreach (SerializedProperty item in slots)
        //    {
        //        EditorGUILayout.ObjectField(item);
        //    }
        //}
        base.OnInspectorGUI();
        if (GUILayout.Button("Open Inventory Manager", GUILayout.Height(30)))
        {
            TPInventoryDesigner.OpenWindow();
        }
        serializedObject.ApplyModifiedProperties();
    }
}