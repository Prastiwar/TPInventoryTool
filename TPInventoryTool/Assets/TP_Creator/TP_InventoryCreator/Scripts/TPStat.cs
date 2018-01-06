using UnityEngine;

namespace TP_Inventory
{
    public class TPStat : ScriptableObject
    {
        public float Value;
        [HideInInspector] public bool HasChanged = false;

        //public float Save()
        //{
        //    return Value;
        //}
        //public void Load(float saved)
        //{
        //    Value = saved;
        //}
    }
}