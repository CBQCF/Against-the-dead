using UnityEngine;
using UnityEngine.EventSystems;

public class BackgroundDrop : MonoBehaviour, IDropHandler
{
    public InventoryManager inventoryManager;

    public void CloseInventory()
    {
        inventoryManager.SwitchInventory();
    }
    
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            InventoryItem inventoryItem = dropped.GetComponent<InventoryItem>();
            inventoryManager.InventoryInteraction(inventoryItem.item.netIdentity, InventoryManager.InvAction.Drop);
        }
    }   
}
