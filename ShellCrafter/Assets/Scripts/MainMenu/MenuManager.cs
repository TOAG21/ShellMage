using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject wavePicker;
    [SerializeField] GameObject infoPanel;
    [SerializeField] GameObject page1;
    [SerializeField] GameObject page2;
    [SerializeField] TMP_Dropdown StupidDropdown;
    [SerializeField] Slider slider;

    EnemyManager em;
    int wave = 0;

    WaveNumber waveNumber;
    string dataPath;

    // Start is called before the first frame update
    void Awake()
    {
        dataPath = Application.streamingAssetsPath + "/data.json";
        try
        {
            string json = System.IO.File.ReadAllText(dataPath);
            waveNumber = JsonUtility.FromJson<WaveNumber>(json);
        }
        catch
        {
            waveNumber = new WaveNumber();
            string data = JsonUtility.ToJson(waveNumber);
            System.IO.File.WriteAllText(dataPath, data);
        }

        em = GetComponent<EnemyManager>();
        baseMenu();


        StupidDropdown.ClearOptions();
        List<string> list = new List<string>();
        for (int i = 0; i < waveNumber.unlocks.Length; i++)
        {
            if (waveNumber.unlocks[i])
            {
                list.Add((i * 10).ToString());
            }
        }
        StupidDropdown.AddOptions(list);
        slider.value = waveNumber.volume;

        Time.timeScale = 1f;
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
        waveNumber.selectedWave = StupidDropdown.value * 10;
        waveNumber.volume = slider.value;
        string data = JsonUtility.ToJson(waveNumber);
        System.IO.File.WriteAllText(dataPath, data);
        SceneManager.LoadScene("SampleScene");
    }

    public void Info()
    {
        infoPanel.SetActive(true);
        page1.SetActive(true);
        page2.SetActive(false);
    }
    public void togglePage()
    {
        if (page1.activeSelf)
        {
            page1.SetActive(false);
            page2.SetActive(true);
        }
        else
        {
            page1.SetActive(true);
            page2.SetActive(false);
        }
    }

    public void baseMenu()
    {
        wavePicker.SetActive(false);
        page1.SetActive(true);
        page2.SetActive(false);
        infoPanel.SetActive(false);
    }

    public void Stop()
    {
        Application.Quit();
    }
}
