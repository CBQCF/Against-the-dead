using System;
using Mirror;
using Unity;
using Unity.VisualScripting;
using UnityEngine;

public class Zombie : NetworkBehaviour
{
    [SyncVar(hook = nameof(TakeDamage))] public float health;
    public int damage { get; set; }
    
    public Transform position { get; set; }

    void Start()
    {
        if (isServer)
        {
            this.AddComponent<ZombieCharacterControl>();
        }

        position = GetComponent<Transform>();
    }

    private void TakeDamage()
    {
        if (health <= 0)
        {
            NetworkServer.Destroy(this.gameObject);
        }
    }

    [Command]
    public void SetHealth(float newHealth)
    {
        health = newHealth;
    }
    
    
    [Command]
    public void Attack(Player player)
    {
        player.health -= damage;
    }
}