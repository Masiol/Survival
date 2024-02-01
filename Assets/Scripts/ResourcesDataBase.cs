using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesDataBase : MonoBehaviour
{
    private Item[] itemsDataBase = null;

    public void Init()
    {
        itemsDataBase = Resources.LoadAll<Item>("Scriptables/Items");
    }

    public Item GetItemByName(string _itemName)
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
