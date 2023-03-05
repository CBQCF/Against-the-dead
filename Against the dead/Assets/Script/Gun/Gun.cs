using UnityEngine;

public class Gun : Weapons
{
    public Gun(float damage = 5, string name = "Gun", int munition = 20) : base(damage, name, munition)
    {
        _Damage = damage;
        _Name = name;
        _Munition = munition;
    }
}