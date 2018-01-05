using UnityEditor;
using UnityEngine;

namespace TP_InventoryEditor
{
    [CustomEditor(typeof(TPInventoryData))]
    public class TPInventoryDataEditor : ScriptlessEditor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("Inventory Data");

            base.OnInspectorGUI();

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Open Inventory Manager", GUILayout.Height(20)))
            {
                TPInventoryDesigner.OpenWindow();
            }
        }
    }
}