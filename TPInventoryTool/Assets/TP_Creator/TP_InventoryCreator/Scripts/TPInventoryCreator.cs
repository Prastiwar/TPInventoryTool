using System.Collections.Generic;
using TP.InventoryEditor;
using UnityEngine;

namespace TP.Inventory
{
    public class TPInventoryCreator : MonoBehaviour
    {
        public static bool DebugMode;
        public Transform SlotParentsTransform = null;
        public TPInventoryData Data;
        public List<TPSlot> Slots = new List<TPSlot>();

        public delegate void OnInventoryChangeHandler();
        OnInventoryChangeHandler OnBeforeAddItem;
        OnInventoryChangeHandler OnAfterAddItem;
        OnInventoryChangeHandler OnBeforeRemoveItem;
        OnInventoryChangeHandler OnAfterRemoveItem;

        enum Finder
        {
            FreeEquipSlot,
            FreeEquipSlotWithType,
            FreeNoEquipSlot,
            FreeNoEquipSlotWithType,
            AnyFree,
            AnyFreeWithType,

            AnyWithItem,
            EquipWithItem,
            NoEquipWithItem
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

        public void SetOnBeforeAddItem(OnInventoryChangeHandler _OnBeforeAddItem)
        {
            OnBeforeAddItem = _OnBeforeAddItem;
        }
        public void SetOnAfterAddItem(OnInventoryChangeHandler _OnAfterAddItem)
        {
            OnAfterAddItem = _OnAfterAddItem;
        }

        public void SetOnBeforeRemoveItem(OnInventoryChangeHandler _OnBeforeRemoveItem)
        {
            OnBeforeRemoveItem = _OnBeforeRemoveItem;
        }
        public void SetOnAfterRemoveItem(OnInventoryChangeHandler _OnAfterRemoveItem)
        {
            OnAfterRemoveItem = _OnAfterRemoveItem;
        }

        public void AddItem(TPItem item)
        {
            if(OnBeforeAddItem != null)
                OnBeforeAddItem();

            FindAnyFreeSlotWithType(item.Type).Item = item;

            if (OnAfterAddItem != null)
                OnAfterAddItem();
        }
        public void AddItem(TPSlot slot, TPItem item)
        {
            if (OnBeforeAddItem != null)
                OnBeforeAddItem();

            if (slot.Type == item.Type
                || slot.Type == null)
            {
                slot.Item = item;
            }
            else
            {
                Debug.LogError("You're trying to add Item to Slot with different types!");
            }

            if (OnAfterAddItem != null)
                OnAfterAddItem();
        }

        public void RemoveItem(TPItem item)
        {
            FindAnySlotWith(item).Item = null;
        }
        public void RemoveItem(TPSlot slot)
        {
            if (OnAfterRemoveItem != null)
                OnAfterRemoveItem();

            slot.Item = null;

            if (OnAfterRemoveItem != null)
                OnAfterRemoveItem();
        }
        
        public bool IsFull()
        {
            return FreeSlotsLength() != 0 ? false : true;
        }

        public bool IsFullOfType(TPType type)
        {
            return FreeSlotsLength(type) != 0 ? false : true;
        }

        public int FreeSlotsOfTypeLength(TPType type)
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

        public TPSlot FindAnySlotWith(TPItem item)
        {
            return FindSlot(Finder.AnyWithItem, null, item);
        }
        public TPSlot FindEquipSlotWith(TPItem item)
        {
            return FindSlot(Finder.EquipWithItem, null, item);
        }
        public TPSlot FindNoEquipSlotWith(TPItem item)
        {
            return FindSlot(Finder.NoEquipWithItem, null, item);
        }

        public TPSlot FindAnyFreeSlot()
        {
            return FindSlot(Finder.AnyFree, null, null);
        }
        public TPSlot FindAnyFreeSlotWithType(TPType type)
        {
            return FindSlot(Finder.AnyFreeWithType, type, null);
        }

        public TPSlot FindFreeEquipSlot()
        {
            return FindSlot(Finder.FreeEquipSlot, null, null);
        }
        public TPSlot FindFreeEquipSlotWithType(TPType type)
        {
            return FindSlot(Finder.FreeEquipSlotWithType, type, null);
        }

        public TPSlot FindFreeNoEquipSlot()
        {
            return FindSlot(Finder.FreeNoEquipSlot, null, null);
        }
        public TPSlot FindFreeNoEquipSlotWithType(TPType type)
        {
            return FindSlot(Finder.FreeNoEquipSlotWithType, type, null);
        }

        bool GetFindBool(Finder finder, TPType type, TPItem item, int indexer)
        {
            bool boolean = false;

            switch (finder)
            {
                case Finder.FreeEquipSlot:
                    boolean = Slots[indexer].Item == null &&
                        Slots[indexer].IsEquipSlot && Slots[indexer].Type == null;
                    break;
                case Finder.FreeEquipSlotWithType:
                    boolean = Slots[indexer].Item == null &&
                        Slots[indexer].IsEquipSlot && Slots[indexer].Type == type;
                    break;
                case Finder.FreeNoEquipSlot:
                    boolean = Slots[indexer].Item == null &&
                        !Slots[indexer].IsEquipSlot && Slots[indexer].Type == null;
                    break;
                case Finder.FreeNoEquipSlotWithType:
                    boolean = Slots[indexer].Item == null &&
                        !Slots[indexer].IsEquipSlot && Slots[indexer].Type == type;
                    break;
                case Finder.AnyFree:
                    boolean = Slots[indexer].Item == null;
                    break;
                case Finder.AnyFreeWithType:
                    boolean = Slots[indexer].Item == null &&
                        Slots[indexer].Type == type;
                    break;

                case Finder.AnyWithItem:
                    boolean = Slots[indexer].Item == item;
                    break;
                case Finder.EquipWithItem:
                    boolean = Slots[indexer].Item == item &&
                        Slots[indexer].IsEquipSlot;
                    break;
                case Finder.NoEquipWithItem:
                    boolean = Slots[indexer].Item == item &&
                        !Slots[indexer].IsEquipSlot;
                    break;
                default:
                    break;
            }

            return boolean;
        }

        TPSlot FindSlot(Finder finder, TPType type, TPItem item)
        {
            int length = Slots.Count;
            for (int i = 0; i < length; i++)
            {
                if (GetFindBool(finder, type, item, i))
                {
                    return Slots[i];
                }
            }
            return null;
        }
    }
}