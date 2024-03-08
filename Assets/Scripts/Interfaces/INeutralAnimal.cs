using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INeutralAnimal
{
    public interface IMovable
    {
        void Move();
    }

    public interface IEatable
    {
        void Eat();
    }

    public interface IRunnable
    {
        void RunAwayFrom(Vector3 dangerPoint);
    }
}
