using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float startingHealth = 200f;

    float health;
    float fireDamage = 0.0f;

    Vector3 pos;
    Color hpColoration = new Color(1f, 1f, 1f, 1f);

    // Start is called before the first frame update
    void Start()
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

    public void damaged(float damage)
    {
        //Debug.Log("Damage called: " + damage);
        health -= damage;
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }

        hpColoration.g = (health / startingHealth);
        hpColoration.b = (health / startingHealth);
        hpColoration.a = 1f;
        this.transform.GetChild(0).GetComponent<SpriteRenderer>().color = hpColoration;
    }

    public void ignite(int fireLevel)
    {
        fireDamage = 0.4f * fireLevel;
    }
}
