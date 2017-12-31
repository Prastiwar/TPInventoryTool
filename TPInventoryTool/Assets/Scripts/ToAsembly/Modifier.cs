using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Modifier
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
    public Stat Stat;
    public ModifierType ModifyType;
    public ModifierCommand ModifyCommand;
    public float Value;
    float tempValue;

    public void Modify()
    {
        tempValue = Value;
        switch (ModifyCommand)
        {
            case ModifierCommand.Increase:
                Stat.Value = ModifyType == ModifierType.Flat ? Stat.Value += Value : Stat.Value += (Stat.Value /= Value);
                break;

            case ModifierCommand.Decrease:
                Stat.Value = ModifyType == ModifierType.Flat ? Stat.Value -= Value : Stat.Value /= Value;
                break;
        }
    }

    public void Undo()
    {
        Value = tempValue;
    }
}
