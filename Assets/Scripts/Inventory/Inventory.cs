using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public enum ItemType
{ 
    Default,
    Weapon, 
    Consumable, 
    Tool,
    Resources
}
public class Inventory : MonoBehaviour
{
    public event System.Action<HandItemSO> OnHoldItem;

    public GameObject inventorySlotsParent;
    public GameObject hotbarSlotsParent;
    public GameObject hotbarSlotsBackgroundParent;
    public Image dragIconImage;
    
    private List<Slot> inventorySlots = new List<Slot>();
    private List<Slot> hotbarInventorySlots = new List<Slot>();
    private List<Image> hotbarBackgroundSlots = new List<Image>();
    private List<Slot> allInventorySlots = new List<Slot>();
    private ItemSO currentDraggedItem;

    private int currentDragSlotIndex = -1;
    private int lastHotBarSelectedSlot = 0;

    

    private void Start()
    {
        InitializeInventory();
    }
    private void InitializeInventory()
    {
        GetSlots();
        SubscribeToEvents();
        HotBarSelected(1);
    }
    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    private void SubscribeToEvents()
    {
        InputManager.Instance.OnInventoryDropItem += DropItem;
        InputManager.Instance.OnInteractHotBar += HotBarSelected;

    }
    private void UnsubscribeEvents()
    {
        InputManager.Instance.OnInventoryDropItem -= DropItem;
        InputManager.Instance.OnInteractHotBar -= HotBarSelected;
    }

    private void Update()
    {
        HandleDragAndDrop();
    }
    private void HandleDragAndDrop()
    {
        if (gameObject.activeInHierarchy && Input.GetMouseButtonDown(0))
        {
            DragInventoryIcon();
        }
        else if (currentDragSlotIndex != -1 && Input.GetMouseButtonUp(0) || currentDragSlotIndex != -1 && !gameObject.activeInHierarchy)
        {
            DropInventoryIcon();
        }
        dragIconImage.transform.position = Input.mousePosition;
    }
    public void AddItemToInventory(ItemSO _itemToAdd)
    {
        int leftoverQuantity = _itemToAdd.itemQuantity;
        Slot openSlot = null;
        for (int i = 0; i < allInventorySlots.Count; i++)
        {
            ItemSO heldItem = allInventorySlots[i].GetItem();

            if (heldItem != null && _itemToAdd.name == heldItem.name)
            {
                int freeSpaceInSlot = heldItem.maxItemQuantity - heldItem.itemQuantity;

                if (freeSpaceInSlot >= leftoverQuantity)
                {
                    heldItem.itemQuantity += leftoverQuantity;
                    allInventorySlots[i].UpdateData();
                    return;
                }
                else
                {
                    heldItem.itemQuantity = heldItem.maxItemQuantity;
                    leftoverQuantity -= freeSpaceInSlot;
                }
            }
            else if (heldItem == null)
            {
                if (!openSlot)
                    openSlot = allInventorySlots[i];
            }

            allInventorySlots[i].UpdateData();
        }

        if (leftoverQuantity > 0 && openSlot)
        {
            openSlot.SetItem(_itemToAdd);
            _itemToAdd.itemQuantity = leftoverQuantity;
        }
        else
        {
            _itemToAdd.itemQuantity = leftoverQuantity;
        }
    }
    public void AddItemToHotbar(ItemSO _itemToAdd)
    {
        var firstSlot = hotbarInventorySlots[0];

        if (firstSlot.HasItem())
        {
            DropItemGeneral(firstSlot);
        }

        firstSlot.SetItem(_itemToAdd);
    }
    private void DropItem()
    {
        for (int i = 0; i < allInventorySlots.Count; i++)
        {
            Slot currentSlot = allInventorySlots[i];
            if (currentSlot.hovered && currentSlot.HasItem())
            {
                DropItemGeneral(currentSlot);
                break;
            }
        }
    }
    private void DropItemGeneral(Slot _slot)
    {
        var item = _slot.GetItem();
        if (item != null)
        {
            for (int i = 0; i < item.itemQuantity; i++)
            {
                // Tworzymy obiekt na scenie dla ka¿dego upuszczanego przedmiotu.
                var go = Instantiate(item.prefab, FindObjectOfType<CameraController>().GetDropPoint().position, Quaternion.identity);
                go.GetComponent<Rigidbody>().isKinematic = false;
                go.GetComponent<Rigidbody>().AddForce(Vector3.forward * 5, ForceMode.Impulse); // Dostosuj si³ê, z jak¹ przedmiot jest rzucony.
            }
        }
        _slot.SetItem(null);
    }
    public void RemoveItemFromInventory()
    {
        Slot currentHotBarSlot = hotbarInventorySlots[lastHotBarSelectedSlot];
        currentHotBarSlot.SetItem(null);
    }

