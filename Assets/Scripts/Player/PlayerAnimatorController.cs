using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    private Animator playerAnimator;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
    }
    public void Equip()
    {
        playerAnimator.SetTrigger("Equip");
    }
    public void PlayTriggerAnimation(string animTriggerName)
    {
        playerAnimator.SetTrigger(animTriggerName);
    }
    public void PlayBoolAnimation(string animTriggerName, bool state)
    {
        playerAnimator.SetBool(animTriggerName,state);
    }
}
