using Mirror;
using UnityEngine;

public class ItemData : NetworkBehaviour
{
    
    public Item item;
    
    [SyncVar]
    public int amount;

    public override string ToString()
    {
        return $"{item.name} {amount}x";
    }
}
