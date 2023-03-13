using Mirror;
using Unity;
using UnityEngine;

public class Zombie : NetworkBehaviour
{
    public float health { get; set; }
    public int damage { get; set; }
    
    private Transform position;

    public Zombie(float Health, int Damage, Vector3 spawn)
    {
        health = Health;
        damage = Damage;
        
        position = GetComponent<Transform>();
        position.position = spawn;
    }

    public void Attack(Player player)
    {
        player.health -= damage;
    }
}