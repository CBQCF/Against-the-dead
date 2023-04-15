using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class WeaponData : NetworkBehaviour
{
    public int MagSize;
    public int ammoInMag;
    public Ammunition ammo;

    public float cadence;
    public Selector selector;
    
    public enum Selector
    {
        semi,
        auto,
    }
}
