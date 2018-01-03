using System.Collections;
using System.Collections.Generic;
using TP_Inventory;
using UnityEngine;

[CreateAssetMenu(menuName = "TP_InventoryTool/data", fileName = "New data")]
public class TPInventoryData : ScriptableObject
{
    public List<TPItem> Items;
    public List<TPStat> Stats;
    public List<TPType> Types;
    //slots
    
}
