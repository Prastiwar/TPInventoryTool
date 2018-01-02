using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TP_Inventory;

public class TPInventorySaveLoadEditor : ScriptlessEditor
{
    //TPInventorySaveLoad InventorySaveLoad;

    //void OnEnable()
    //{
    //    InventorySaveLoad = target as TPInventorySaveLoad;
    //    InventorySaveLoad.Items = FindAssetsByType<TPItem>().ToArray();
    //}

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Script for Save/Load");
        base.OnInspectorGUI();
    }
}
