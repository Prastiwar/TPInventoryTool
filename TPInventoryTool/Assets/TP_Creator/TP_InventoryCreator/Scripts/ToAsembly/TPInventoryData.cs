using System.Collections.Generic;
using UnityEngine;

namespace TP_Inventory
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