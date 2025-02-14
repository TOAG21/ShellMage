using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UImanager : MonoBehaviour
{
    [SerializeField] TMP_Text health;
    [SerializeField] TMP_Text wave;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setHealth(float healthIn)
    {
        health.text = healthIn.ToString();
    }
    public void setWave(int waveIn)
    {
        wave.text = waveIn.ToString();
    }
}
