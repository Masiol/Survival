using UnityEngine;
using System;

public class TreeHealth : MonoBehaviour
{
    public event Action<int> OnHealthChanged;
    [SerializeField] private int maxHealth = 8;
    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0 && GetComponent<Tree>().GetTreeType() == TreeType.Tree)
        {
            GetComponent<Tree>().ApplyPhysics();
            GetComponentInParent<TreeManager>().CanChopStump = true;
        }
        if (currentHealth <= 0 && GetComponent<Tree>().GetTreeType() == TreeType.Stump)
        {
            GetComponent<Tree>().DestroyObject();
        }
    }
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}