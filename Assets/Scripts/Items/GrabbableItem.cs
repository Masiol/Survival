using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GrabbableItem : PickupableItem, IGrabbable
{
    public string[] animTriggerName;
    protected PlayerAnimatorController playerAnimatorController;

    public virtual void Awake()
    {
        playerAnimatorController = FindObjectOfType<PlayerAnimatorController>();
    }
    public virtual void Grab()
    { 
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
        playerAnimatorController.PlayTriggerAnimation(animTriggerName[0]);
    }

    public virtual void SpecialUse(bool _isPressed)
    {

        Debug.Log($"Special using {itemData.itemName}.");
    }
}