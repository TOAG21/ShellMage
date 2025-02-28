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

    string filepath;
    string filepathMenu;

    public bool menu = false;

    // Start is called before the first frame update
    void Awake()
    {
        string json;
        filepath = Application.persistentDataPath + "/WaveData.json";
        filepathMenu = Application.persistentDataPath + "/WaveDataM.json";
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
        if (waveNum > waveHolder.waves.Length - 1) { waveNum = waveLoop(waveNum); }

        Wave wave = waveHolder.waves[waveNum];
        noEnemies = false;
        for (int i = 0; i < wave.enemyType.Length; i++)
        {
            enemies.Add(GameObject.Instantiate(EnemyPrefabs[wave.enemyType[i]], wave.enemyPos[i], Quaternion.Euler(Vector3.zero)));
        }
    }

    int waveLoop(int waveIn)
    {
        if (waveIn > waveHolder.waves.Length - 1)
        {
            waveIn -= waveHolder.waves.Length;
            waveIn = waveLoop(waveIn);
        }

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
