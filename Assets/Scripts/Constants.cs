using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Weapon {
    public string name;
    public string type;
    public int damage;
}

public static class Constants
{
    public static readonly Weapon[] Weapons = new Weapon[]
    {
        new Weapon() { name = "Handgun1", type = "gun", damage = 3 },
        new Weapon() { name = "Handgun2", type = "gun", damage = 5 },
        new Weapon() { name = "Handgun2", type = "gun", damage = 10 },

        new Weapon() { name = "Axe1", type = "melee", damage = 5 },
        new Weapon() { name = "Axe2", type = "melee", damage = 10 },
        new Weapon() { name = "Axe2", type = "melee", damage = 20 },
    };
}