using UnityEngine;
using UnityEngine.EventSystems;

public class BackgroundDrop : MonoBehaviour, IDropHandler
{
    public InventoryManager inventoryManager;
    
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            InventoryItem inventoryItem = dropped.GetComponent<InventoryItem>();
            inventoryManager.DropItem(inventoryItem);
        }
    }   
}
