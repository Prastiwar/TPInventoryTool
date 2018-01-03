using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TPModifier
{
    public enum ModifierType
    {
        Flat,
        Percentage
    }
    public enum ModifierCommand
    {
        Increase,
        Decrease
    }
    public TPStat Stat;
    public ModifierType ModifyType;
    public ModifierCommand ModifyCommand;
    public float Value;
    bool isModified = false;
    float tempValue;

    public void Modify()
    {
        switch (ModifyCommand)
        {
            case ModifierCommand.Increase:
                if (!isModified) // Modify
                {
                    if (ModifyType == ModifierType.Percentage)
                        tempValue = Mathf.CeilToInt(Stat.Value / Value);
                    Stat.Value = ModifyType == ModifierType.Flat ? Stat.Value += Value : Stat.Value += tempValue;
                }
                else // Undo
                {
                    Stat.Value = ModifyType == ModifierType.Flat ? Stat.Value -= Value : Stat.Value -= tempValue;
                }
                break;

            case ModifierCommand.Decrease:
                if (!isModified) // Modify
                {
                    if (ModifyType == ModifierType.Percentage)
                        tempValue = Mathf.CeilToInt(Stat.Value / Value);
                    Stat.Value = ModifyType == ModifierType.Flat ? Stat.Value -= Value : Stat.Value -= tempValue;
                }
                else // Undo
                {
                    Stat.Value = ModifyType == ModifierType.Flat ? Stat.Value += Value : Stat.Value += tempValue;
                }
                break;
        }
        isModified = !isModified;
    }
}
