﻿using UnityEngine;

namespace TP_Inventory
{
    //[CreateAssetMenu(menuName = "TP_InventoryTool/Item", fileName = "New Item")]
    public class TPItem : ScriptableObject
    {
        [HideInInspector]
        public int ID;

        public Sprite Sprite;
        public string Name;
        public string Description;
        public float Worth;
        public TPType Type;
        public TPModifier[] Modifiers;
        public TPSound[] Sounds;
    }

}