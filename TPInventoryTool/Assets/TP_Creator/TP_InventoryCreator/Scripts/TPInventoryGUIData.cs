using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TP_InventoryEditor
{
    public class TPInventoryGUIData : ScriptableObject
    {
        [HideInInspector] public GUISkin GUISkin;
        [HideInInspector] public string InventoryDataPath;
        [HideInInspector] public string InventoryAssetsPath;
        [HideInInspector] public GameObject InventoryPrefab;
    }
}