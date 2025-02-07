using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum componentID
{
    EMPTY,
    HIGH_EXPLOSIVE,
    ARMOR_PIERCING,
    FRAGMENTATION,
    INCENDIARY,
    TUNGSTEN,
    RAILGUN,
    NUCLEAR
}

public static class Components
{
    static Component Empty =         new Component(0f, componentID.EMPTY,           Color.white);
    static Component HighExplosive = new Component(0f, componentID.HIGH_EXPLOSIVE,  new Color( 203 / 255f,  46 / 255f,  46 / 255f));
    static Component ArmorPiercing = new Component(0f, componentID.ARMOR_PIERCING,  new Color(  45 / 255f,  49 / 255f, 100 / 255f));
    static Component Fragmentation = new Component(0f, componentID.FRAGMENTATION,   new Color( 128 / 255f, 128 / 255f, 128 / 255f));
    static Component Incendiary =    new Component(0f, componentID.INCENDIARY,      new Color( 224 / 255f, 134 / 255f,  31 / 255f));
    static Component Tungsten =      new Component(0f, componentID.TUNGSTEN,        new Color( 240 / 255f, 240 / 255f, 240 / 255f));
    static Component Railgun =       new Component(0f, componentID.RAILGUN,         new Color( 105 / 255f, 243 / 255f, 243 / 255f));
    static Component Nuclear =       new Component(0f, componentID.NUCLEAR,         new Color(  31 / 255f, 207 / 255f,  76 / 255f));


    public static Component getComponent(componentID id)
    {
        switch (id)
        {
            case componentID.EMPTY:
                return Empty;
            case componentID.HIGH_EXPLOSIVE: 
                return HighExplosive;
            case componentID.ARMOR_PIERCING:
                return ArmorPiercing;
            case componentID.FRAGMENTATION:
                return Fragmentation;
            case componentID.INCENDIARY:
                return Incendiary;
            case componentID.TUNGSTEN:
                return Tungsten;
            case componentID.RAILGUN:
                return Railgun;
            case componentID.NUCLEAR:
                return Nuclear;
            default:
                return Empty;
        }
    }
}

public class Component
{
    private float cost;
    private Color color;
    private componentID id;

    public Component(float costIn, componentID idIn, Color colorIn)
    {
        this.cost = costIn;
        this.id = idIn;
        this.color = colorIn;
    }

    public Color GetColor() { return color; }
    public componentID getId() { return id; }
}
