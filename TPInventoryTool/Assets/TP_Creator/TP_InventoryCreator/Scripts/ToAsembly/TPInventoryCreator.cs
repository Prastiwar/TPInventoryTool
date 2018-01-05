using System.Collections.Generic;
using UnityEngine;

namespace TP_Inventory
{
    [RequireComponent(typeof(TPInventoryPersistance))]
    public class TPInventoryCreator : MonoBehaviour
    {
        [Header("Put there PARENT of all slot's PARENTS")]
        [HideInInspector] public Transform slotParentsTransform = null;
        [HideInInspector] public TPInventoryPersistance InventoryPersistance;
        [HideInInspector] public List<TPSlot> Slots = new List<TPSlot>();

        void OnValidate()
        {
            if (InventoryPersistance == null) InventoryPersistance = GetComponent<TPInventoryPersistance>();
            RefreshSlots();
        }

        public void RefreshSlots()
        {
            if (slotParentsTransform != null)
            {
                Slots.Clear();
                foreach (Transform trans in slotParentsTransform)
                {
                    Transform child = trans;
                    foreach (Transform slot in child)
                    {
                        if (slot.GetComponent<TPSlot>())
                            Slots.Add(slot.GetComponent<TPSlot>());
                        else
                            Slots.Add(slot.gameObject.AddComponent<TPSlot>());
                    }
                }
            }
        }

        void Awake()
        {
            if (InventoryPersistance.inventoryData.isSaving)
                InventoryPersistance.Load();
        }

        public void OnApplicationPause(bool pause)
        {
            if (InventoryPersistance.inventoryData.isSaving)
                InventoryPersistance.Save();
        }
        public void OnApplicationQuit()
        {
            if (InventoryPersistance.inventoryData.isSaving)
                InventoryPersistance.Save();
        }

    }
}