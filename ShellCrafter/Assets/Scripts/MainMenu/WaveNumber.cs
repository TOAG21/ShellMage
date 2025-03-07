using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaveNumber
{
    public bool[] unlocks = new bool[] { true, false, false, false, false }; //0-40 by 10
    public bool[] compUnlocks = new bool[] {true, false, false, false, false, false, false, false }; //components
    public bool[] slots = new bool[] {true, false, false, false }; //comp slots
    public int selectedWave = 0;
    public float volume = 1.0f;
}
