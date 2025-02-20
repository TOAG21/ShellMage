using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject turret;
    [SerializeField] UImanager ui;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject[] componentIcons;
    [SerializeField] AudioClip CannonShot;
    [SerializeField] AudioClip Component;
    [SerializeField] AudioClip Explosion;
    [SerializeField] GameObject Enemy1;


    private Bullet bullet;


    int wave = 0;
    float health = 40f;
    EnemyManager em;

    Component[] shell = new Component[4];
    int shellIndex = 0;
    float fireCooldown = 0.0f;

    Vector3 mousePos;

    // Start is called before the first frame update
    void Start()
    {
        ClearShells();
        em = GetComponent<EnemyManager>();
    }

    // Update is called once per frame
    void Update()
    {
        pointTurret();
        if(fireCooldown >= 0.0f) { fireCooldown -= Time.deltaTime; }

        InputCheck();    
        
        if(em.noEnemies)
        {
            em.SpawnWave(wave);
            wave++;
            ui.setWave(wave);
        }
    }

    void InputCheck()
    {       
        //load shells
        if (Input.GetKeyDown(KeyCode.Q))
        {
            LoadShell(Components.getComponent(componentID.HIGH_EXPLOSIVE));
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            LoadShell(Components.getComponent(componentID.ARMOR_PIERCING));
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            LoadShell(Components.getComponent(componentID.FRAGMENTATION));
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            LoadShell(Components.getComponent(componentID.INCENDIARY));
        }
        ////////////////////////////////////////////
        if (Input.GetKeyDown(KeyCode.A))
        {
            LoadShell(Components.getComponent(componentID.TUNGSTEN));
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            LoadShell(Components.getComponent(componentID.RAILGUN));
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            LoadShell(Components.getComponent(componentID.NUCLEAR));
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            LoadShell(Components.getComponent(componentID.ENHANCEMENT));
        }

        //for dev testing
        if (Input.GetKeyDown(KeyCode.Z))
        {
            GameObject.Instantiate(Enemy1, new Vector3(Random.Range(-7, 7f), 4f, 0f), Quaternion.Euler(Vector3.zero));
        }

        //shoot turret
        if (Input.GetMouseButtonDown(0) && fireCooldown < 0.0f)
        {
            Shoot();
            fireCooldown = 1.0f;
            ClearShells();
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
        AudioSource.PlayClipAtPoint(CannonShot, Vector3.zero, 100f);
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
                    bullet.aoeDamage += 10f;
                    bullet.aoeSize += 0.05f;
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
                    bullet.hitDamage += 20f;
                    bullet.knockback += 10f;
                    break;
                case componentID.RAILGUN:
                    bullet.pierces += 5;
                    bullet.speed += 6f;
                    bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.up * bullet.speed;
                    break;
                case componentID.NUCLEAR:
                    bullet.aoeDamage += 50f;
                    bullet.aoeSize += 1f;
                    takeDamage(4f);
                    break;
                case componentID.ENHANCEMENT:
                    bullet.enhanced = true;
                    string enhance = "";
                    for (int i = 0; i < 3; i++)
                    {
                        if (shell[i].getId() != componentID.EMPTY && shell[i].getId() != componentID.ENHANCEMENT)
                        {
                            enhance = ((int)shell[i].getId()).ToString() + ((int)shell[i + 1].getId()).ToString();
                            switch (enhance)
                            {
                                case "12": bullet.addEnhancement(0);  break;
                                case "13": bullet.addEnhancement(1);  break;
                                case "14": bullet.addEnhancement(2);  break;
                                case "15": bullet.addEnhancement(3);  break;
                                case "16": bullet.addEnhancement(4);  break;
                                case "23": bullet.addEnhancement(5);  break;
                                case "24": bullet.addEnhancement(6);  break;
                                case "25": bullet.addEnhancement(7);  break;
                                case "26": bullet.addEnhancement(8);  break;
                                case "34": bullet.addEnhancement(9);  break;
                                case "35": bullet.addEnhancement(10); break;
                                case "36": bullet.addEnhancement(11); break;
                                case "45": bullet.addEnhancement(12); break;
                                case "46": bullet.addEnhancement(13); break;
                                case "56": bullet.addEnhancement(14); break;
                                case "66": bullet.addEnhancement(15); break;
                                default: break;
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }

    void LoadShell(Component compIn)
    {
        if (shellIndex >= 4)
        {
            return;
        }
        shell[shellIndex] = compIn;
        componentIcons[shellIndex].SetActive(true);
        componentIcons[shellIndex].GetComponent<Image>().color = compIn.GetColor();

        shellIndex++;
        
        AudioSource.PlayClipAtPoint(Component, Vector3.zero, 0.8f);
    }

    void ClearShells()
    {
        shellIndex = 0;
        shell[0] = Components.getComponent(componentID.EMPTY);
        componentIcons[0].SetActive(false);
        shell[1] = Components.getComponent(componentID.EMPTY);
        componentIcons[1].SetActive(false);
        shell[2] = Components.getComponent(componentID.EMPTY);
        componentIcons[2].SetActive(false);
        shell[3] = Components.getComponent(componentID.EMPTY);
        componentIcons[3].SetActive(false);
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
    }
}

