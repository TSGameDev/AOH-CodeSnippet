using System.Collections;
using System.Collections.Generic;
using TSGameDev.Inventories;
using UnityEngine;

public class StartingItems : MonoBehaviour
{
    [SerializeField] Inventory inventoryToPopulate;
    [SerializeField] List<InventoryItem> startingItems = new();

    private void Awake()
    {
        foreach(InventoryItem item in startingItems)
        {
            inventoryToPopulate.AddToFirstEmptySlot(item, 1);
        }
    }
}
