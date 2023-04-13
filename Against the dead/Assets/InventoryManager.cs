using System;
using Mirror;
using UnityEngine;

public class InventoryManager : NetworkBehaviour
{
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    public GameObject mainInventory;
    [HideInInspector] public PlayerController pc;

    private int selectedSlot;

    private void Start()
    {
        inventorySlots[selectedSlot].Select();
    }

    private void ChangeSelectedSlot(int newSelSlot)
    {
        inventorySlots[selectedSlot].Deselect();
        
        inventorySlots[newSelSlot].Select();
        selectedSlot = newSelSlot;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchInventory();
        }

        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 9)
            {
                ChangeSelectedSlot(number - 1);
            }
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
    
    public bool AddItem(Item item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.amount < item.maxStack)
            {
                itemInSlot.amount++;
                itemInSlot.RefreshCount();
                return true;
            }
        }
        
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnItem(item, slot);
                return true;
            }
        }

        return false;
    }

    public void SpawnItem(Item item, InventorySlot slot)
    {
        GameObject itemGameObject = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = itemGameObject.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }

    public Item GetSelectedItem(bool use)
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null)
        {
            if (use)
            {
                itemInSlot.amount--;
                if (itemInSlot.amount <= 0)
                {
                    Destroy(itemInSlot.gameObject);
                }
                else
                {
                    itemInSlot.RefreshCount();
                }
            }
            
            return itemInSlot.item;
        }

        return null;

    }
}
