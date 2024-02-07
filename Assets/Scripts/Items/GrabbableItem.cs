using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GrabbableItem : PickupableItem, IGrabbable
{
    public PlayerAnimatorController playerAnimatorController;
    public string[] animTriggerName;
    private void Awake()
    {
        playerAnimatorController = FindObjectOfType<PlayerAnimatorController>();
    }
    public virtual void Grab()
    {
        Inventory inventory = FindObjectOfType<Inventory>();
        if (inventory != null)
        {
            inventory.AddItemToHotbar(itemData);
            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("Inventory not found.");
        }     
    }

    public virtual void Use()
    {
        playerAnimatorController.PlayTriggerAnimation(animTriggerName[1]);
    }

    public virtual void SpecialUse()
    {
        Debug.Log($"Special using {itemData.itemName}.");
    }
}