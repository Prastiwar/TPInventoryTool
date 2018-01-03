using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TP_Inventory;

[CustomEditor(typeof(TPInventoryPersistance))]
public class TPInventoryPersistanceEditor : ScriptlessEditor
{
    TPInventoryPersistance InventorySaveLoad;

    void OnEnable()
    {
        InventorySaveLoad = target as TPInventoryPersistance;

        if (InventorySaveLoad.inventoryData == null)
        {
            InventorySaveLoad.inventoryData =
                (TPInventoryData)AssetDatabase.LoadAssetAtPath("Assets/TP_Creator/TP_InventoryCreator/InventoryData/InventoryData.asset", typeof(TPInventoryData));
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