    private void GetSlots()
    {
        inventorySlots = inventorySlotsParent.GetComponentsInChildren<Slot>().ToList();
        hotbarInventorySlots = hotbarSlotsParent.GetComponentsInChildren<Slot>().ToList();
        hotbarBackgroundSlots = hotbarSlotsBackgroundParent.GetComponentsInChildren<Image>().ToList();

        allInventorySlots = inventorySlots.Concat(hotbarInventorySlots).ToList();

        hotbarInventorySlots.ForEach(slot => slot.OnItemChanged += HandleItemChangeInHotbar);

        allInventorySlots.ForEach(slot => slot.InitialiseSlot());
    }
    private void DragInventoryIcon()
    {
        for (int i = 0; i < allInventorySlots.Count; i++)
        {
            Slot currentSlot = allInventorySlots[i];
            if(currentSlot.hovered && currentSlot.HasItem())
            {
                currentDragSlotIndex = i;

                currentDraggedItem = currentSlot.GetItem();
                dragIconImage.sprite = currentDraggedItem.itemIcon;
                dragIconImage.color = new Color(1, 1, 1, 1);

                currentSlot.SetItem(null);
            }
        }
    }
    private void DropInventoryIcon()
    {
        dragIconImage.sprite = null;
        dragIconImage.color = new Color(1, 1, 1, 0);

        bool itemPlaced = false;

        for (int i = 0; i < allInventorySlots.Count; i++)
        {
            Slot currentSlot = allInventorySlots[i];
            if (currentSlot.hovered)
            {
                if (currentSlot.CanAcceptItem(currentDraggedItem))
                {
                    if (currentSlot.HasItem())
                    {
                        ItemSO itemToSwap = currentSlot.GetItem();
                        currentSlot.SetItem(currentDraggedItem);

                        allInventorySlots[currentDragSlotIndex].SetItem(itemToSwap);
                    }
                    else
                    {
                        currentSlot.SetItem(currentDraggedItem);
                    }
                    itemPlaced = true;
                    break;
                }
            }
        }

        if (!itemPlaced)
        {
            allInventorySlots[currentDragSlotIndex].SetItem(currentDraggedItem);
        }

        ResetDragVariables();
    }
    private void ResetDragVariables()
    {
        currentDraggedItem = null;
        currentDragSlotIndex = 1;
    }

    private int activeHotbarIndex = 0;

    private void HotBarSelected(int _keyNum)
    {
        int newActiveIndex = _keyNum - 1;
        if (newActiveIndex == activeHotbarIndex)
        {
            return;
        }

        activeHotbarIndex = newActiveIndex;
        lastHotBarSelectedSlot = activeHotbarIndex;

        for (int i = 0; i < hotbarInventorySlots.Count; i++)
        {
            hotbarBackgroundSlots[i].color = new Color32(255, 255, 255, 128);
        }
        hotbarBackgroundSlots[activeHotbarIndex].color = new Color32(255, 255, 255, 210);

        ItemSO selectedItem = hotbarInventorySlots[activeHotbarIndex].GetItem();
        HandItemSO handItem = selectedItem as HandItemSO;

        OnHoldItem?.Invoke(handItem);
    }
    private void HandleItemChangeInHotbar(ItemSO _newItem)
    {
        ItemSO currentItem = hotbarInventorySlots[activeHotbarIndex].GetItem();
        HandItemSO handItem = currentItem as HandItemSO;

        if (currentItem == _newItem)
        {
            OnHoldItem?.Invoke(handItem);
        }
        else
        {
            return;
        }
    }
}