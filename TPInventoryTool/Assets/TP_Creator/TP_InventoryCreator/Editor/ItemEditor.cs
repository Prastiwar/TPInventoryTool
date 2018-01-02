using System.Collections.Generic;
using TP_Inventory;
using UnityEditor;

[CustomEditor(typeof(TPItem))]
public class ItemEditor : ScriptlessEditor
{
    //private static readonly string[] scriptField = new string[] { "m_Script"};

    List<TPItem> Items = new List<TPItem>();
    int length;
    TPItem item;

    void OnEnable()
    {
        Items = FindAssetsByType<TPItem>();
        length = Items.Count;
        item = (TPItem)target;
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
                if(item.ID == Items[i].ID)
                {
                    EditorGUILayout.HelpBox("ID is actually used, should be unique!", MessageType.Error);
                    break;
                }
            }
        }
        DrawPropertiesExcluding(serializedObject, scriptField);

        serializedObject.ApplyModifiedProperties();
    }

}