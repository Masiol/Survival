using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Inventory : MonoBehaviour
{
    
    public static Inventory instance = null;


    [SerializeField] private GameObject InventoryObject;
    [HideInInspector] public InventoryPanel inventoryPanel = null;
    [HideInInspector] public PanelOptions panelOptions = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    private void Start()
    {
        InputManager.Instance.OnInventory += ShowHideInventory;
        Init();       
    }
    private void OnDisable()
    {
        InputManager.Instance.OnInventory -= ShowHideInventory;
    }

    public void Init()
    {
        inventoryPanel = GetComponentInChildren<InventoryPanel>();
        inventoryPanel.Init();

        panelOptions = GetComponentInChildren<PanelOptions>();
        panelOptions.Init();

        InventoryObject.SetActive(false);
        
    }

    public void ShowHideInventory(bool _showInventory)
    {
        InventoryObject.SetActive(_showInventory);

        GameManager.instance.SetCursor(_showInventory);
    }

    public bool AddItems(Item _item)
    {   
        Debug.Log("inventory item");
    
        if (_item == null || _item.itemQuantity <= 0)
            return false;

        List<Slot> listSlots = GetSlots(inventoryPanel.grid);
        if (listSlots.Count <= 0)
            return false;

        Slot slotFound = listSlots.FirstOrDefault(
            s => s.currentItem != null 
            && s.currentItem.stackable 
            && s.currentItem.itemQuantity + _item.itemQuantity <= _item.maxItemQuantity);

        if(slotFound != null)
        {
            slotFound.currentItem.itemQuantity += _item.itemQuantity;
            slotFound.Refresh();
        }
        else
        {
            slotFound = listSlots.FirstOrDefault(s => s.currentItem == null);
            if (slotFound == null)
            {
                return false;
                // inventory full
            }
            slotFound.ChangeItem(_item);
        }
        return true;
    }

    private List<Slot> GetSlots(Transform _grid)
    {
        if (_grid == null || _grid.childCount <= 0)
            return null;

        List<Slot> slots = new List<Slot>();

        for (int i = 0; i < _grid.childCount; i++)
        {
            slots.Add(_grid.GetChild(i).GetComponent<Slot>());
        }
        return slots;
    }
}
