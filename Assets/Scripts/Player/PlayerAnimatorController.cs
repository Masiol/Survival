using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
    }
    public void PlayTriggerAnimation(string animTriggerName)
    {
        playerAnimator.SetTrigger(animTriggerName);
    }
}
