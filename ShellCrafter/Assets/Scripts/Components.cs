using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum componentID
{
    EMPTY,
    HIGH_EXPLOSIVE
}

public static class Components
{
    static Component Empty = new Component(0.0f);
    static Component HighExplosive = new Component(10.0f);

    public static Component getComponent(componentID id)
    {
        switch (id)
        {
            case componentID.EMPTY:
                return Empty;
            case componentID.HIGH_EXPLOSIVE: 
                return HighExplosive;
            default:
                return Empty;
        }
    }
}

public class Component
{
    private float cost;

    public Component(float costIn)
    {
        this.cost = costIn;
    }
}
