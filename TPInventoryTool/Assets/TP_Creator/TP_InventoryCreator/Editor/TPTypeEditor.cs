using UnityEditor;
using TP_Inventory;
using UnityEngine;

namespace TP_InventoryEditor
{
    [CustomEditor(typeof(TPType))]
    public class TPTypeEditor : ScriptlessEditor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("Inventory Type");
            base.OnInspectorGUI();

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Open Inventory Manager", GUILayout.Height(20)))
            {
                TPInventoryDesigner.OpenWindow();
            }
        }
    }
}