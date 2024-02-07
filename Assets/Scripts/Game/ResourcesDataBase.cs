using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesDataBase : MonoBehaviour
{
    ItemSO[] itemsDataBase = null;

    public void Init()
    {
        itemsDataBase = Resources.LoadAll<ItemSO>("Scriptables/Items");
    }

    public ItemSO GetItemByName(string _itemName)
    {
        if (itemsDataBase.Length <= 0)
        {
            return null;
        }
        for (int i = 0; i < itemsDataBase.Length; i++)
        {
            if(itemsDataBase[i].itemName == _itemName)
            {
                return Instantiate(itemsDataBase[i]);
            }
        }
        return null;
    }
}
