using TP_Inventory;
using UnityEditor;
using UnityEngine;

namespace TP_InventoryEditor
{
    [CustomEditor(typeof(TPInventoryCreator))]
    public class TPInventoryCreatorEditor : ScriptlessEditor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("This script allows you to manage your Inventory");

            base.OnInspectorGUI();

            if (GUILayout.Button("Open Inventory Manager", GUILayout.Height(30)))
            {
                TPInventoryDesigner.OpenWindow();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}