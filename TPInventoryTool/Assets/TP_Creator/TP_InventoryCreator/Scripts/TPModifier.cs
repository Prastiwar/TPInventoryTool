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

        string NullStat = "You don't have Modifier reference in this item!";

        public void Modify()
        {
            if (Stat == null)
            {
                Debug.LogError(NullStat);
                return;
            }

            BeforeChange();

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

            AfterChange();
        }

        public void UnModify()
        {
            if (Stat == null)
            {
                Debug.LogError(NullStat);
                return;
            }

            BeforeChange();

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

            AfterChange();
        }

        void BeforeChange()
        {
            if (Stat.BeforeChange != null)
                Stat.BeforeChange();
        }

        void AfterChange()
        {
            if (Stat.AfterChange != null)
                Stat.AfterChange();
        }
    }

}