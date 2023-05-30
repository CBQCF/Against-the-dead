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

    public bool CanBePickedUp;
    
    [SyncVar] public int Quantity;
    [SyncVar] public string Name; 
    [ReadOnly] public int UID;
    
    public ItemType type;

    public readonly int maxStack;

    public void RefreshCount()
    {
        inventoryItem.RefreshCount();
    }

    [Server]
    public void VisibleOnGround(bool status)
    {
        CanBePickedUp = status;
        refOnGround.SetActive(status);
        refOnInventory.SetActive(!status);
        VisibleOnGroundRPC(status);

        if (status)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
        else
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }
    
    [ClientRpc]
    private void VisibleOnGroundRPC(bool status)
    {
        CanBePickedUp = status;
        refOnGround.SetActive(status);
        refOnInventory.SetActive(!status);
        
        if (status)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
        else
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }
    
    [Server]
    public void NewParent(NetworkIdentity parent)
    {
        if (parent is null)
        {
            transform.parent = null;
        }
        else
        {
            transform.parent = parent.transform;
        }
        
        NewParentRPC(parent);
    }
    
    [ClientRpc]
    private void NewParentRPC(NetworkIdentity parent)
    {
        if (parent is null)
        {
            transform.parent = null;
        }
        else
        {
            transform.parent = parent.transform;
        }
    }

    public override string ToString()
    {
        return $"{Name} x{Quantity}";
    }
}