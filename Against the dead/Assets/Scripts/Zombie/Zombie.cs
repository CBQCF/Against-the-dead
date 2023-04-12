using System;
using Mirror;
using Unity;
using Unity.VisualScripting;
using UnityEngine;

public class Zombie : NetworkBehaviour
{
    public Transform position { get; set; }

    void Start()
    {
        position = GetComponent<Transform>();
        Stats st = this.AddComponent<Stats>();
        if (isServer)
        {
            st.health = 100;
        }
        
    }
}