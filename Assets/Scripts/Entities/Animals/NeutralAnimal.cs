using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// Zak³adaj¹c, ¿e interfejsy IMovable, IEatable, i IRunnable s¹ ju¿ zdefiniowane w INeutralAnimal
public abstract class NeutralAnimal : Animal, INeutralAnimal.IMovable, INeutralAnimal.IEatable, INeutralAnimal.IRunnable
{
    protected bool isEating = false;
    protected bool isRunningAway = false;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }

    public abstract void Move();
    public abstract void Eat();
    public abstract void RunAwayFrom(Vector3 dangerPoint);

    protected override void ReactToPlayer()
    {
        Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        RunAwayFrom(playerPosition);
    }

    protected abstract void PerformRandomAction();
}
