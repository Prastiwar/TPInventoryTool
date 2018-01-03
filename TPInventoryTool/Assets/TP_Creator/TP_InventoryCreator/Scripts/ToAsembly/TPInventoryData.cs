using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TP_Inventory
{
    //[CreateAssetMenu(menuName = "TP_InventoryTool/data", fileName = "New data")]
    public class TPInventoryData : ScriptableObject
    {
        [SerializeField] public List<TPItem> Items;
        [SerializeField] public List<TPStat> Stats;
        [SerializeField] public List<TPType> Types;
        //slots

        void OnEnable()
        {
            Items = TPHelper.FindAssetsByType<TPItem>();
            Stats = TPHelper.FindAssetsByType<TPStat>();
            Types = TPHelper.FindAssetsByType<TPType>();

            //slots
        }
    }
}