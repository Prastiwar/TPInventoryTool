using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TP_Inventory;
using TP_Inventory_Item;
using TP_Inventory_Slot;

public class InventoryCreator : MonoBehaviour
{
    [Header("Put there PARENT of all slots")]
    [SerializeField] Transform slotTransform;
    [Header("Slots will automatically load")]
    [SerializeField] List<Slot> slots = new List<Slot>();
    [Header("Put there your custom items")]
    [SerializeField] List<Item> items = new List<Item>();

    void OnValidate()
    {
        if (slotTransform != null && slots.Count < slotTransform.childCount)
        {
            foreach (Transform slot in slotTransform)
            {
                Transform child = slot;
                foreach (Transform slott in child)
                {
                    slots.Add(slott.gameObject.AddComponent<Slot>());
                }
            }
        }

        RefreshUI();
    }

    void RefreshUI()
    {
        int _itemsLength = items.Count;
        int _slotsLength = slots.Count;
        int i;
        for (i = 0; i < _itemsLength && i < _slotsLength; i++)
        {
            slots[i].Item = items[i];
        }
        for (; i < _slotsLength; i++)
        {
            slots[i].Item = null;
        }
    }
}
