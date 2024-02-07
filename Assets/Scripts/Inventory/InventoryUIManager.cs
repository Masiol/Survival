using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    private GameObject inventory;

    private void Awake()
    {
        inventory = this.transform.GetChild(0).gameObject;   
    }
    private void Start()
    {
        InputManager.Instance.OnInventory += ToggleInventory;
        InputManager.Instance.Cancel += CloseInventory;
        CloseInventory();
    }
    private void OnDisable()
    {
        InputManager.Instance.OnInventory -= ToggleInventory;
        InputManager.Instance.Cancel -= CloseInventory;
    }
    private void ToggleInventory()
    {
        bool isActive = inventory.activeSelf;
        inventory.SetActive(!isActive);
        GameManager.instance.SetCursor(!isActive);
    }

    private void CloseInventory()
    {
        if (inventory.activeSelf)
        {
            inventory.SetActive(false);
            GameManager.instance.SetCursor(false);
        }
    }
}
