using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TP.InventoryEditor
{
    public class TPInventoryGUIData : ScriptableObject
    {
        public GUISkin GUISkin;
        public string InventoryDataPath;
        public string InventoryAssetsPath;
        public GameObject InventoryPrefab;
    }
}