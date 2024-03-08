using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAggresiveAnimal
{
    public interface IAttackable
    {
        void Attack(Transform target);
    }

    public interface IMoveTowards
    {
        void MoveTowards(Vector3 target);
    }
}
