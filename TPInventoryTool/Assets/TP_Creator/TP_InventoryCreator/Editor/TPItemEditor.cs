using System.Collections.Generic;
using TP_Inventory;
using UnityEditor;
using UnityEngine;

namespace TP_InventoryEditor
{
    [CustomEditor(typeof(TPItem))]
    public class TPItemEditor : ScriptlessEditor
    {
        List<TPItem> Items = new List<TPItem>();
        int length;
        TPItem item;

        void OnEnable()
        {
            item = (TPItem)target;
            Items = TPHelper.FindAssetsByType<TPItem>();
            length = Items.Count;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("Custom Item");
            item.ID = EditorGUILayout.IntField("Unique ID", item.ID);

            for (int i = 0; i < length; i++)
            {
                if (Items[i].name != item.name)
                {
                    if (item.ID == Items[i].ID)
                    {
                        EditorGUILayout.HelpBox("ID is actually used, must be unique!", MessageType.Error);
                        break;
                    }
                }
            }

            if (GUI.changed)
                EditorUtility.SetDirty(item);

            DrawPropertiesExcluding(serializedObject, scriptField);

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Open Inventory Manager", GUILayout.Height(20)))
            {
                TPInventoryDesigner.OpenWindow();
            }
        }

    }
}