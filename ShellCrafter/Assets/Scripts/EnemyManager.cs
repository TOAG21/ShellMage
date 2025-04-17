using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //this class will spawn enemies in based on data from a json file. it will track whether all enemies are dead or not, and it will be told by game manager to spawn the next wave

    //basic, swarmer, bomber, captain, armor, big armor
    [SerializeField] GameObject[] EnemyPrefabs;
    WaveHolder waveHolder;
    public bool noEnemies = true;
    List<GameObject> enemies = new List<GameObject>();
    int loopTracker = 0;
    float healthMult = 1.0f;

    string filepath;
    string filepathMenu;

    public bool menu = false;

    // Start is called before the first frame update
    void Awake()
    {
        string json;
        filepath = Application.streamingAssetsPath + "/WaveData.json";
        filepathMenu = Application.streamingAssetsPath + "/WaveDataM.json";
        //Debug.Log(filepath);

        /*test serialization
        WaveHolder wh = new WaveHolder();
        wh.waves = new Wave[2];
        wh.waves[0] = new Wave();
        wh.waves[0].enemyType = new int[] { 1, 1};
        wh.waves[0].enemyPos = new Vector3[] { new Vector3(0, 10, 0), new Vector3(-1, 10, 0)};
        wh.waves[1] = new Wave();
        wh.waves[1].enemyType = new int[] { 1, 1 };
        wh.waves[1].enemyPos = new Vector3[] { new Vector3(0, 10, 0), new Vector3(-1, 10, 0) };

        json = JsonUtility.ToJson(wh);
        //System.IO.File.WriteAllText(filepath, json);
        */

        //read data in
        if (!menu)
        {
            json = System.IO.File.ReadAllText(filepath);
        }
        else
        {
            json = System.IO.File.ReadAllText(filepathMenu);
        }
        waveHolder = JsonUtility.FromJson<WaveHolder>(json);
    }

    // Update is called once per frame
    void Update()
    {
        int listLength = enemies.Count;
        Enemy script;
        for (int i = 0; i < listLength; i++)
        {
            script = enemies[i].GetComponent<Enemy>();
            if (script.health <= 0)
            {
                if (script.bomber) { script.Detonate(); }
                Destroy(enemies[i]);
                enemies.RemoveAt(i);
                i--;
                listLength--;
            }
        }
        if (enemies.Count <= 0) { noEnemies = true; }
    }

    public void SpawnWave(int waveNum)
    {
        waveNum = waveNum - loopTracker * waveHolder.waves.Length;
        if (waveNum > waveHolder.waves.Length - 1) { waveNum = waveLoop(waveNum); }

        Wave wave = waveHolder.waves[waveNum];
        noEnemies = false;
        for (int i = 0; i < wave.enemyType.Length; i++)
        {
            enemies.Add(GameObject.Instantiate(EnemyPrefabs[wave.enemyType[i]], wave.enemyPos[i], Quaternion.Euler(Vector3.zero)));
        }
        for (int i = 0; i < wave.enemyType.Length; i++)
        {
            enemies[i].GetComponent<Enemy>().health *= healthMult;
            enemies[i].GetComponent<Enemy>().multSHP(healthMult);
        }
    }

    int waveLoop(int waveIn)
    {
        loopTracker++;
        waveIn -= waveHolder.waves.Length;

        //enemy health increases with each loop     ----      50% more?
        healthMult += 0.5f;

        return waveIn;
    }
}

[Serializable]
public class WaveHolder
{
    public Wave[] waves;
}

[Serializable]
public class Wave
{
    public int[] enemyType;
    //positions must be between (-9,9) and (6,15)
    public Vector3[] enemyPos;
}

//basic, swarmer, bomber, captain, armor, big armor
/*
 Wave Data Wiki:
Wave 0: 1 basic
Wave 1: 3 basic enemies
Wave 2: 1 armor + 2 basic behind - piercing tutorial
Wave 3: 2 armor + 2 basic
Wave 4: 3 groups of 2 swarmers
Wave 5: swarmers circling an armor
Wave 6: 2 groups of 4 basics - diamond one normal basic
Wave 7: 2 groups of 4 armors - flat
Wave 8: 5 armors 5 basics
Wave 9: 1 captain
Wave 10: 9 basics 2 bombers
Wave 11: 5 armors 5 bombers V fligth pattern
Wave 12: 5 armors single file
Wave 13: 2 captains
Wave 14: 3 armors 4 basics all in line
Wave 15:
Wave 16:
Wave 17: mass of swarmers horseshoe shape

Wave x Layer of basics behind armors
Wave x+1 Swarmer swarm
wave x+2 bombers gaurded by basics
 
 
 
 
 */
