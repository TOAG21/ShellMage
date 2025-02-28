using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaveNumber
{
    bool[] unlocks = new bool[5]; //0-40 by 10
    int selectedWave = 0;
    float volume = 1.0f;
}
