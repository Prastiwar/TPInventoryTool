using TP.Inventory;
using UnityEditor;
using UnityEngine;

namespace TP.InventoryEditor
{
    internal class ScriptlessInventoryEditor : Editor
    {
        public readonly string scriptField = "m_Script" ;

        public override void OnInspectorGUI()
        {
            DrawPropertiesExcluding(serializedObject, scriptField);

            OpenCreator();
        }

        public void OpenCreator()
        {
            if (TPInventoryCreator.DebugMode)
            {
                if (serializedObject.targetObject.hideFlags != HideFlags.NotEditable)
                    serializedObject.targetObject.hideFlags = HideFlags.NotEditable;
                return;
            }

            if (serializedObject.targetObject.hideFlags != HideFlags.None)
                serializedObject.targetObject.hideFlags = HideFlags.None;

            if (GUILayout.Button("Open Inventory Manager", GUILayout.Height(30)))
            {
                TPInventoryDesigner.OpenWindow();
            }
        }
    }
}