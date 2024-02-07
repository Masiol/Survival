using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    [SerializeField] private string itemName = string.Empty;
    protected ItemSO itemData;
    protected Inventory inventory;

    protected void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        LoadItemData();
    } 
    public virtual void Pickup()
    {
        if (inventory != null && itemData != null)
        {
            inventory.AddItemToInventory(itemData);
            Destroy(gameObject);
        }
    }
    private void LoadItemData()
    {
        itemData = GameManager.instance.resources.GetItemByName(itemName);
    }
   
}