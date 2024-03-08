using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Fox : NeutralAnimal
{
    [Header("Wandering Settings")]
    [SerializeField] private float wanderDistance = 50f;
    [SerializeField] private float angleRange = 120f;
    [SerializeField] private float walkSpeed = 2f;

    [Header("State Timing Settings")]
    [SerializeField] private float minTimeBeforeNewPoint = 5f;
    [SerializeField] private float maxTimeBeforeNewPoint = 10f;
    [SerializeField] private float eatingDuration = 5f;
    [SerializeField] private float idleDuration = 5f;


    [Header("Fleeing Settings")]
    [SerializeField] private float runSafeDistance = 30f;
    [SerializeField] private float minRunningTime = 5f;
    [SerializeField] private float maxRunningTime = 8f;
    [SerializeField] private float runSpeed = 5.5f;
    private bool isFleeing = false;

    private enum State { Moving, Eating, Idle, Running }
    private State currentState = State.Idle;

    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(StateController());
    }

    IEnumerator StateController()
    {
        while (true)
        {
            switch (currentState)
            {
                case State.Moving:
                    Move();
                    break;
                case State.Eating:
                    Eat();
                    break;
                case State.Idle:
                    Idle();
                    break;
                case State.Running:
                    RunAwayFrom(PlayerPosition());
                    break;
            }

            float waitTime = Random.Range(minTimeBeforeNewPoint, maxTimeBeforeNewPoint);
            yield return new WaitForSeconds(waitTime);

            ChooseNextAction();
        }
    }

    void ChooseNextAction()
    {
        int action = Random.Range(0, 3); // Losowanie miêdzy 0 a 2
        currentState = (State)action;
    }

    public override void Move()
    {
        currentState = State.Moving;
        ResetAllAnimations();
        animator.SetBool("IsMoving", true);

        agent.isStopped = false;
        Vector3 randomDestination = GetRandomDestination();
        agent.SetDestination(randomDestination);
    }

    public override void Eat()
    {
        currentState = State.Eating;
        ResetAllAnimations();
        animator.SetBool("IsEating", true);

        agent.isStopped = true;
        StartCoroutine(StopEatingAfterDelay(eatingDuration));
    }

    void Idle()
    {
        currentState = State.Idle;
        ResetAllAnimations();
        animator.SetBool("IsIdle", true);
        agent.isStopped = true;
        StartCoroutine(StopIdleAfterDelay(idleDuration));
    }

    IEnumerator StopEatingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ChooseNextAction();
    }

    IEnumerator StopIdleAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ChooseNextAction();
    }

    void ResetAllAnimations()
    {
        animator.SetBool("IsMoving", false);
        animator.SetBool("IsEating", false);
        animator.SetBool("IsIdle", false);
        animator.SetBool("IsFleeing", false);
    }

    Vector3 GetRandomDestination()
    {
        float randomAngle = Random.Range(-angleRange / 2, angleRange / 2);
        Quaternion rotation = Quaternion.Euler(0, randomAngle, 0);
        Vector3 direction = rotation * transform.forward * wanderDistance;
        Vector3 randomDestination = transform.position + direction;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDestination, out hit, wanderDistance, -1))
        {
            return hit.position;
        }
        return transform.position;
    }

    protected override void PerformRandomAction()
    {
        ChooseNextAction();
    }
    protected override void ReactToPlayer()
    {
        Vector3 playerPosition = PlayerPosition();
        RunAwayFrom(playerPosition);
    }

    public override void RunAwayFrom(Vector3 dangerPoint)
    {
        currentState = State.Running;
        agent.speed = runSpeed;
        ResetAllAnimations();
        animator.SetBool("IsFleeing", true);

        agent.isStopped = false;
        Vector3 fleeDirection = (transform.position - dangerPoint).normalized;
        Vector3 fleePoint = GetFleeDestination(fleeDirection);

        agent.SetDestination(fleePoint);

        isFleeing = true;
        StartCoroutine(ResetFleeingStateAfterDelay());
    }
    Vector3 GetFleeDestination(Vector3 fleeDirection)
    {
        Vector3 potentialDestination = transform.position + fleeDirection * runSafeDistance;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(potentialDestination, out hit, runSafeDistance, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return transform.position;
    }
    IEnumerator ResetFleeingStateAfterDelay()
    {
        float fleeDuration = Random.Range(minRunningTime, maxRunningTime);
        yield return new WaitForSeconds(fleeDuration);
        if (IsPlayerNearby())
        {
            StartCoroutine(ResetFleeingStateAfterDelay());
        }
        else
        {
            isFleeing = false;
            ChooseNextAction();
            agent.speed = walkSpeed;
        }
    }

}
