using System;
using System.Collections.Generic;
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

    public Item inHands { get; private set; }
    public int pickupRange = 3;
    [HideInInspector] public int selectedSlot { get; private set; }

    public static InventoryManager Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }

            instance = FindObjectOfType<InventoryManager>();
            return instance;
        }
    }

    private static InventoryManager instance;
    
    private void Start()
    {
        inventorySlots[selectedSlot].Select();
        instance = this;
    }

    public void ChangeSelectedSlot(int newSelSlot)
    {
        inventorySlots[selectedSlot].Deselect();

        inventorySlots[newSelSlot].Select();
        selectedSlot = newSelSlot;
    }

    public void WeaponInHands()
    {
        inHands = GetSelectedItem(false);
        if (inHands is not null)
        {
            if (inHands.type == Item.ItemType.Weapon)
            {
                player.playerWeapon.ChangeVisible(inHands.prefab);
                player.playerShoot.damage = inHands.prefab.GetComponent<WeaponData>().ammo.Damage;
                player.playerShoot.range = inHands.prefab.GetComponent<WeaponData>().ammo.AmmoRange;
                return;
            }
        }

        player.playerWeapon.HideWeapon();
        player.playerShoot.damage = 10;
        player.playerShoot.range = 1;
    }

    public void PickupItem(ItemData itemData)
    {
        if (AddItem(itemData.item))
        {
            player.Destroy(itemData.gameObject.GetComponent<NetworkIdentity>().netId);
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

    public override string ToString()
    {
        List<ItemSerialize> items = new List<ItemSerialize>();
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventoryItem itemInSlot = inventorySlots[i].GetComponentInChildren<InventoryItem>();
            if (itemInSlot is not null)
            {
                ItemSerialize ser = new ItemSerialize();
                ser.amount = itemInSlot.amount;
                ser.name = itemInSlot.item.name;
                
                items.Add(ser);
            }
        }

        ListWrapper listWrapper = new ListWrapper();
        listWrapper.list = items;
        
        return JsonUtility.ToJson(listWrapper);
    }
}