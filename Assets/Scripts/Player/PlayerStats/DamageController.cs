using UnityEngine;
public class DamageController
{
    private IHealth health;

    public DamageController(IHealth health)
    {
        this.health = health;
    }

    public void DealDamage(float damage)
    {
        health.ChangeHealth(-damage);
    }
}