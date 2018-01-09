using System.Collections.Generic;
using TP_InventoryEditor;
using UnityEngine;

namespace TP_Inventory
{
    public class TPInventoryCreator : MonoBehaviour
    {
        [HideInInspector] public Transform SlotParentsTransform = null;
        [HideInInspector] public List<TPSlot> Slots = new List<TPSlot>();
        [HideInInspector] public TPInventoryData Data;

        void OnValidate()
        {
            RefreshSlots();
#if UNITY_EDITOR
            FindData();
#endif
        }

#if UNITY_EDITOR
        void FindData()
        {
            TPInventoryGUIData guiData = (TPInventoryGUIData)UnityEditor.AssetDatabase.LoadAssetAtPath(
                    "Assets/TP_Creator/TP_InventoryCreator/EditorResources/InventoryEditorGUIData.asset",
                    typeof(TPInventoryGUIData));
            if (guiData != null)
                Data = (TPInventoryData)UnityEditor.AssetDatabase.LoadAssetAtPath(
                    "Assets/" + guiData.InventoryDataPath + "InventoryData.asset", typeof(TPInventoryData));
        }
#endif
        void Awake()
        {
            RefreshSlots();
        }

        public void RefreshSlots()
        {
            if (SlotParentsTransform != null)
            {
                Slots.Clear();
                foreach (Transform trans in SlotParentsTransform)
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

        public bool IsFull()
        {
            return FreeSlotsLength() != 0 ? false : true;
        }

        public int FreeSlotsLength()
        {
            int freeSlots = 0;
            int length = Slots.Count;
            for (int i = 0; i < length; i++)
            {
                if (Slots[i].Item == null)
                    freeSlots++;
            }
            return freeSlots;
        }
    }
}