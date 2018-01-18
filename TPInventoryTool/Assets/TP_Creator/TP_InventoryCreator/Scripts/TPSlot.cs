using UnityEngine;
using UnityEngine.UI;
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

        [HideInInspector] public TPType Type;
        [HideInInspector] public bool IsEquipSlot;
        [HideInInspector] public bool IsSelectable;
        [HideInInspector] public bool IsSelected;
        public delegate void OnSelection();
        OnSelection onSelection;

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

        void Awake()
        {
            if (Image == null) Image = GetComponent<Image>();
            if (inventoryCreator == null) inventoryCreator = FindObjectOfType<TPInventoryCreator>();
            if (actualTransform == null) actualTransform = GetComponent<Transform>();
            if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();

            RefreshItemUI();
        }

        void RefreshItemUI()
        {
            if (Image != null)
            {
                if (!Image.enabled)
                    Image.enabled = true;
                Image.sprite = Item != null ? Item.Sprite : null;
                Image.color = Item != null ? Color.white : Color.clear;
            }
            if(Item != null) Item.OnSlot = this;
        }

        //public int Save()
        //{
        //    if (Item != null)
        //        return Item.ID;
        //    else
        //        return -1;
        //}

        //public void Load(int _saved)
        //{
        //    if (_saved == -1)
        //        return;

        //    int length = inventoryCreator.Data.Items.Count;
        //    for (int i = 0; i < length; i++)
        //    {
        //        if (inventoryCreator.Data.Items[i].ID == _saved)
        //            Item = inventoryCreator.Data.Items[i];
        //    }
        //}

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (Item == null)
                return;

            if (IsSelectable)
            {
                if (!IsSelected && onSelection != null)
                    onSelection();
                if (IsSelected)
                    AutoEquip();

                IsSelected = !IsSelected;
            }
            else
                AutoEquip();

        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            if (Item == null)
                return;

            basePosition = actualTransform.position;
            canvasGroup.blocksRaycasts = false;
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            if (Item == null)
                return;

            actualTransform.position = eventData.position;
        }
        
        public virtual void OnEndDrag(PointerEventData eventData)
        {
            if (Item == null)
                return;

            if (eventData.pointerEnter == null)
            {
                DisableStick();
                return;
            }
            TPSlot slotEntered = eventData.pointerEnter.GetComponent<TPSlot>();
            if (slotEntered == null)
            {
                DisableStick();
                return;
            }

            if (IsEquipSlot)
            {
                if (slotEntered.IsEquipSlot)
                {
                    //Debug.Log("_From equip to equip"); - move
                    if ((slotEntered.Item != null && (slotEntered.Item.Type == Item.Type || (Type == null && slotEntered.Type == null))) ||
                        (slotEntered.Item == null && (slotEntered.Type == Item.Type || slotEntered.Type == null)))
                        ChangeSlot(slotEntered, false);
                    else
                        PlaySound(TPSound.AudioTypeEnum.Failure);
                }
                else
                {
                    //Debug.Log("_From equip to normal"); - remove
                    if((slotEntered.Item != null && (slotEntered.Item.Type == Type || Type == null)) || 
                        (slotEntered.Item == null && (slotEntered.Type == Item.Type || slotEntered.Type == null)))
                        ChangeSlot(slotEntered, true);
                    else
                        PlaySound(TPSound.AudioTypeEnum.Failure);
                } 
            }
            else
            {
                if (slotEntered.IsEquipSlot)
                {
                    //Debug.Log("_From normal to equip"); - wear
                    if ((slotEntered.Item != null && (slotEntered.Item.Type == Item.Type || slotEntered.Type == null)) ||
                        (slotEntered.Item == null && (slotEntered.Type == Item.Type || slotEntered.Type == null)))
                        ChangeSlot(slotEntered, true);
                    else
                        PlaySound(TPSound.AudioTypeEnum.Failure);
                }
                else
                {
                    //Debug.Log("_From normal to normal"); - move
                    if ((slotEntered.Item != null && (slotEntered.Item.Type == Item.Type || (Type == null && slotEntered.Type == null))) ||
                        (slotEntered.Item == null && (slotEntered.Type == Item.Type || slotEntered.Type == null)))
                        ChangeSlot(slotEntered, false);
                    else
                        PlaySound(TPSound.AudioTypeEnum.Failure);
                }
            }
            DisableStick();
        }

        void DisableStick()
        {
            canvasGroup.blocksRaycasts = true;
            actualTransform.position = basePosition;
            basePosition = Vector2.zero;
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (Item == null)
                return;
            
        }

        public void SetOnSelection(OnSelection _OnSelection)
        {
            onSelection = _OnSelection;
        }

        void AutoEquip()
        {
            if (IsEquipSlot)
            {
                // DeEquip item
                if (inventoryCreator.IsFull() || (inventoryCreator.IsFullOfType(null) && inventoryCreator.IsFullOfType(Item.Type)))
                {
                    PlaySound(TPSound.AudioTypeEnum.Failure);
                    return;
                }

                if (Item.Type != null)
                {
                    if (inventoryCreator.FindFreeNoEquipSlotWithType(Item.Type) != null)
                        ChangeSlot(inventoryCreator.FindFreeNoEquipSlotWithType(Item.Type), true);
                    else
                        ChangeSlot(inventoryCreator.FindFreeNoEquipSlot(), true);
                }
                else
                    ChangeSlot(inventoryCreator.FindFreeNoEquipSlot(), true);
            }
            else
            {
                // Equip item
                if (Item.Type != null)
                {
                    if (inventoryCreator.FindFreeEquipSlotWithType(Item.Type) != null)
                        ChangeSlot(inventoryCreator.FindFreeEquipSlotWithType(Item.Type), true);
                    else
                    {
                        if (inventoryCreator.FindFreeEquipSlot() != null)
                            ChangeSlot(inventoryCreator.FindFreeEquipSlot(), true);
                        else
                        {
                            int length = inventoryCreator.Slots.Count;
                            for (int i = 0; i < length; i++)
                            {
                                if (inventoryCreator.Slots[i].Item)
                                    if (inventoryCreator.Slots[i].IsEquipSlot && inventoryCreator.Slots[i].Item.Type == Item.Type)
                                    {
                                        ChangeSlot(inventoryCreator.Slots[i], true);
                                        return;
                                    }
                            }
                            PlaySound(TPSound.AudioTypeEnum.Failure);
                        }
                    }
                }
                else
                {
                    if (inventoryCreator.FindFreeEquipSlot() != null)
                        ChangeSlot(inventoryCreator.FindFreeEquipSlot(), true);
                    else
                        PlaySound(TPSound.AudioTypeEnum.Failure);
                }
            }
        }

        //void changeslottest()
        //{
        //if (IsEquipSlot && slot.IsEquipSlot && (Item == null || slot.Item == null))
        //{
        //    Debug.Log("From equip to equip, empty");
        //}
        //if (IsEquipSlot && !slot.IsEquipSlot && (Item == null || slot.Item == null))
        //{
        //    Debug.Log("From equip to normal, empty");
        //}
        //if (!IsEquipSlot && slot.IsEquipSlot && (Item == null || slot.Item == null))
        //{
        //    Debug.Log("From normal to equip, empty");
        //}
        //if (!IsEquipSlot && !slot.IsEquipSlot && (Item == null || slot.Item == null))
        //{
        //    Debug.Log("From normal to normal, empty");
        //}

        //if (Item != null && slot.Item != null && IsEquipSlot && slot.IsEquipSlot)
        //{
        //    Debug.Log("from equip to equip both with items");
        //}
        //if (Item != null && slot.Item != null && !IsEquipSlot && slot.IsEquipSlot)
        //{
        //    Debug.Log("from normal to equip both with items");
        //}
        //if (Item != null && slot.Item != null && !IsEquipSlot && !slot.IsEquipSlot)
        //{
        //    Debug.Log("from normal to normal = move/replace - both with items");
        //}
        //if (Item != null && slot.Item != null && IsEquipSlot && !slot.IsEquipSlot)
        //{
        //    Debug.Log("from equip to normal = move/replace - both with items");
        //}
        //}

        void ChangeSlot(TPSlot slot, bool toModify)
        {
            if (toModify)
            {
                ModifyStats();
                slot.ModifyStats();
                PlaySound(IsEquipSlot ? TPSound.AudioTypeEnum.RemoveItem : TPSound.AudioTypeEnum.WearItem);
            }
            else
                PlaySound(TPSound.AudioTypeEnum.MoveItem);

            var tempItem = slot.Item;
            slot.Item = Item;
            Item = tempItem;
        }

        void ModifyStats()
        {
            if (Item == null)
                return;

            int _Modlength = Item.Modifiers.Length;
            for (int i = 0; i < _Modlength; i++)
            {
                if (IsEquipSlot)
                    Item.Modifiers[i].UnModify();
                else
                    Item.Modifiers[i].Modify();
            }
        }

        void PlaySound(TPSound.AudioTypeEnum audioType)
        {
            AudioSource source = FindObjectOfType<AudioSource>();
            if(source == null)
            {
                Debug.LogError("There is no Audio Source in scene");
                return;
            }

            int length = Item.Sounds.Length;
            for (int i = 0; i < length; i++)
            {
                if (Item.Sounds[i].AudioType == audioType)
                {
                    source.PlayOneShot(Item.Sounds[i].AudioClip);
                    return;
                }
            }
        }
    }
}