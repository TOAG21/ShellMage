using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    [SerializeField] TMP_Text health;
    [SerializeField] TMP_Text wave;

    [SerializeField] Image[] slots;
    [SerializeField] Image[] comps;
    //0 = lock, 1 = knob, 2-10 are comps in order
    [SerializeField] Sprite[] images;


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

    public void setLocks(bool[] slotsIn, bool[] compsIn)
    {
        for (int i = 0; i < slotsIn.Length; i++)
        {
            if (slotsIn[i])
            {
                slots[i].sprite = images[1];
                slots[i].gameObject.SetActive(false);
            }
            else
            {
                slots[i].sprite = images[0];
            }
        }

        for (int i = 0; i < compsIn.Length; i++)
        {
            if (compsIn[i])
            {
                comps[i].sprite = images[i + 2];
            }
            else
            {
                comps[i].sprite = images[0];
            }
        }
    }
}
