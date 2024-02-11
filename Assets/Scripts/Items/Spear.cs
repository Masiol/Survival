using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : GrabbableItem
{ 

    public override void Awake()
    {
        base.Awake();
    }
    public override void Grab()
    {
        base.Grab();
    }

    public override void Use()
    {
        base.Use();
    }

    public override void SpecialUse(bool _isPressed)
    {
        if (!_isPressed) return;
       /* if (inventory != null)
        {
            inventory.RemoveItemFromInventory();
        }*/
    }
}
