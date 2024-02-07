using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickupableItem : ItemBase, IPickupable
{
    public override void Pickup()
    {
        base.Pickup();
        Debug.Log($"{itemData.itemName} picked up.");
    }
}