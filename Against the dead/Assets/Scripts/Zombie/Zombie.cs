using System;
using Mirror;
using Unity;
using Unity.VisualScripting;
using UnityEngine;

public class Zombie : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnDamageTaken))] public float health;
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

    private void OnDamageTaken(float _old, float _new)
    {
        if (_new <= 0)
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