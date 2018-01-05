using UnityEngine;
using UnityEditor;

namespace TP_InventoryEditor
{
    [CustomEditor(typeof(TPInventoryGUIData))]
    public class TPInventoryGUIDataEditor : ScriptlessEditor
    {
        TPInventoryGUIData TPInventoryEditorData;

        void OnEnable()
        {
            TPInventoryEditorData = (TPInventoryGUIData)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            //base.OnInspectorGUI();

            EditorGUILayout.LabelField("GUI Skin");
            TPInventoryEditorData.GUISkin =
                (EditorGUILayout.ObjectField(TPInventoryEditorData.GUISkin, typeof(GUISkin), true) as GUISkin);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Inventory Data file - Path");
            EditorGUILayout.BeginHorizontal();
            TPInventoryEditorData.InventoryDataPath = EditorGUILayout.TextField(TPInventoryEditorData.InventoryDataPath);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Inventory Assets(Items, Types, Stats) - Path");
            EditorGUILayout.BeginHorizontal();
            TPInventoryEditorData.InventoryAssetsPath = EditorGUILayout.TextField(TPInventoryEditorData.InventoryAssetsPath);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Empty Inventory Hierarchy Prefab");
            TPInventoryEditorData.InventoryPrefab = (EditorGUILayout.ObjectField(TPInventoryEditorData.InventoryPrefab, typeof(GameObject), true) as GameObject);

            if (GUI.changed)
                EditorUtility.SetDirty(TPInventoryEditorData);

            DrawPropertiesExcluding(serializedObject, scriptField);

            serializedObject.ApplyModifiedProperties();
        }
    }
}