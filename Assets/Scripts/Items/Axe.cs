using UnityEngine;

public class Axe : GrabbableItem
{
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
        if(_isPressed)
        playerAnimatorController.PlayBoolAnimation(animTriggerName[1], true);
        else
            playerAnimatorController.PlayBoolAnimation(animTriggerName[1], false);
    }
}