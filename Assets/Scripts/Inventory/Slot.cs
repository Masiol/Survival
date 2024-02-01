using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image enterImage = null;
    [SerializeField] private Image selectImage = null;

    public Item currentItem = null;
    private Image icon;
    private TextMeshProUGUI itemQuantityTxt;

    private void Awake()
    {
        icon = transform.Find("Icon").GetComponent<Image>();
        itemQuantityTxt = transform.Find("Quantity").GetComponent<TextMeshProUGUI>();
        enterImage.enabled = false;
        selectImage.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        enterImage.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        enterImage.enabled = false;
    }
    public void MouseSelect(BaseEventData data)
    {
        PointerEventData pointer = (PointerEventData)data;
        if (pointer.button == PointerEventData.InputButton.Right)
        {
            Inventory.instance.panelOptions.ShowOptions(this);
        }
    }
    public void ChangeItem(Item _item)
    {
        currentItem = _item;
        Refresh();
    }

    public void Refresh()
    {
        RefreshIcon();
        RefreshQuantity();
    }
    public void SetSelectImage(bool active)
    {
        selectImage.enabled = active;
    }
    public void DeleteItem()
    {
        if (currentItem == null || currentItem.itemQuantity <= 0)
            return;

        currentItem.itemQuantity--;
        if(currentItem.itemQuantity <= 0)
        {
            ChangeItem(null);
            return;
        }
        Refresh();
    }

    private void RefreshIcon()
    {
        if (currentItem == null)
        {
            icon.sprite = null;
            icon.color = new Color(255, 255, 255, 0);
            return;
        }
        icon.sprite = currentItem.itemIcon;
        icon.type = Image.Type.Simple;
        icon.preserveAspect = true;
        icon.color = Color.white;
    }

    private void RefreshQuantity()
    {
        if (currentItem == null)
        {
            itemQuantityTxt.text = string.Empty;
            return;
        }
        itemQuantityTxt.text = (currentItem.itemQuantity > 1) ? currentItem.itemQuantity.ToString() : string.Empty;
    }

    

    
}

