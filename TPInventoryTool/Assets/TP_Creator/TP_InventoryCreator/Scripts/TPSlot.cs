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
        UnityAction onPointerEnterAction;
        [SerializeField] TPItem _item;

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

        public void Load(int _saved)
        {
            if (_saved == -1)
                return;

            int length = inventoryCreator.InventoryPersistance.inventoryData.Items.Count;
            for (int i = 0; i < length; i++)
            {
                if (inventoryCreator.InventoryPersistance.inventoryData.Items[i].ID == _saved)
                    Item = inventoryCreator.InventoryPersistance.inventoryData.Items[i];
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Item == null)
                return;

            AutoEquip();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (Item == null)
                return;

            basePosition = actualTransform.position;
            canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (Item == null)
                return;

            actualTransform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (Item == null)
                return;

            if (eventData.pointerEnter != null)
            {
                TPSlot slot = eventData.pointerEnter.GetComponent<TPSlot>();
                if (slot != null)
                {
                    // Jeśli to na co patrzysz jest equipem, to sprawdz czy ma ten sam typ co trzymany item
                    // Jeśli to na co patrzysz nie jest equipem to sprawdź, czy ma ten sam typ lub nie ma typu co trzymany item
                    if (slot.isEquipSlot ? Item.Type == slot.Type : (Item.Type == slot.Type || slot.Type == null))
                    {
                        if (slot.Item != null) // if on looking slot there is actually Item - replace them
                        {
                            // Sprawdź, czy trzymany item jest Equip slotem
                            // Jeśli tak, to sprawdź czy item na patrzonym slocie jest tego typu co trzymany item.
                            // Replacing functionality
                            if (isEquipSlot ? slot.Item.Type == Item.Type/* || slot.Item == null*/ : true)
                            {
                                if (isEquipSlot)
                                {
                                    ModifyStats();
                                    slot.ModifyStats();
                                    PlaySound(TPSound.AudioTypeEnum.RemoveItem);
                                }
                                else if (slot.isEquipSlot)
                                {
                                    ModifyStats();
                                    slot.ModifyStats();
                                    PlaySound(TPSound.AudioTypeEnum.WearItem);
                                }
                                else
                                    PlaySound(TPSound.AudioTypeEnum.MoveItem);

                                var tempItem = slot.Item;
                                slot.Item = Item;
                                Item = tempItem;
                            }
                        }
                        else // If slot is free
                        {
                            if (slot.isEquipSlot || isEquipSlot)
                                ModifyStats();
                            PlaySound(slot.isEquipSlot ? TPSound.AudioTypeEnum.WearItem :
                                (isEquipSlot ? TPSound.AudioTypeEnum.RemoveItem : TPSound.AudioTypeEnum.MoveItem));
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
            if (Item == null)
                return;

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
            var slots = inventoryCreator.Slots;
            int _length = slots.Count;

            for (int i = 0; i < _length; i++)
            {
                // If you click on equip slot, it will find no-equip slot
                if (isEquipSlot ? !slots[i].isEquipSlot : slots[i].isEquipSlot)
                {
                    // old code
                    //if (isEquipSlot ?
                    //    ((slots[i].Type == Item.Type || slots[i].Type == null) && slots[i].Item == null)
                    //    :
                    //    ((slots[i].Type == Item.Type && slots[i].Item == null)))
                    //{
                    //    ModifyStats();
                    //    PlaySound(isEquipSlot ? TPSound.AudioTypeEnum.RemoveItem : TPSound.AudioTypeEnum.WearItem);
                    //    slots[i].Item = Item;
                    //    Item = null;
                    //    break;
                    //}
                    if ((slots[i].Type == Item.Type || slots[i].Type == null) && slots[i].Item == null)
                    {
                        ModifyStats();
                        PlaySound(isEquipSlot ? TPSound.AudioTypeEnum.RemoveItem : TPSound.AudioTypeEnum.WearItem);
                        slots[i].Item = Item;
                        Item = null;
                        break;
                    }
                    else if (!isEquipSlot && slots[i].Type == Item.Type)
                    {
                        ModifyStats();
                        slots[i].ModifyStats();
                        PlaySound(isEquipSlot ? TPSound.AudioTypeEnum.RemoveItem : TPSound.AudioTypeEnum.WearItem);
                        var tempItem = slots[i].Item;
                        slots[i].Item = Item;
                        Item = tempItem;
                        break;
                    }
                }
            }
        }

        public void ModifyStats()
        {
            int _Modlength = Item.Modifiers.Length;
            for (int i = 0; i < _Modlength; i++)
            {
                if (isEquipSlot)
                    Item.Modifiers[i].UnModify();
                else
                    Item.Modifiers[i].Modify();
                StartCoroutine(Item.Modifiers[i].StatValueChanged());
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