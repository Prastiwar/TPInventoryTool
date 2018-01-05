using UnityEditor;
using TP_Inventory;
using UnityEngine;

[CustomEditor(typeof(TPType))]
public class TPTypeEditor : ScriptlessEditor
{
    GUISkin skin;
    void OnEnable()
    {
        TPInventoryGUIData tp = FindObjectOfType<TPInventoryGUIData>();
        skin = tp.GUISkin;
        skin = AssetDatabase.LoadAssetAtPath(
            "Assets/TP_Creator/TP_InventoryCreator/EditorResources/EditorGUIData.guiskin", typeof(GUISkin)) as GUISkin;
        Debug.Log(skin);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Inventory Type");
        base.OnInspectorGUI();

        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Open Inventory Manager", skin.button, GUILayout.Height(20)))
        {
            TPInventoryDesigner.OpenWindow();
        }
    }
}