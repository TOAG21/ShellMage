using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject wavePicker;
    [SerializeField] GameObject infoPanel;

    EnemyManager em;
    int wave = 0;

    WaveNumber waveNumber;
    string dataPath;

    // Start is called before the first frame update
    void Awake()
    {
        dataPath = Application.persistentDataPath + "/data.json";
        //get waveNumber;

        em = GetComponent<EnemyManager>();
        baseMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (em.noEnemies)
        {
            em.SpawnWave(wave);
            wave++;
        }
    }

    public void playMenu()
    {
        wavePicker.SetActive(true);
    }

    public void Play()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Info()
    {
        infoPanel.SetActive(true);
    }

    public void baseMenu()
    {
        wavePicker.SetActive(false);
        infoPanel.SetActive(false);
    }

    public void Stop()
    {
        Application.Quit();
    }
}
