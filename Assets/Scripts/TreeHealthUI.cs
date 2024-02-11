using UnityEngine;
using System.Collections.Generic;

public class TreeHealthUI : MonoBehaviour
{
    [SerializeField] private TreeHealth treeHealth;
    [SerializeField] private List<GameObject> healthSteps;
    [SerializeField] private GameObject healthStepsParent;

    private void OnEnable()
    {
        treeHealth.OnHealthChanged += UpdateHealthUI;
    }
    private void Start()
    {
        healthStepsParent.SetActive(false);
        foreach (var step in healthSteps)
        {
            step.SetActive(true);
        }
        UpdateHealthUI(treeHealth.GetCurrentHealth());
    }

    private void OnDisable()
    {
        treeHealth.OnHealthChanged -= UpdateHealthUI;
    }

    public void SetVisibleObject(bool state)
    {
        healthStepsParent.SetActive(state);
    }

    private void UpdateHealthUI(int currentHealth)
    {
        if (!healthStepsParent.activeSelf)
            return;
        
        for (int i = 0; i < healthSteps.Count; i++)
        {
            healthSteps[i].SetActive(i < currentHealth);
        }
    }
}