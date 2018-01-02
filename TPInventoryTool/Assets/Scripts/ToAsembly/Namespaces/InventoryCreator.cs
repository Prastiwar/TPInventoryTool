using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TP_Inventory;
using TP_Inventory_Item;
using TP_Inventory_Slot;

public class InventoryCreator : MonoBehaviour
{
    [Header("Put there PARENT of all slot's PARENTS")]
    [SerializeField] Transform slotTransform;
    [Header("Do you want to save/load inventory?")]
    [SerializeField] bool isSaving;

    [HideInInspector] public InventorySaveLoad inventorySaveLoad;
    [HideInInspector] public List<Slot> slots = new List<Slot>();

    void OnValidate()
    {
        if (inventorySaveLoad == null) GetComponent<InventorySaveLoad>();

        if (slotTransform != null)
        {
            slots.Clear();
            foreach (Transform trans in slotTransform)
            {
                Transform child = trans;
                foreach (Transform slot in child)
                {
                    if (slot.GetComponent<Slot>())
                        slots.Add(slot.GetComponent<Slot>());
                    else
                        slots.Add(slot.gameObject.AddComponent<Slot>());
                }
            }
            inventorySaveLoad.Slots = slots;
        }
    }

    void Awake()
    {
        if (isSaving)
            inventorySaveLoad.Load();
    }

    public void OnApplicationPause(bool pause)
    {
        if(isSaving)
            inventorySaveLoad.Save();
    }
    public void OnApplicationQuit()
    {
        if (isSaving)
            inventorySaveLoad.Save();
    }

}