using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TP_Inventory_Item;

namespace TP_Inventory_Slot
{
    public class Slot : MonoBehaviour
    {
        Image Image;
        Item _item;
        public TypeEnum type;

        public Item Item { get { return _item; } set{
                _item = value;
                if (_item != null)
                {
                    Image.enabled = true;
                    Image.sprite = _item.Sprite;
                }
                else
                    Image.enabled = false;
            }}

        void OnValidate()
        {
            if (Image == null)
                Image = GetComponent<Image>();
        }
    }
}