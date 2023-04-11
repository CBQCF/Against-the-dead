using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

public class ItemInteraction : NetworkBehaviour
{
    private Transform cam;
    [SerializeField] private LayerMask itemLayer;
    private Inventory inventorySystem;

    [SerializeField] private TextMeshProUGUI txt_HoveredItem;

    void Start()
    {
        cam = Camera.main.transform;
        itemLayer = LayerMask.NameToLayer("UI");
        inventorySystem = GetComponent<Inventory>();
        txt_HoveredItem = GameObject.FindWithTag("Main UI").transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, 2, itemLayer))
        {
            ItemObject hitObject = hit.collider.GetComponent<ItemObject>();
            
            if (!hitObject)
            {
                return;
            }
            
            txt_HoveredItem.text = $"Press 'F' to pick up {hitObject.amount}x {hitObject.itemStats.name}";

            if (Input.GetKeyDown(KeyCode.F))
            {
                inventorySystem.PickUpItem(hitObject);
            }
        }
        else
        {
            txt_HoveredItem.text = "";
        }
    }
}
