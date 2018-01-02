using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TP_InventoryTool/Stat", fileName = "New Stat")]
public class Stat : ScriptableObject
{
    [Header("Statistic Property")]
    public float Value;

    public float Save()
    {
        return Value;
    }

    public void Load(float saved)
    {
        Value = saved;
    }
}