using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemObject : MonoBehaviour, IPickupable, IHoldable
{
    [SerializeField] private string itemName = string.Empty;

    public Item item = null;
    private void Start()
    {
        item = GameManager.instance.resources.GetItemByName(itemName);
    }

    public void PickItem()
    {
        if (item == null)
            return;
        bool result = Inventory.instance.AddItems(item);
        if (result)
            Destroy(gameObject);
    }

    public void Pickup()
    {
        PickItem();
    }

    public void Hold()
    {
        //throw new System.NotImplementedException();
    }
}
