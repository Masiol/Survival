using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RequiredItems
{
    public ItemSO item;
    public int itemRequiredAmount;
    [HideInInspector] public int collectedRequiredItemAmount;
    public bool itemColleted;

    // Definicja zdarzenia do obs³ugi zmiany collectedRequiredItemAmount
    public delegate void CollectedAmountChangedDelegate();
    public event CollectedAmountChangedDelegate CollectedAmountChanged;



    // Metoda do zmiany collectedRequiredItemAmount
    public void UpdateCollectedAmount(int newAmount)
    {
        collectedRequiredItemAmount = newAmount;
        CollectedAmountChanged?.Invoke(); // Wywo³anie zdarzenia
    }
}

public class BuildableElement : MonoBehaviour, IBuildable
{
    [SerializeField] private List<RequiredItems> requiredItems = new List<RequiredItems>();
    [SerializeField] private Material finalMaterial;
    private Inventory inventory;

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();

        foreach (RequiredItems requiredItem in requiredItems)
        {
            requiredItem.CollectedAmountChanged += UpdateUIOnCollectedAmountChanged;
        }
    }

    private void OnDestroy()
    {
        foreach (RequiredItems requiredItem in requiredItems)
        {
            requiredItem.CollectedAmountChanged -= UpdateUIOnCollectedAmountChanged;
        }
    }

    public void SetupUI()
    {
        gameObject.transform.root.GetComponentInChildren<BuildableUI>()?.InstantiateItemUI(requiredItems);
    }

    public void Build()
    {
        if (inventory != null)
        {
            bool canBuild = false;

            foreach (RequiredItems requiredItem in requiredItems)
            {
                int itemCount = inventory.CountItemsOfType(requiredItem.item);
                Debug.Log(itemCount);
                if (itemCount > 0)
                {
                    canBuild = true;
                    break;
                }
            }

            if (canBuild)
            {
                foreach (RequiredItems requiredItem in requiredItems)
                {
                    if (requiredItem.collectedRequiredItemAmount < requiredItem.itemRequiredAmount)
                    {
                        RemoveItemAndUpdateCollectedAmount(requiredItem.item);
                    }
                }

                if (AreAllItemsCollected())
                {
                    FinalBuildAfterDeliveryMaterials();
                    Destroy(gameObject.transform.root.GetComponentInChildren<BuildableUI>().gameObject);
                }
            }
        }
    }

    private bool AreAllItemsCollected()
    {
        foreach (RequiredItems requiredItem in requiredItems)
        {
            if (!requiredItem.itemColleted)
            {
                return false;
            }
        }
        return true;
    }

    private void RemoveItemAndUpdateCollectedAmount(ItemSO _item)
    {
        int remainingAmount = _item.itemQuantity;

        foreach (Slot slot in inventory.GetInventorySlots())
        {
            ItemSO itemInSlot = slot.GetItem();

            if (itemInSlot != null && itemInSlot.itemName == _item.itemName)
            {
                int itemsToRemove = Mathf.Min(itemInSlot.itemQuantity, remainingAmount);

                itemInSlot.itemQuantity -= itemsToRemove;
                slot.UpdateData();

                if (itemInSlot.itemQuantity == 0)
                {
                    slot.SetItem(null);
                }

                remainingAmount -= itemsToRemove;

                foreach (RequiredItems requiredItem in requiredItems)
                {
                    if (requiredItem.item == _item)
                    {
                        requiredItem.UpdateCollectedAmount(requiredItem.collectedRequiredItemAmount + itemsToRemove);

                        if (requiredItem.collectedRequiredItemAmount >= requiredItem.itemRequiredAmount)
                        {
                            requiredItem.itemColleted = true;
                        }
                        break;
                    }
                }

                if (remainingAmount <= 0)
                    break;
            }
        }
    }

    private void FinalBuildAfterDeliveryMaterials()
    {
        foreach (MeshRenderer meshRenderer in transform.root.GetComponentsInChildren<MeshRenderer>())
        {
            meshRenderer.material = finalMaterial;
        }

        Transform modelTransform = transform.root.transform.Find("Model");
        if (modelTransform != null)
        {
            Collider modelCollider = modelTransform.GetComponentInChildren<Collider>();
            if (modelCollider != null)
            {
                modelCollider.isTrigger = false;
            }
        }
    }

    private void UpdateUIOnCollectedAmountChanged()
    {
        transform.root.GetComponentInChildren<BuildableUI>()?.UpdateItemUI(requiredItems);
    }
}
