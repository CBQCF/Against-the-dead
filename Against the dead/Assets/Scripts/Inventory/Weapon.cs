using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Items.Ammunition;
using UnityEngine;

public class Weapon : Item
{
    public enum WeaponType
    {
        Melee,
        SemiAuto,
        Auto,
    }
    public Ammunition Ammo { get; }
    public int Capacity { get; }

    public WeaponType wpType { get; private set; }
    
    public Weapon(Ammunition ammo, ItemType type, string name, int capacity, WeaponType wptype)
    {
        name = name;
        Ammo = ammo;
        Capacity = capacity;
        type = type;
        wpType = wptype;
    }

    public void changeType(WeaponType newType)
    {
        wpType = newType;
    }
    
}
