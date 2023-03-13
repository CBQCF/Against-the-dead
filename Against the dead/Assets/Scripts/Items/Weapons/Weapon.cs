using Unity;
using UnityEngine;

public class Weapon : Item
{
    public float Damage { get; }
    public string Name { get; }
    public int Magazine { get; }

    public Weapon(float damage, string name, int mag)
    {
        Damage = damage;
        Name = name;
        Magazine = mag;
    }
}