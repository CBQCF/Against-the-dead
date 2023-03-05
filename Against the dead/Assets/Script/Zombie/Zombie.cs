using Unity;
using UnityEngine;

public class Zombie
{
    public float _Life { get; set; }
    public string _Name { get; set; }
    public int _Damage { get; set; }
    public Vector3 _Spawn { get; set; }

    public Zombie(float life, string name, int damage,Vector3 spawn)
    {
        _Life = life;
        _Name = name;
        _Damage = damage;
        _Spawn = spawn;
    }

    public void Attack(PlayerController player)
    {
        player._Life -= _Damage;
    }
}