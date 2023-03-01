using Unity;
using UnityEngine;

public class Weapons
{
    public float _Damage { get; set;}
    public string _Name { get; set;}
    public int _Munition { get; set;}

    public Weapons(float damage, string name, int munition)
    {
        _Damage = damage;
        _Name = name;
        _Munition = munition;
    }
}