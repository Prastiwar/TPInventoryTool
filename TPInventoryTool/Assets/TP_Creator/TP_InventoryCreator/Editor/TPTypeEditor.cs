using UnityEditor;
using TP_Inventory;
using UnityEngine;

namespace TP_InventoryEditor
{
    [CustomEditor(typeof(TPType))]
    public class TPTypeEditor : ScriptlessInventoryEditor
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