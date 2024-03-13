using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Deer : NeutralAnimal
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
    private Vector3 fleeDestination = Vector3.zero;

    private enum State { Moving, Eating, Idle, Running, Died }
    private State currentState = State.Idle;

    private bool isDead;
    protected override void Awake()
    {
        base.Awake();
        animalHealth.InitializeHealth(100f);
        animalHealth.OnHealthChanged += UpdateHealth;
        animalHealth.OnHealthZero += Die;
        StartCoroutine(StateController());
    }
    private IEnumerator StateController()
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
                case State.Died:
                    Die();
                    break;
            }

            float waitTime = Random.Range(minTimeBeforeNewPoint, maxTimeBeforeNewPoint);
            yield return new WaitForSeconds(waitTime);

            ChooseNextAction();
        }
    }

    private void ChooseNextAction()
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

    private void Idle()
    {
        currentState = State.Idle;
        ResetAllAnimations();
        animator.SetBool("IsIdle", true);
        agent.isStopped = true;
        StartCoroutine(StopIdleAfterDelay(idleDuration));
    }

    private IEnumerator StopEatingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ChooseNextAction();
    }

    private IEnumerator StopIdleAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ChooseNextAction();
    }

    private void ResetAllAnimations()
    {
        animator.SetBool("IsMoving", false);
        animator.SetBool("IsEating", false);
        animator.SetBool("IsIdle", false);
        animator.SetBool("IsFleeing", false);
    }

    private Vector3 GetRandomDestination()
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
    private void UpdateHealth()
    {
        animator.SetTrigger("IsHitted");
    }

    private void Die()
    {
        if (!isDead)
        {
            isDead = true;
            ResetAllAnimations();
            isFleeing = false;

            agent.isStopped = true;
            agent.speed = 0;
            agent.SetDestination(transform.position);
            animator.SetTrigger("IsDead");
            currentState = State.Died; // Zmiana stanu na Died
        }
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
        if (isDead) return;

        currentState = State.Running;
        agent.speed = runSpeed;
        ResetAllAnimations();
        animator.SetBool("IsFleeing", true);

        agent.isStopped = false;
        Vector3 fleeDirection = (transform.position - dangerPoint).normalized;
        Vector3 fleePoint = GetFleeDestination(fleeDirection);
        fleeDestination = fleePoint; // Aktualizacja zmiennej przechowuj¹cej punkt ucieczki

        agent.SetDestination(fleePoint);

        isFleeing = true;
        StartCoroutine(ResetFleeingStateAfterDelay());
    }
    private Vector3 GetFleeDestination(Vector3 fleeDirection)
    {
        Vector3 potentialDestination = transform.position + fleeDirection * runSafeDistance;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(potentialDestination, out hit, runSafeDistance, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return transform.position;
    }
    private IEnumerator ResetFleeingStateAfterDelay()
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
    private void OnDrawGizmosSelected()
    {
        if (currentState == State.Running)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(fleeDestination, 1f);
            Gizmos.DrawLine(transform.position, fleeDestination);// Rysuje czerwon¹ kulkê w punkcie ucieczki
        }
    }

}
