using System.Collections;
using UnityEngine;

namespace TP_Inventory
{
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
        float tempValue;
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

        string NullStat = "You don't have Modifier reference in this item!";

        public void Modify()
        {
            if (Stat == null)
            {
                Debug.LogError(NullStat);
                return;
            }
            switch (ModifyCommand)
            {
                case ModifierCommand.Increase:
                        if (ModifyType == ModifierType.Percentage)
                            tempValue = Mathf.CeilToInt((Value / Stat.Value) * 100);
                        Stat.Value = ModifyType == ModifierType.Flat ? Stat.Value += Value : Stat.Value += tempValue;
                    break;

                case ModifierCommand.Decrease:
                        if (ModifyType == ModifierType.Percentage)
                            tempValue = Mathf.CeilToInt((Value / Stat.Value) * 100);
                        Stat.Value = ModifyType == ModifierType.Flat ? Stat.Value -= Value : Stat.Value -= tempValue;
                    break;
            }
        }

        public void UnModify()
        {
            if (Stat == null)
            {
                Debug.LogError(NullStat);
                return;
            }
            switch (ModifyCommand)
            {
                case ModifierCommand.Increase:
                    Stat.Value = ModifyType == ModifierType.Flat ? Stat.Value -= Value : Stat.Value -= tempValue;
                    break;

                case ModifierCommand.Decrease:
                    Stat.Value = ModifyType == ModifierType.Flat ? Stat.Value += Value : Stat.Value += tempValue;
                    break;

                default:
                    break;
            }
        }

        public IEnumerator StatValueChanged()
        {
            Stat.HasChanged = true;
            yield return waitForEndOfFrame;
            Stat.HasChanged = false;
        }
    }

}