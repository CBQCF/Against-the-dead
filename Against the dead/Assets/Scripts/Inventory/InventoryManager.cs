using System;
using Mirror;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    public GameObject mainInventory;
    public TextMeshProUGUI interactionText;
    [HideInInspector] public Player player;


    private Camera mainCamera;
    private int pickupRange = 3;
    private int selectedSlot;

    private void Start()
    {
        inventorySlots[selectedSlot].Select();
        mainCamera = Camera.main;
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

        RaycastHit lookat;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out lookat, pickupRange))
        {
            ItemData item = lookat.transform.gameObject.GetComponent<ItemData>();
            if (item is not null)
            {
                interactionText.gameObject.SetActive(true);
                interactionText.text = item.ToString();
                if (Input.GetKeyDown(KeyCode.F))
                {
                    PickupItem(item);
                }
            }
            else
            {
                interactionText.gameObject.SetActive(false);
            }
        }
    }
    
    public void PickupItem(ItemData itemData)
    {
        if (AddItem(itemData.item))
        {
            player.DestroyItem(itemData.gameObject.GetComponent<NetworkIdentity>().netId);
        }
        else
        {
            
        }
    }
    
    
    
    public void SwitchInventory()
    {
        mainInventory.SetActive(!mainInventory.activeInHierarchy);
        player.playerController.inInterface = mainInventory.activeInHierarchy;

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

    public void DropItem(InventoryItem item)
    {
        Vector3 spawnPos = player.transform.position + player.transform.forward * 2;
        player.CmdSpawnItem(NetworkManager.singleton.spawnPrefabs.IndexOf(item.item.prefab), item.amount, spawnPos);
        Destroy(item.gameObject);
    }
}