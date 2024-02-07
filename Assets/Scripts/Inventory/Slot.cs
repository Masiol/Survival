using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool hovered;
    private ItemSO heldItem;

    private Color opaque = new Color(1, 1, 1, 1);
    private Color transparent = new Color(1, 1, 1, 0);

    private Image thisSlotImage;
    private TMP_Text thisSlotQuantityText;

    public delegate void ItemChanged(ItemSO newItem);
    public event ItemChanged OnItemChanged;

    [SerializeField] public bool isHotbarSlot;

    public void InitialiseSlot()
    {
        thisSlotImage = gameObject.GetComponent<Image>();
        thisSlotQuantityText = transform.GetChild(0).GetComponent<TMP_Text>();
        thisSlotImage.sprite = null;
        thisSlotImage.color = transparent;
        SetItem(null);
    }    
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        hovered = true;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        hovered = false;
    }

    public void SetItem(ItemSO _item)
    {
        if (_item != null && !CanAcceptItem(_item))
        {
            return;
        }
        heldItem = _item;

        if (_item != null)
        {
            thisSlotImage.sprite = heldItem.itemIcon;
            thisSlotImage.color = opaque;
            UpdateData();
        }
        else
        {
            thisSlotImage.sprite = null;
            thisSlotImage.color = transparent;
            UpdateData();
        }

        if (isHotbarSlot)
        {
            OnItemChanged?.Invoke(_item);
        }
    }

    public ItemSO GetItem()
    {
        return heldItem;
    }

    public void UpdateData()
    {
        if (heldItem != null)
            thisSlotQuantityText.text = heldItem.itemQuantity.ToString();
        else
            thisSlotQuantityText.text = "";
    }

    public bool HasItem()
    {
        return heldItem ? true : false;
    }
    public bool CanAcceptItem(ItemSO _item)
    {
        if (isHotbarSlot)
        {
            return _item.ItemType == ItemType.Weapon || _item.ItemType == ItemType.Tool || _item.ItemType == ItemType.Consumable;
        }
        return true;
    }


}