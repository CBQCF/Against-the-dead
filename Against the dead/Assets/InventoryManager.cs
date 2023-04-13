using System;
using Mirror;
using UnityEngine;

public class InventoryManager : NetworkBehaviour
{
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    public GameObject mainInventory;
    [HideInInspector] public PlayerController pc;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchInventory();
        }
    }

    public void SwitchInventory()
    {
        mainInventory.SetActive(!mainInventory.activeInHierarchy);
        pc.inInterface = mainInventory.activeInHierarchy;
        
        if (mainInventory.activeInHierarchy)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    
    public void AddItem(Item item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnItem(item, slot);
                return;
            }
        }
    }

    public void SpawnItem(Item item, InventorySlot slot)
    {
        GameObject itemGameObject = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = itemGameObject.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }
}
