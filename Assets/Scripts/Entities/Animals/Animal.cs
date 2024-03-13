using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public abstract class Animal : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected Animator animator;
    protected AnimalHealth animalHealth;

    public LayerMask playerLayer;
    public float detectionRadius = 10f;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        InitializeAnimalHealth();
    }
    protected void InitializeAnimalHealth()
    {
        animalHealth = gameObject.AddComponent<AnimalHealth>();
    }

    protected bool IsPlayerNearby()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
        return hits.Length > 0;
    }

    protected abstract void ReactToPlayer();

    protected virtual void Update()
    {
        if (IsPlayerNearby())
        {
            ReactToPlayer();
        }
    }
    protected Vector3 PlayerPosition()
    {
        return GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}