using Items.Magazine;
using UnityEngine;

public abstract class Weapon : Item
    {
        public Magazine mag { get; protected set; }
    }