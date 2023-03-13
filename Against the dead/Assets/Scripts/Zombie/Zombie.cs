using System;
using Mirror;
using Unity;
using UnityEngine;

public class Zombie : NetworkBehaviour
{
    public float health { get; set; }
    public int damage { get; set; }
    
    public Transform position { get; set; }

    void Start()
    {
        position = GetComponent<Transform>();
    }
    public void Attack(Player player)
    {
        player.health -= damage;
    }
}