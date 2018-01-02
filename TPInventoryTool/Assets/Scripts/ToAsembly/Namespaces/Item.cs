using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TP_Inventory_Item
{
    [CreateAssetMenu(menuName = "TP_InventoryTool/Item", fileName = "New Item")]
    public class Item : ScriptableObject
    {
        [HideInInspector]
        public int ID;
        public Sprite Sprite;
        public string Name;
        public string Description;
        public float Value;
        public Type Type;
        public Modifier[] Modifiers;
        public Sound[] Sounds;

        public void Save()
        {

        }
    }

}