using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float startingHealth = 200f;

    public float health;
    [SerializeField] float damage = 1f;
    public bool bomber;
    float fireDamage = 0.0f;

    Vector3 pos;
    Color hpColoration = new Color(1f, 1f, 1f, 1f);

    // Start is called before the first frame update
    void Awake()
    {
        health = startingHealth;

        pos = transform.position;
    }

    private void FixedUpdate()
    {
        pos.y -= 0.01f;
        transform.position = pos;

        damaged(fireDamage);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "wall")
            { return; }
        try
        {
            GameObject.FindGameObjectWithTag("scripts").GetComponent<GameManager>().takeDamage(damage);
        }
        catch { }
        health = 0;
    }

    public void damaged(float damage)
    {
        //Debug.Log("Damage called: " + damage);
        health -= damage;

        hpColoration.g = (health / startingHealth);
        hpColoration.b = (health / startingHealth);
        hpColoration.a = 1f;
        this.transform.GetChild(0).GetComponent<SpriteRenderer>().color = hpColoration;
    }

    public void ignite(int fireLevel)
    {
        fireDamage = 0.4f * fireLevel;
    }

    public bool isBurning()
    {
        if (fireDamage > 0)
        {
            return true;
        }
        return false;
    }

    public void Detonate()
    {

    }
}
