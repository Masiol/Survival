using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[Serializable]
public class AnimationCheck
{
    public string animationName;
    public float normalizedTime;
    public MeleeItemSO meleeItem; // Zak³adam, ¿e ta klasa jest zdefiniowana gdzie indziej w Twoim projekcie.
}

public class AnimationListenerForUseStamina : MonoBehaviour
{
    public List<AnimationCheck> animationsToCheck = new List<AnimationCheck>();
    private Animator animator;
    private IStaminaConsumer staminaConsumer; // Zak³adam, ¿e interfejs jest zdefiniowany gdzie indziej w Twoim projekcie.

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
        float normalizedTime = stateInfo.normalizedTime % 1; // Dla obs³ugi animacji w pêtli.

        foreach (var animationCheck in animationsToCheck)
        {
            if (stateInfo.IsName(animationCheck.animationName) && !animator.IsInTransition(0))
            {
                // SprawdŸ, czy punkt normalizedTime zosta³ osi¹gniêty.
                if (normalizedTime >= animationCheck.normalizedTime && normalizedTime < (animationCheck.normalizedTime + 0.01f) % 1) // Dodano margines, aby unikn¹æ problemów z precyzj¹ zmiennoprzecinkow¹.
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
            // Opcjonalnie, mo¿esz tutaj dodaæ logikê do resetowania stanu, jeœli to konieczne.
        }
    }
}