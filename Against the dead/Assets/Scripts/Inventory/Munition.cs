using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ammunition", menuName = "Create new ammo")]
[Serializable]
public class Ammunition : Item
{
    public enum Caliber
    {
        b9, // 9mm (Pistol)
        b556, // 5,56 (Saug...)
        b762, // 7,62 (Ak)
        b12g, // 12 gauge (Shotgun)
        mele // Fireaxe
    }

    public int damage { get; }
    public Caliber caliber { get; }
    
}