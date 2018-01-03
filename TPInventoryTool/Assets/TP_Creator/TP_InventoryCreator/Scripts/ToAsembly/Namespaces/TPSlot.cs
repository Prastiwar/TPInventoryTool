using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TP_Inventory
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(Image))]
    public class TPSlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler
    {
        TPInventoryCreator inventoryCreator;
        Transform actualTransform;
        Vector2 basePosition;
        CanvasGroup canvasGroup;
        Image Image;
        TPItem _item;
        UnityAction onPointerEnterAction;

        public TPType Type;
        public bool isEquipSlot;

        public TPItem Item
        {
            get { return _item; }
            set { _item = value; RefreshItemUI(); }
        }

        void OnValidate()
        {
            if (Image == null) Image = GetComponent<Image>();
            if (inventoryCreator == null) inventoryCreator = FindObjectOfType<TPInventoryCreator>();
            if (actualTransform == null) actualTransform = GetComponent<Transform>();
            if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();

            RefreshItemUI();
        }

        void RefreshItemUI()
        {
            if (!Image.enabled)
                Image.enabled = true;
            Image.sprite = Item != null ? Item.Sprite : null;
            Image.color = Item != null ? Color.white : Color.clear;
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

            int length = inventoryCreator.InventorySaveLoad.Items.Length;
            for (int i = 0; i < length; i++)
            {
                if (inventoryCreator.InventorySaveLoad.Items[i].ID == saved)
                    Item = inventoryCreator.InventorySaveLoad.Items[i];
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Item != null)
            {
                AutoEquip();
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (Item != null)
            {
                basePosition = actualTransform.position;
                canvasGroup.blocksRaycasts = false;
            }
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (Item != null)
            {
                actualTransform.position = eventData.position;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (eventData.pointerEnter != null && Item != null)
            {
                TPSlot slot = eventData.pointerEnter.GetComponent<TPSlot>();
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
            if (eventData != null && Item != null)
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
            var slots = inventoryCreator.Slots;
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