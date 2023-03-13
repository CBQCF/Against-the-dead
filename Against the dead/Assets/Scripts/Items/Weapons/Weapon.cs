using Unity;
using UnityEngine;

public class Weapon : Item
{
    public float Damage { get; set;}
    public string Name { get; set;}
    public int Magazine { get; set;}

    public Weapon(float damage, string name, int mag)
    {
        Damage = damage;
        Name = name;
        Magazine = mag;
    }
}