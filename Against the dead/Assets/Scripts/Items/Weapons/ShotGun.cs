using UnityEngine;

public class ShotGun : Weapon
{
    public ShotGun(float damage = 15, string name = "ShotGun", int mag = 5) : base(damage, name, mag)
    {
    }
}