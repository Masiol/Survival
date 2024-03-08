using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressiveAnimal : Animal, IAggresiveAnimal.IAttackable, IAggresiveAnimal.IMoveTowards
{
    public float attackDistance = 5f;
    private Transform playerTransform;

    protected override void Awake()
    {
        base.Awake();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected override void ReactToPlayer()
    {
        if (Vector3.Distance(transform.position, PlayerPosition()) <= attackDistance)
        {
            Attack(playerTransform);
        }
        else
        {
            MoveTowards(PlayerPosition());
        }
    }

    public void Attack(Transform target)
    {
        // Implementacja logiki atakowania, np. zmniejszenie punktów zdrowia gracza
        Debug.Log("Attacking the player!");
    }

    public void MoveTowards(Vector3 target)
    {
        agent.SetDestination(target);
    }
}