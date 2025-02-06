using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum componentID
{
    EMPTY,
    HIGH_EXPLOSIVE,
    ARMOR_PIERCING
}

public static class Components
{
    static Component Empty = new Component(0.0f, componentID.EMPTY, Color.white);
    static Component HighExplosive = new Component(0f, componentID.HIGH_EXPLOSIVE, new Color(203 / 255f,  66 / 255f,  46 / 255f));
    static Component ArmorPiercing = new Component(0f, componentID.ARMOR_PIERCING, new Color( 45 / 255f,  49 / 255f, 100 / 255f));

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
