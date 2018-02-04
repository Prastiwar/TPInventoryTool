using UnityEngine;
using UnityEditor;
using TP.Inventory;

namespace TP.InventoryEditor
{
    [CustomEditor(typeof(TPInventoryGUIData))]
    internal class TPInventoryGUIDataEditor : ScriptlessInventoryEditor
    {
        TPInventoryGUIData TPInventoryEditorData;

        void OnEnable()
        {
            TPInventoryEditorData = (TPInventoryGUIData)target;
            if (serializedObject.targetObject.hideFlags != HideFlags.NotEditable)
                serializedObject.targetObject.hideFlags = HideFlags.NotEditable;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Container for editor data");
            if (!TPInventoryCreator.DebugMode)
                return;

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

            serializedObject.ApplyModifiedProperties();
        }
    }
}