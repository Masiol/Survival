using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IHealth
{
    event Action<float> OnHealthChange;
    void ChangeHealth(float amount);
    float GetCurrentHealth();
}