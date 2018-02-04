using UnityEngine;

namespace TP.Inventory
{
    public class TPStat : ScriptableObject
    {
        public float Value;

        public delegate void OnChange();
        public OnChange BeforeChange;
        public OnChange AfterChange;
    }
}