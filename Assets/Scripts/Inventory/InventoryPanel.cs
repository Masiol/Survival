using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPanel : MonoBehaviour
{
    [HideInInspector] public Transform grid = null;

    public void Init()
    {
        grid = transform.Find("Slots");
    }
}
