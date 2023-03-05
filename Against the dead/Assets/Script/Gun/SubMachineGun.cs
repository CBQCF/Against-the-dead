using UnityEngine;

public class SubMachineGun : Weapons
{
    public SubMachineGun(float damage = 3, string name = "SubmachineGun", int munition = 30) : base(damage, name, munition)
    {
        _Damage = damage;
        _Name = name;
        _Munition = munition;
    }
}