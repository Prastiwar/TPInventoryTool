using System.Collections.Generic;
using TP_Inventory;
using UnityEditor;
using UnityEngine;

namespace TP_InventoryEditor
{
    [CustomEditor(typeof(TPItem))]
    internal class TPItemEditor : ScriptlessInventoryEditor
    {
        List<TPItem> Items = new List<TPItem>();
        int length;
        TPItem item;

        SerializedProperty _ID;

        void OnEnable()
        {
            item = (TPItem)target;
            Items = TPHelper.FindAssetsByType<TPItem>();
            length = Items.Count;
            _ID = serializedObject.FindProperty("_ID");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("Custom Item");
            EditorGUILayout.PropertyField(_ID);

            for (int i = 0; i < length; i++)
            {
                if (Items[i].name != item.name)
                {
                    if (item.ID == Items[i].ID)
                    {
                        _ID.intValue++;
                        Repaint();
                        break;
                    }
                }
            }

            if (GUI.changed)
                EditorUtility.SetDirty(item);

            DrawPropertiesExcluding(serializedObject, new string[] { "_ID", "m_Script"});

            serializedObject.ApplyModifiedProperties();
        }

    }
}