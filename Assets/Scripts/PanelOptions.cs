using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelOptions : MonoBehaviour
{
    private Transform gridButtons = null;
    public Slot slotSelected = null;

    public void Init()
    {
        gridButtons = transform.Find("GridButtons");
        HideOptions();
    }

    public void ShowOptions(Slot _slot)
    {

        if (_slot == null || _slot.currentItem == null)
            return;

        slotSelected = _slot;
        slotSelected.SetSelectImage(true);

        gameObject.SetActive(true);

        gridButtons.localPosition = _slot.transform.localPosition;
    }

    public void HideOptions()
    {
        if (slotSelected != null)
            slotSelected.SetSelectImage(false);

        slotSelected = null;

        gameObject.SetActive(false);
    }

    public void EventButtonDrop()
    {

        Item item = Instantiate(slotSelected.currentItem);
        item.itemQuantity = 1;

        FindObjectOfType<PlayerController>().DropObject(item);

        slotSelected.DeleteItem();
        HideOptions();
    }
    public void EventButtonDropAll()
    {
        if (slotSelected != null || slotSelected.currentItem == null)
        {
            HideOptions();
            return;
        }
        HideOptions();
    }
    public void EventButtonSplit()
    {
        if (slotSelected != null || slotSelected.currentItem == null)
        {
            HideOptions();
            return;
        }
        HideOptions();
    }
}
