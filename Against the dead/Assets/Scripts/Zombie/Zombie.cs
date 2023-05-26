using System;
using Mirror;
using Unity;
using Unity.VisualScripting;
using UnityEngine;

public class Zombie : NetworkBehaviour
{
    public Transform position { get; set; }

    public Bar healthBar;

    void Start()
    {
        position = GetComponent<Transform>();
        Stats st = this.AddComponent<Stats>();
        st.healthBar = healthBar;
        if (isServer)
        {
            st.health = 100;
        }
        
    }
}