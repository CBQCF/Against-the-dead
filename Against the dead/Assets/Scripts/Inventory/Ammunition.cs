using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Ammo")]
public class Ammunition : ScriptableObject
{
    public int Damage;
    public int AmmoRange;
}