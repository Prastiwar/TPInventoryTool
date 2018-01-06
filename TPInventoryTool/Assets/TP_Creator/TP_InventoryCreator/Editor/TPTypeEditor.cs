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
            EditorGUILayout.LabelField("Inventory Type");

            if (GUILayout.Button("Open Inventory Manager", GUILayout.Height(20)))
            {
                TPInventoryDesigner.OpenWindow();
            }
        }
    }
}