using System;
using Mirror;
using Unity;
using Unity.VisualScripting;
using UnityEngine;

public class Zombie : NetworkBehaviour
{
    public Transform position { get; set; }

    public Bar healthBar;
    private AudioSource death;

    void Start()
    {
        death = gameObject.AddComponent<AudioSource>();
        death.clip = Resources.Load<AudioClip>("Sound/Mort-Zombie_1");
        position = GetComponent<Transform>();
        Stats st = GetComponent<Stats>();
        st.healthBar = healthBar;
        if (isServer)
        {
            st.health = 100;
        }
        
    }
}