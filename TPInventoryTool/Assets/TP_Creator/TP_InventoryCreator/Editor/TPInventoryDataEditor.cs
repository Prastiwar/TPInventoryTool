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

        //foreach (var item in TPHelper.FindAssetsByType<TPType>())
        //{
        //    EditorUtility.SetDirty(item);
        //}
        //foreach (var item in TPHelper.FindAssetsByType<TPItem>())
        //{
        //    EditorUtility.SetDirty(item);
        //}
        //foreach (var item in TPHelper.FindAssetsByType<TPStat>())
        //{
        //    EditorUtility.SetDirty(item);
        //}
        //AssetDatabase.LoadAssetAtPath("Assets/TP_Creator/TP_InventoryCreator/Demo/New Type.asset", typeof(TPType));
        //AssetDatabase.SaveAssets();

        InventoryData.Items = TPHelper.FindAssetsByType<TPItem>();
        InventoryData.Stats = TPHelper.FindAssetsByType<TPStat>();
        InventoryData.Types = TPHelper.FindAssetsByType<TPType>();
    }

}