using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSO : ScriptableObject
{
    [SerializeField] private string ItemName = string.Empty;
    public string itemName => ItemName;

    [SerializeField] private string ItemDesc = string.Empty;
    public string itemDesc => ItemDesc;

    [SerializeField] private Sprite ItemIcon = null;
    public Sprite itemIcon => ItemIcon;

    public int itemQuantity;

    [SerializeField] private int MaxItemQuantity = 1;
    public int maxItemQuantity => MaxItemQuantity;

    [SerializeField] private bool Stackable;
    public bool stackable => Stackable;

    [SerializeField] private GameObject Prefab;
    public GameObject prefab => Prefab;

    [SerializeField] private ItemType itemType = ItemType.Default;
    public ItemType ItemType => itemType;
}
