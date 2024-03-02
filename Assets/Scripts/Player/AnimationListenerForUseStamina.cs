using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[Serializable]
public class AnimationCheck
{
    public string animationName;
    public float normalizedTime;
    public MeleeItemSO meleeItem; // Zak�adam, �e ta klasa jest zdefiniowana gdzie indziej w Twoim projekcie.
}

public class AnimationListenerForUseStamina : MonoBehaviour
{
    public List<AnimationCheck> animationsToCheck = new List<AnimationCheck>();
    private Animator animator;
    private IStaminaConsumer staminaConsumer; // Zak�adam, �e interfejs jest zdefiniowany gdzie indziej w Twoim projekcie.

    private void Awake()
    {
        animator = GetComponent<Animator>();
        var consumers = FindObjectsOfType<MonoBehaviour>().OfType<IStaminaConsumer>();
        if (consumers.Any())
        {
            staminaConsumer = consumers.First();
        }
    }

        private void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float normalizedTime = stateInfo.normalizedTime % 1; // Dla obs�ugi animacji w p�tli.

        foreach (var animationCheck in animationsToCheck)
        {
            if (stateInfo.IsName(animationCheck.animationName) && !animator.IsInTransition(0))
            {
                // Sprawd�, czy punkt normalizedTime zosta� osi�gni�ty.
                if (normalizedTime >= animationCheck.normalizedTime && normalizedTime < (animationCheck.normalizedTime + 0.01f) % 1) // Dodano margines, aby unikn�� problem�w z precyzj� zmiennoprzecinkow�.
                {
                    PerformAction(animationCheck);
                }
            }
        }
    }

    private void PerformAction(AnimationCheck animationCheck)
    {
        if (animationCheck.meleeItem != null && staminaConsumer != null)
        {
            staminaConsumer.ConsumeStamina(animationCheck.meleeItem.consumeStamina);
            // Opcjonalnie, mo�esz tutaj doda� logik� do resetowania stanu, je�li to konieczne.
        }
    }
}