using UnityEngine;

public class Bazooka : Weapons
{
    public Bazooka(float damage = 30, string name = "Bazooka", int munition = 1) : base(damage, name, munition)
    {
        _Damage = damage;
        _Name = name;
        _Munition = munition;
    }
}