using System.Collections.Generic;

public class Ammunition : Item
{
    public enum Caliber
    {
        b9, // 9mm
        b556, // 5,56
        b762, // 7,62
        b308 // .308
    }

    public int damage { get; }
    public Caliber caliber { get; }
    
}