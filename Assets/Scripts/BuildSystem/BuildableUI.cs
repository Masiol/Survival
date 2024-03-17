using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class BuildableUI : MonoBehaviour
{
    [SerializeField] private GameObject itemUITemplate;
    [SerializeField] private Transform itemUIParent;

    private Dictionary<RequiredItems, GameObject> itemUIInstances = new Dictionary<RequiredItems, GameObject>();

    private void Awake()
    {
        itemUIParent.gameObject.SetActive(false);
    }

    public void InstantiateItemUI(List<RequiredItems> requiredItems)
    {
        itemUIParent.gameObject.SetActive(true);

        foreach (KeyValuePair<RequiredItems, GameObject> kvp in itemUIInstances)
        {
            Destroy(kvp.Value);
        }

        itemUIInstances.Clear();

        foreach (RequiredItems requiredItem in requiredItems)
        {
            GameObject newItemUI = Instantiate(itemUITemplate, itemUIParent);
            newItemUI.SetActive(true);
            newItemUI.GetComponentInChildren<Image>().sprite = requiredItem.item.itemIcon;
            itemUIInstances.Add(requiredItem, newItemUI);

            TextMeshProUGUI itemText = newItemUI.GetComponentInChildren<TextMeshProUGUI>();
            if (itemText != null)
            {
                itemText.text = $"{requiredItem.collectedRequiredItemAmount}/{requiredItem.itemRequiredAmount}";
            }
        }
    }

    public void UpdateItemUI(List<RequiredItems> requiredItems)
    {
        foreach (RequiredItems requiredItem in requiredItems)
        {
            if (itemUIInstances.ContainsKey(requiredItem))
            {
                TextMeshProUGUI itemText = itemUIInstances[requiredItem].GetComponentInChildren<TextMeshProUGUI>();
                if (itemText != null)
                {
                    itemText.text = $"{requiredItem.collectedRequiredItemAmount}/{requiredItem.itemRequiredAmount}";
                }
            }
        }
    }
    public void DestroyUI()
    {
        Destroy(this.gameObject);
    }
}
