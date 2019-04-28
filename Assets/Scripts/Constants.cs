using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Weapon {
    public string name;

    public Weapon(string name)
    {
        this.name = name;
    }
}

class Weapons: List<Weapon>
{
    public void Add(string name)
    {
        Add(new Weapon(name));
    }
}

public static class Constants
{
    public static readonly Weapon[] weapons = new Weapon[]
    {
        new Weapon() { name = "Handgun" }
    };
}
