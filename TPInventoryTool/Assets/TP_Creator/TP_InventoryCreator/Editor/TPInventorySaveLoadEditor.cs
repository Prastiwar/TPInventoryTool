using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TP_Inventory;

[CustomEditor(typeof(TPInventorySaveLoad))]
public class TPInventorySaveLoadEditor : ScriptlessEditor
{
    TPInventorySaveLoad InventorySaveLoad;

    void OnEnable()
    {
        InventorySaveLoad = target as TPInventorySaveLoad;

        if (InventorySaveLoad.inventoryData == null)
        {
            InventorySaveLoad.inventoryData =
                (TPInventoryData)AssetDatabase.LoadAssetAtPath("Assets/TP_Creator/TP_InventoryCreator/Demo/InventoryData.asset", typeof(TPInventoryData));
        }
        if (InventorySaveLoad.inventoryData != null)
        {
            InventorySaveLoad.Items = InventorySaveLoad.inventoryData.Items.ToArray();
            InventorySaveLoad.Stats = InventorySaveLoad.inventoryData.Stats.ToArray();
            InventorySaveLoad.Types = InventorySaveLoad.inventoryData.Types.ToArray();
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Script for Save/Load");
    
        base.OnInspectorGUI();

        serializedObject.ApplyModifiedProperties();
    }
}
