using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ammunition", menuName = "Create new ammo")]
[Serializable]
public class Ammunition : Item
{
    public int damage;
    public int range;
}