using System.Collections.Generic;
using TP_Inventory;
using UnityEngine;

namespace TP_InventoryEditor
{
    public class TPInventoryData : ScriptableObject
    {
        public List<TPItem> Items;
        public List<TPStat> Stats;
        public List<TPType> Types;
        public bool isSaving;

        void OnEnable()
        {
            Items = TPHelper.FindAssetsByType<TPItem>();
            Stats = TPHelper.FindAssetsByType<TPStat>();
            Types = TPHelper.FindAssetsByType<TPType>();
        }
    }
}