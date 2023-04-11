using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Inventory : NetworkBehaviour
{
    [SerializeField] public Slot[] slots = new Slot[18];
    [SerializeField] private GameObject inventoryUI;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        inventoryUI = GameObject.FindWithTag("Main UI").transform.GetChild(0).gameObject;
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = inventoryUI.transform.GetChild(i).GetComponent<Slot>();
            if (slots[i].item == null)
            {
                for (int j = 0; j < slots[i].transform.childCount; j++)
                {
                    slots[i].transform.GetChild(j).gameObject.SetActive(false);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!inventoryUI.activeInHierarchy && Input.GetKeyDown(KeyCode.E))
        {
            inventoryUI.SetActive(true);
        }
        else if (inventoryUI.activeInHierarchy && Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape))
        {
            inventoryUI.SetActive(false);
        }
    }

    public void PickUpItem(ItemObject item)
    {
        foreach (Slot slot in slots)
        {
            if (slot.item != null && slot.item.id == item.itemStats.id && slot.Amount < slot.item.maxStack)
            {
                if (slot.item.maxStack <= slot.Amount + item.amount)
                {
                    slot.Amount += item.amount;
                    Destroy(item.gameObject);
                    slot.SetStats();
                    return;
                }
                else
                {
                    int remaining = slot.item.maxStack - slot.Amount;
                    item.amount = slot.Amount + item.amount - slot.item.maxStack;
                    slot.Amount += remaining;
                    slot.SetStats();
                    PickUpItem(item);
                    return;
                }
            }

            if (slot.item == null)
            {
                slot.item = item.itemStats;
                slot.Amount = item.amount;
                Destroy(item.gameObject);
                return;
            }
        }
    }
}
