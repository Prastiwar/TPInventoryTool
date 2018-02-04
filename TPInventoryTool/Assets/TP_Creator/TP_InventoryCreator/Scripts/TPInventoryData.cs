using System.Collections.Generic;
using TP.Inventory;
using UnityEngine;

namespace TP.InventoryEditor
{
    public class TPInventoryData : ScriptableObject
    {
        public List<TPItem> Items;
        public List<TPStat> Stats;
        public List<TPType> Types;

#if UNITY_EDITOR
        void OnEnable()
        {
            Refresh();
        }
        public void Refresh()
        {
            Items = TPHelper.FindAssetsByType<TPItem>();
            Stats = TPHelper.FindAssetsByType<TPStat>();
            Types = TPHelper.FindAssetsByType<TPType>();
        }
#endif
    }
}