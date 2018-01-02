using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TP_Inventory_Item;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TP_Inventory_Slot
{
    [RequireComponent(typeof(EventTrigger))]
    [RequireComponent(typeof(CanvasGroup))]
    public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler
    {
        InventoryCreator inventoryCreator;
        Transform actualTransform;
        Vector2 basePosition;
        CanvasGroup canvasGroup;
        Image Image;
        Item _item;
        UnityAction onPointerEnterAction;

        public Type Type;
        public bool isEquipSlot;

        public Item Item
        {
            get { return _item; }
            set { _item = value; RefreshItem(); }
        }

        void OnValidate()
        {
            if (Image == null) Image = GetComponent<Image>();
            if (inventoryCreator == null) inventoryCreator = FindObjectOfType<InventoryCreator>();
            if (actualTransform == null) actualTransform = GetComponent<Transform>();
            if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();

            RefreshItem();
        }

        void RefreshItem()
        {
            if (Image != null)
            {
                Image.enabled = true;
                if (_item != null)
                {
                    Image.sprite = _item.Sprite;
                    Image.color = Color.white;
                }
                else
                {
                    Image.sprite = null;
                    Image.color = Color.clear;
                }
            }
        }

        public int Save()
        {
            if (Item != null)
                return Item.ID;
            else
                return -1;
        }

        public void Load(int saved)
        {
            if (saved == -1)
                return;

            int length = inventoryCreator.inventorySaveLoad.Items.Length;
            for (int i = 0; i < length; i++)
            {
                if (inventoryCreator.inventorySaveLoad.Items[i].ID == saved)
                    Item = inventoryCreator.inventorySaveLoad.Items[i];
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            AutoEquip();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            basePosition = actualTransform.position;
            canvasGroup.blocksRaycasts = false;
        }
        public void OnDrag(PointerEventData eventData)
        {
            actualTransform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (eventData.pointerEnter != null)
            {
                Slot slot = eventData.pointerEnter.GetComponent<Slot>();
                if (slot != null)
                {
                    // Jeśli to na co patrzysz jest equipem, to sprawdz czy ma ten sam typ co trzymany item
                    // Jeśli to na co patrzysz nie jest equipem to sprawdź, czy ma ten sam typ lub nie ma typu co trzymany item
                    if (slot.isEquipSlot ? Item.Type == slot.Type : (Item.Type == slot.Type || slot.Type == null))
                    {
                        if (slot.Item != null)
                        {
                            // Sprawdź, czy trzymany item jest Equip slotem
                            // Jeśli tak, to sprawdź czy item na patrzonym slocie jest tego typu co trzymany item.
                            if (isEquipSlot ? slot.Item.Type == Item.Type : true)
                            {
                                var tempItem = slot.Item;
                                slot.Item = Item;
                                Item = tempItem;
                            }
                        }
                        else
                        {
                            slot.Item = Item;
                            Item = null;
                        }
                    }
                }
            }
            canvasGroup.blocksRaycasts = true;
            actualTransform.position = basePosition;
            basePosition = Vector2.zero;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData != null)
            {
                if (onPointerEnterAction != null)
                    OnPointerEnter();
            }
        }
        void OnPointerEnter()
        {
            onPointerEnterAction();
        }
        public void SetPointerEnter(UnityAction _action)
        {
            onPointerEnterAction = _action;
        }

        void AutoEquip()
        {
            var slots = inventoryCreator.slots;
            int _length = slots.Count;

            for (int i = 0; i < _length; i++)
            {
                // If you click on equip slot, it will find no-equip slot
                if (isEquipSlot ? !slots[i].isEquipSlot : slots[i].isEquipSlot)
                {
                    //If you click on equip slot, it will find matching Type (or null)
                    if (isEquipSlot ? ((slots[i].Type == Item.Type || slots[i].Type == null) && slots[i].Item == null) : (slots[i].Type == Item.Type))
                    {
                        int _Modlength = Item.Modifiers.Length;
                        for (int j = 0; j < _Modlength; j++)
                        {
                            Item.Modifiers[j].Modify();
                        }
                        slots[i].Item = Item;
                        Item = null;
                        break;
                    }
                }
            }
        }

    }
}