using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Weapon")]
public class Weapon : Item
{
    public enum WeaponType
    {
        Melee,
        SemiAuto,
        Auto,
    }

    public Ammunition ammo;
    public int capacity;
    public WeaponType wpType;
}
