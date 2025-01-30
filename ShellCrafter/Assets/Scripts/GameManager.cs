using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    public GameObject turret;
    [SerializeField] GameObject bullet;
    [SerializeField] AudioClip CannonShot;
    [SerializeField] AudioClip Component;
    [SerializeField] AudioClip Explosion;

    Component[] shell = new Component[4];
    int shellIndex = 0;
    int wave = 1;
    float fireCooldown = 0.0f;

    Vector3 mousePos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pointTurret();
        if(fireCooldown >= 0.0f) { fireCooldown -= Time.deltaTime; }

        InputCheck();        
    }

    void InputCheck()
    {       
        //load shells
        if (Input.GetKeyDown(KeyCode.Q))
        {
            LoadShell(Components.getComponent(componentID.HIGH_EXPLOSIVE));
        }

        //shoot turret
        if (Input.GetMouseButtonDown(0) && fireCooldown < 0.0f)
        {
            Shoot();
            fireCooldown = 1.0f;
            ClearShells();
        }
    }

    void Shoot()
    {
        GameObject.Instantiate(bullet, turret.transform.GetChild(0).position, turret.transform.rotation);
        AudioSource.PlayClipAtPoint(CannonShot, Vector3.zero, 100f);
    }

    void LoadShell(Component compIn)
    {
        if (shellIndex >= 4)
        {
            return;
        }
        shell[shellIndex] = compIn;
        shellIndex++;
        
        AudioSource.PlayClipAtPoint(Component, Vector3.zero, 25f);
    }

    void ClearShells()
    {
        shellIndex = 0;
        shell[0] = Components.getComponent(componentID.EMPTY);
        shell[1] = Components.getComponent(componentID.EMPTY);
        shell[2] = Components.getComponent(componentID.EMPTY);
        shell[3] = Components.getComponent(componentID.EMPTY);
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
}

