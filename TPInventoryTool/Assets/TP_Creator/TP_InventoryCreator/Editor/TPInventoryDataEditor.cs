using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TP_Inventory;

[CustomEditor(typeof(TPInventoryData))]
public class TPInventoryDataEditor : Editor
{
    TPInventoryData InventoryData;

    void OnEnable()
    {
        InventoryData = target as TPInventoryData;

        //InventoryData.Items = TPHelper.FindAssetsByType<TPItem>();
        //InventoryData.Stats = TPHelper.FindAssetsByType<TPStat>();
        //InventoryData.Types = TPHelper.FindAssetsByType<TPType>();
        
    }

}