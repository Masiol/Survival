using UnityEngine;
using System.Collections.Generic;

public class TreeHealthUI : MonoBehaviour
{
    private TreeHealth treeHealth;
    [SerializeField] private List<GameObject> healthSteps;
    [SerializeField] private GameObject healthStepsParent;

    private void Start()
    {
        treeHealth = GetComponent<TreeHealth>();
        treeHealth.OnHealthChanged += UpdateHealthUI;

        healthStepsParent.SetActive(true);

        healthSteps.Clear();

        foreach (Transform child in healthStepsParent.transform)
        {
            healthSteps.Add(child.gameObject);
        }

        healthStepsParent.SetActive(false);

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
