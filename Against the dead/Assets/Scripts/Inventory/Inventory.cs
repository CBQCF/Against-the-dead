using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Inventory : NetworkBehaviour
{
    public Item[] Inv;
    
    // Start is called before the first frame update
    void Start()
    {
        Inv = new Item[24];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
