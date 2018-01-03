using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TP_Inventory
{
    public class TPInventoryCreator : MonoBehaviour
    {
        [Header("Put there PARENT of all slot's PARENTS")]
        [HideInInspector] public Transform slotTransform = null;

        bool isSaving = false;
        [HideInInspector] public TPInventoryPersistance InventoryPersistance;
        [HideInInspector] public List<TPSlot> Slots = new List<TPSlot>();

        void OnValidate()
        {
            if (InventoryPersistance == null) InventoryPersistance = GetComponent<TPInventoryPersistance>();

            if (slotTransform != null)
            {
                Slots.Clear();
                foreach (Transform trans in slotTransform)
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
            if (isSaving)
                InventoryPersistance.Load();
        }

        public void OnApplicationPause(bool pause)
        {
            if (isSaving)
                InventoryPersistance.Save();
        }
        public void OnApplicationQuit()
        {
            if (isSaving)
                InventoryPersistance.Save();
        }

    }
}