using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class DebugItem : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Item[] itemsToPickup;
    
    public void GiveItem(int id)
    {
        for (int i = 0; i < NetManager.Instance.spawnPrefabs.Count; i++)
        {
            GameObject spawnable = NetManager.Instance.spawnPrefabs[i];
            Item item = spawnable.GetComponent<Item>();
            if (item is not null)
            {
                if (item.UID == itemsToPickup[id].UID)
                {
                    NotificationManager.Instance.SuccessNetwork("Giving : " + itemsToPickup[id].Name);
                    inventoryManager.InventoryGive(i);
                    return;
                }
            }
        }
        NotificationManager.Instance.Error($"Error while giving {itemsToPickup[id].Name}!");
    }
}
