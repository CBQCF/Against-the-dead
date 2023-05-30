using System;
using Mirror;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public enum ItemType
{
    Weapon,
    Ammunition,
    Consumable,
}

[Serializable]
public class Item : NetworkBehaviour
{
    public GameObject refOnGround;
    public GameObject refOnInventory;

    public InventoryItem inventoryItem;
    
    // ItemData

    [SyncVar] public int Quantity;
    [SyncVar] public string Name; 
    [ReadOnly] public int UID;
    
    public ItemType type;

    public readonly int maxStack;

    public void RefreshCount()
    {
        inventoryItem.RefreshCount();
    }

    public override string ToString()
    {
        return $"{Name} x{Quantity}";
    }
}