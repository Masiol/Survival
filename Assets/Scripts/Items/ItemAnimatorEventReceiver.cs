using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemAnimatorEventReceiver : MonoBehaviour
{
    [SerializeField] private MeleeItemSO expectedItem;
    [SerializeField] private Transform damagePoint;
    [SerializeField] private float damageRadius;
    private IStaminaConsumer staminaConsumer;
    

    private void Awake()
    {
        var consumers = FindObjectsOfType<MonoBehaviour>().OfType<IStaminaConsumer>();
        if (consumers.Any())
        {
            staminaConsumer = consumers.First();
        }
    }
    private void OnEnable()
    {
        AnimatorListener.OnAttackEvent += CheckEvent;
    }

    private void OnDisable()
    {
        AnimatorListener.OnAttackEvent -= CheckEvent;
    }
    private void CheckEvent(HandItemSO item)
    {
        Debug.Log("check item");
        if (item is MeleeItemSO meleeItem)
        {
            if (meleeItem.itemName == expectedItem.itemName)
            {
                Collider[] hits = Physics.OverlapSphere(damagePoint.position, damageRadius);
                foreach (var hit in hits)
                {
                    var healthComponent = hit.GetComponent<AnimalHealth>();
                    if (healthComponent != null)
                    {
                        healthComponent.TakeDamage(meleeItem.damage);
                    }
                }

                if (meleeItem != null && staminaConsumer != null)
                {
                    staminaConsumer.ConsumeStamina(meleeItem.consumeStamina);
                }
            }
            else
            {
                Debug.Log("Melee item does not match!");
            }
        }
    }
}
