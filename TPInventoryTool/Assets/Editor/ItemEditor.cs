using System.Collections;
using System.Collections.Generic;
using TP_Inventory_Item;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Item))]
public class ItemEditor : Editor
{
    private static readonly string[] scriptField = new string[] { "m_Script"};

    List<Item> Items = new List<Item>();
    int length;
    Item item;

    void OnEnable()
    {
        Items = FindAssetsByType<Item>();
        length = Items.Count;
        item = (Item)target;
    }

    public override void OnInspectorGUI()
    {
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
    }

    static List<T> FindAssetsByType<T>() where T : UnityEngine.Object
    {
        List<T> assets = new List<T>();
        string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (asset != null)
            {
                assets.Add(asset);
            }
        }
        return assets;
    }
}
