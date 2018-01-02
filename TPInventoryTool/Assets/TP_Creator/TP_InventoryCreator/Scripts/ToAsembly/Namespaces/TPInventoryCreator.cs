using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TP_Inventory
{
    public class TPInventoryCreator : MonoBehaviour
    {
        [Header("Put there PARENT of all slot's PARENTS")]
        [HideInInspector] public Transform slotTransform = null;

        [HideInInspector] public bool isSaving = false;
        [HideInInspector] public TPInventorySaveLoad InventorySaveLoad;
        [HideInInspector] public List<TPSlot> Slots = new List<TPSlot>();

        void OnValidate()
        {
            if (InventorySaveLoad == null) InventorySaveLoad = GetComponent<TPInventorySaveLoad>();

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
                InventorySaveLoad.Slots = Slots;
            }
        }

        void Awake()
        {
            if (isSaving)
                InventorySaveLoad.Load();
        }

        public void OnApplicationPause(bool pause)
        {
            if (isSaving)
                InventorySaveLoad.Save();
        }
        public void OnApplicationQuit()
        {
            if (isSaving)
                InventorySaveLoad.Save();
        }

    }
}