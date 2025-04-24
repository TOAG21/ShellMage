using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject turret;
    [SerializeField] UImanager ui;
    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject[] componentIcons;
    [SerializeField] Sprite[] icons;
    [SerializeField] AudioClip CannonShot;
    [SerializeField] AudioClip[] ComponentAudios;
    float[] audioVolumes;
    [SerializeField] GameObject Enemy1;


    private Bullet bullet;
    bool enhancementEleven = false;

    int wave = 0;
    float health = 40f;
    EnemyManager em;
    bool paused = false;
    bool gameover = false;

    Component[] shell = new Component[4];
    int shellIndex = 0;
    int shellLimit = 0;
    float fireCooldown = 0.0f;
    bool ghostInput = false;
    bool reloadAudio = true;

    Vector3 mousePos;

    string dataPath;
    WaveNumber dataFile;

    // Start is called before the first frame update
    void Awake()
    {
        dataPath = Application.streamingAssetsPath + "/data.json";
        string json = System.IO.File.ReadAllText(dataPath);
        dataFile = JsonUtility.FromJson<WaveNumber>(json);

        wave = dataFile.selectedWave;

        em = GetComponent<EnemyManager>();

        pauseUI.SetActive(false);

        audioVolumes = new float[] {1.0f, 0.9f, 0.9f, 0.7f, 0.4f, 1.0f, 0.9f, 1.0f };
        foreach (bool slot in dataFile.slots)
        {
            if(slot)
            {
                shellLimit++;
            }
        }
        ClearShells();
        ui.setLocks(dataFile.slots, dataFile.compUnlocks);
    }

    // Update is called once per frame
    void Update()
    {
        pointTurret();
        if(fireCooldown >= 0.0f) { fireCooldown -= Time.deltaTime; }

        if (!reloadAudio && fireCooldown < 0.0f)
        {
            AudioSource.PlayClipAtPoint(ComponentAudios[8], Vector3.zero);
            reloadAudio = true;
        }

        InputCheck();        
        
        if(em.noEnemies)
        {
            em.SpawnWave(wave);
            ui.setWave(wave);

            unlockCheck(wave);
            ui.setLocks(dataFile.slots, dataFile.compUnlocks);

            wave++;
        }
    }

    void InputCheck()
    {       
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
            return;
        }
        if (paused) { return; }

        //load shells
        if (Input.GetKeyDown(KeyCode.Q) && dataFile.compUnlocks[0])
        {
            LoadShell(Components.getComponent(componentID.HIGH_EXPLOSIVE));
        }
        if (Input.GetKeyDown(KeyCode.W) && dataFile.compUnlocks[1])
        {
            LoadShell(Components.getComponent(componentID.ARMOR_PIERCING));
        }
        if (Input.GetKeyDown(KeyCode.E) && dataFile.compUnlocks[2])
        {
            LoadShell(Components.getComponent(componentID.FRAGMENTATION));
        }
        if (Input.GetKeyDown(KeyCode.R) && dataFile.compUnlocks[3])
        {
            LoadShell(Components.getComponent(componentID.INCENDIARY));
        }
        ////////////////////////////////////////////
        if (Input.GetKeyDown(KeyCode.A) && dataFile.compUnlocks[4])
        {
            LoadShell(Components.getComponent(componentID.TUNGSTEN));
        }
        if (Input.GetKeyDown(KeyCode.S) && dataFile.compUnlocks[5])
        {
            LoadShell(Components.getComponent(componentID.RAILGUN));
        }
        if (Input.GetKeyDown(KeyCode.D) && dataFile.compUnlocks[6])
        {
            LoadShell(Components.getComponent(componentID.NUCLEAR));
        }
        if (Input.GetKeyDown(KeyCode.F) && dataFile.compUnlocks[7])
        {
            LoadShell(Components.getComponent(componentID.ENHANCEMENT));
        }

        /*for dev testing*/
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (!dataFile.slots[1]) { shellLimit++; }
            dataFile.slots[1] = true;
            dataFile.compUnlocks[1] = true;
            dataFile.compUnlocks[2] = true;
            dataFile.compUnlocks[3] = true;
            dataFile.unlocks[1] = true;
            if (!dataFile.slots[2]) { shellLimit++; }
            dataFile.slots[2] = true;
            dataFile.compUnlocks[4] = true;
            dataFile.compUnlocks[5] = true;
            dataFile.compUnlocks[6] = true;
            dataFile.unlocks[2] = true;
            if (!dataFile.slots[3]) { shellLimit++; }
            dataFile.slots[3] = true;
            dataFile.compUnlocks[7] = true;
            dataFile.unlocks[3] = true;
            dataFile.unlocks[4] = true;
        }
        

        //shoot turret
        if ((Input.GetMouseButtonDown(0) || ghostInput) && fireCooldown < 0.0f)
        {
            Shoot();
            fireCooldown = 1.0f;
            ghostInput = false;
            reloadAudio = false;
            ClearShells();
        }
        else if (Input.GetMouseButtonDown(0) && fireCooldown < 0.25f)
        {
            ghostInput = true;
        }

        //airburst last shell
        if (bullet != null)
        {
            bullet.Airburst();
        }
    }

    void Shoot()
    {

        bullet = GameObject.Instantiate(bulletPrefab, turret.transform.GetChild(0).position, turret.transform.rotation).GetComponent<Bullet>();
        AddStats();
        bullet.volume = dataFile.volume;

        if (enhancementEleven)
        {
            GameObject fragHolder;

            for (int f = 0; f < 5; f++)
            {
                Vector3 rot;
                fragHolder = GameObject.Instantiate(bulletPrefab, bullet.gameObject.transform.position - new Vector3(0f, 0.3f, 0f), bullet.gameObject.transform.rotation);
                rot = fragHolder.transform.rotation.eulerAngles;
                rot.z += UnityEngine.Random.Range(-10f, 10f);
                fragHolder.transform.rotation = Quaternion.Euler(rot);
                fragHolder.GetComponent<Rigidbody2D>().velocity = fragHolder.transform.up * fragHolder.GetComponent<Bullet>().speed;
                fragHolder.GetComponent<Bullet>().isFragment = true;
                fragHolder.GetComponent<Bullet>().hitDamage = 25f;
            }

            Destroy(bullet.gameObject);
            enhancementEleven = false;
        }

        AudioSource.PlayClipAtPoint(CannonShot, Vector3.zero, 1f * dataFile.volume);
    }

    void AddStats()
    {
        foreach(Component comp in shell)
        {
            switch (comp.getId())
            {
                case componentID.EMPTY:
                    continue;
                case componentID.HIGH_EXPLOSIVE:
                    bullet.aoeDamage += 17f;
                    bullet.aoeSize += 0.15f;
                    break;
                case componentID.ARMOR_PIERCING:
                    bullet.pierces += 1;
                    bullet.hitDamage += 7f;
                    break;
                case componentID.FRAGMENTATION:
                    bullet.fragLevel += 1;
                    break;
                case componentID.INCENDIARY:
                    bullet.fireLevel += 1;
                    break;
                case componentID.TUNGSTEN:
                    bullet.hitDamage += 25f;
                    bullet.knockback += 10f;
                    break;
                case componentID.RAILGUN:
                    bullet.pierces += 3;
                    bullet.speed += 6f;
                    bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.up * bullet.speed;
                    break;
                case componentID.NUCLEAR:
                    bullet.aoeDamage += 50f;
                    bullet.aoeSize += 1f;
                    takeDamage(4f);
                    break;
                case componentID.ENHANCEMENT:
                    string enhance = "";
                    bool three = false;
                    for (int i = 0; i < 3; i++)
                    {
                        if (shell[i].getId() != componentID.EMPTY && shell[i].getId() != componentID.ENHANCEMENT)
                        {
                            int j;
                            for (j = i + 1; j < 4; j++)
                            {
                                if (shell[j].getId() != componentID.EMPTY && shell[j].getId() != componentID.ENHANCEMENT)
                                {
                                    if (shell[i].getId() != componentID.RAILGUN && shell[i].getId() == shell[j].getId()) 
                                    { 
                                        continue;
                                    }

                                    break;
                                }
                                if(j == 3) { j = 0; break; }
                            }

                            if(j == 0) { break; }

                            enhance = ((int)shell[i].getId()).ToString() + ((int)shell[j].getId()).ToString();
                            switch (enhance)
                            {
                                case "12" or "21": bullet.addEnhancement(0);    break;
                                case "13" or "31": bullet.addEnhancement(1);    break;
                                case "14" or "41": bullet.addEnhancement(2);    break;
                                case "15" or "51": three = true;               break;
                                case "16" or "61": bullet.addEnhancement(4);    break;
                                case "23" or "32": bullet.addEnhancement(5);    break;
                                case "24" or "42": bullet.addEnhancement(6);    break;
                                case "25" or "52": bullet.addEnhancement(7);    break;
                                case "26" or "62": bullet.addEnhancement(8);    break;
                                case "34" or "43": bullet.addEnhancement(9);    break;
                                case "35" or "53": bullet.addEnhancement(10);   break;
                                case "36" or "63": enhancementEleven = true;    break;
                                case "45" or "54": bullet.addEnhancement(12);   break;
                                case "46" or "64": bullet.addEnhancement(13);   break;
                                case "56" or "65": bullet.addEnhancement(14);   break;
                                case "66":         bullet.addEnhancement(15);   break;
                                default: break;
                            }
                        }
                        if(enhance != "")
                        { break; }
                    }
                    if (three) { bullet.aoeSize -= 0.1f; bullet.aoeDamage += 75f; }
                    break;
                default:
                    break;
            }
        }
    }

    void LoadShell(Component compIn)
    {
        if (shellIndex >= shellLimit)
        {
            return;
        }
        shell[shellIndex] = compIn;
        componentIcons[shellIndex].SetActive(true);

        int value = (int)compIn.getId() - 1;

        componentIcons[shellIndex].GetComponent<Image>().sprite = icons[value];

        shellIndex++;

        AudioSource.PlayClipAtPoint(ComponentAudios[value], Vector3.zero, audioVolumes[value] * dataFile.volume);
    }

    void ClearShells()
    {
        shellIndex = 0;
        for (int i = 0; i < 4; i++)
        {
            if (i < shellLimit)
            {
                shell[i] = Components.getComponent(componentID.EMPTY);
                componentIcons[i].SetActive(false);
            }
            else
            {
                shell[i] = Components.getComponent(componentID.EMPTY);
                componentIcons[i].SetActive(true);
            }
        }
    }

    void pointTurret()
    {
        mousePos = Input.mousePosition;
        mousePos.z = 0;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        Vector3 towardMouse = mousePos - turret.transform.position;
        float angle = Mathf.Atan2(towardMouse.y, towardMouse.x) * 57.3f - 90f;//* 57.3f
        turret.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public void takeDamage(float damage)
    {
        health -= damage;
        ui.setHealth(health);

        if (health <= 0)
        {
            gameover = true;
            Time.timeScale = 0f;
            pauseUI.SetActive(true);
            pauseUI.transform.GetChild(1).GetComponent<TMP_Text>().text = "Game Over";
        }
    }

    void unlockCheck(int waveIn)
    {
        if(waveIn % 10 == 0)
        {
            health = 40;
            ui.setHealth(health);
        }

        switch (waveIn)
        {
            case 1:
                if (!dataFile.slots[1]) { shellLimit++; }
                dataFile.slots[1] = true;
                break;
            case 2:
                dataFile.compUnlocks[1] = true;
                break;
            case 5:
                dataFile.compUnlocks[2] = true;
                break;
            case 7:
                dataFile.compUnlocks[3] = true;
                break;
            case 10:
                dataFile.unlocks[1] = true;
                if (!dataFile.slots[2]) { shellLimit++; }
                dataFile.slots[2] = true;
                break;
            case 12:
                dataFile.compUnlocks[4] = true;
                break;
            case 14:
                dataFile.compUnlocks[5] = true;
                break;
            case 17:
                dataFile.compUnlocks[6] = true;
                break;
            case 20:
                dataFile.unlocks[2] = true;
                if (!dataFile.slots[3]) { shellLimit++; }
                dataFile.slots[3] = true;
                dataFile.compUnlocks[7] = true;
                break;
            case 30:
                dataFile.unlocks[3] = true;
                break;
            default: break;
        }

        if (dataFile.waveRecord < waveIn)
        { dataFile.waveRecord = waveIn; }

        string data = JsonUtility.ToJson(dataFile);
        System.IO.File.WriteAllText(dataPath, data);
    }

    
    public void TogglePause()
    {
        if(gameover) { return; }

        if (!paused)
        {
            pauseUI.SetActive(true);
            Time.timeScale = 0f;
            paused = true;
        }
        else
        {
            pauseUI.SetActive(false);
            Time.timeScale = 1f;
            paused = false;
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

