using System.Collections.Generic;

namespace Items.Ammunition
{
    public enum Caliber
    {
        b9,     // 9mm
        b556,   // 5,56
        b762,   // 7,62
        b308    // .308
    }

    public abstract class Ammunition : Item
    {
        public int Damage { get; }
        public Caliber Caliber { get; }
    
    
        public Ammunition(int damage,  string name, Caliber caliber)
        {
            Caliber = caliber;
            Damage = damage;
            name = name;
        }
    }
}