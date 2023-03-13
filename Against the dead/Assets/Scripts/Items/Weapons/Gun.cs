using UnityEngine;

public class Gun : Weapon
{
    public Gun(float damage = 5, string name = "Pistol", int mag = 20) : base(damage, name, mag)
    {
    }
}