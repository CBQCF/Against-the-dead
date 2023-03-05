using UnityEngine;

public class ShotGun : Weapons
{
    public ShotGun(float damage = 15, string name = "ShotGun", int munition = 5) : base(damage, name, munition)
    {
        _Damage = damage;
        _Name = name;
        _Munition = munition;
    }
}