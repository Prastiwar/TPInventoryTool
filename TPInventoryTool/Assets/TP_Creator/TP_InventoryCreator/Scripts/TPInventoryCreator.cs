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

        enum Finder
        {
            EquipSlot,
            EquipSlotWithType,
            NoEquipSlot,
            NoEquipSlotWithType
        }

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

        public bool IsFullType(TPType type)
        {
            return FreeSlotsLength(type) != 0 ? false : true;
        }

        public int FreeSlotsTypeLength(TPType type)
        {
            return FreeSlotsLength(type);
        }

        public int FreeSlotsLength()
        {
            int freeSlots = 0;
            int length = Data.Types.Count;
            for (int i = 0; i < length; i++)
            {
                freeSlots += FreeSlotsLength(Data.Types[i]);
            }
            return freeSlots;
        }

        int FreeSlotsLength(TPType type)
        {
            int freeSlots = 0;
            int length = Slots.Count;
            for (int i = 0; i < length; i++)
            {
                if (Slots[i].Item == null && Slots[i].Type == type)
                    freeSlots++;
            }
            return freeSlots;
        }

        //public TPSlot FindFreeSlot()
        //{
        //    bool boolean = Slots[indexer].Item == null;
        //    return FindSlot(boolean);
        //}

        //public TPSlot FindFreeSlotWithType(TPType type)
        //{
        //    bool boolean = Slots[indexer].Item == null && Slots[indexer].Type == type;
        //    return FindSlot(boolean);
        //}

        public TPSlot FindFreeEquipSlot()
        {
            return FindSlot(Finder.EquipSlot, null);
        }
        public TPSlot FindFreeEquipSlotWithType(TPType type)
        {
            return FindSlot(Finder.EquipSlotWithType, type);
        }

        public TPSlot FindFreeNoEquipSlot()
        {
            return FindSlot(Finder.NoEquipSlot, null);
        }
        public TPSlot FindFreeNoEquipSlotWithType(TPType type)
        {
            return FindSlot(Finder.NoEquipSlotWithType, type);
        }

        bool GetFindBool(Finder finder, TPType type, int indexer)
        {
            bool boolean = false;

            switch (finder)
            {
                case Finder.EquipSlot:
                    boolean = Slots[indexer].Item == null &&
                        Slots[indexer].IsEquipSlot && Slots[indexer].Type == null;
                    break;
                case Finder.EquipSlotWithType:
                    boolean = Slots[indexer].Item == null &&
                        Slots[indexer].IsEquipSlot && Slots[indexer].Type == type;
                    break;
                case Finder.NoEquipSlot:
                    boolean = Slots[indexer].Item == null &&
                        !Slots[indexer].IsEquipSlot && Slots[indexer].Type == null;
                    break;
                case Finder.NoEquipSlotWithType:
                    boolean = Slots[indexer].Item == null &&
                        !Slots[indexer].IsEquipSlot && Slots[indexer].Type == type;
                    break;
                default:
                    break;
            }

            return boolean;
        }

        TPSlot FindSlot(Finder finder, TPType type)
        {
            TPSlot freeSlot = null;
            int length = Slots.Count;
            for (int i = 0; i < length; i++)
            {
                if (GetFindBool(finder, type, i))
                {
                    freeSlot = Slots[i];
                    return freeSlot;
                }
            }
            return freeSlot;
        }
    }
}