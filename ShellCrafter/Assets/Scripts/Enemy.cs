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
    public GameObject explosionEffect;
    float bomberDamage = 60f;

    float fireDamage = 0.0f;
    [SerializeField] GameObject fireEffect;
    GameObject fireHolder;

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

        if(health <= 0 && isBurning())
        {
            Destroy(fireHolder);
        }
    }

    public void ignite(int fireLevel)
    {
        fireDamage = 0.4f * fireLevel;

        fireHolder = Instantiate(fireEffect, this.transform);
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
        GameObject.Instantiate(explosionEffect, gameObject.transform.position, Quaternion.identity);
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(this.transform.position.x, this.transform.position.y), 1.5f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.tag == "enemy")
            {
                hitCollider.gameObject.GetComponent<Enemy>().damaged(bomberDamage);
            }
        }
    }

    public void knockback(Vector3 newPos)
    {
        pos = newPos;
    }

    public void multSHP(float multiplier)
    {
        startingHealth *= multiplier;
    }
}
