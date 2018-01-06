using UnityEngine;

namespace TP_Inventory
{
    public class TPItem : ScriptableObject
    {
        [SerializeField] int _ID;
        public int ID { get { return _ID; } }

        public Sprite Sprite;
        public string Name;
        public string Description;
        public float Worth;
        public TPType Type;
        public TPModifier[] Modifiers;
        public TPSound[] Sounds;
        public TPSlot OnSlot { get { return slot; } set { slot = value; } }

        TPSlot slot;
    }

}