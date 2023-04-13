using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugItem : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Item[] itemsToPickup;

    public void PickupItem(int id)
    {
        inventoryManager.AddItem(itemsToPickup[id]);
    }
}
