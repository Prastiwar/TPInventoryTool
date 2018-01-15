using UnityEngine;

namespace TP_Inventory
{
    public class TPStat : ScriptableObject
    {
        public float Value;

        public delegate void OnChange();
        public OnChange BeforeChange;
        public OnChange AfterChange;
    }
}