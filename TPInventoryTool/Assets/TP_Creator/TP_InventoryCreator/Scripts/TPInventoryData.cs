using System.Collections.Generic;
using TP.Inventory;
using TP.Utilities;
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
            Items = TPFind.FindAssetsByType<TPItem>();
            Stats = TPFind.FindAssetsByType<TPStat>();
            Types = TPFind.FindAssetsByType<TPType>();
        }
#endif
    }
}