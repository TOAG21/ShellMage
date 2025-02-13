using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject explosionEffect;

    public float speed = 10f;
    public int pierces = 0;
    public float hitDamage = 15;
    public float aoeDamage = 35;
    public float aoeSize = 0.25f;
    public int fireLevel = 0;
    public int fragLevel = 0;
    public float knockback = 0f;

    public bool fragment = false;

    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<Rigidbody2D>().velocity = transform.up * speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.collider.gameObject.tag);
        if (collision.gameObject.tag != "enemy")
        { return; }
        if (collision.gameObject.tag == "wall")
        { Destroy(gameObject); }

        if (fragment)
        {
            //what should the bullet do if its a fragment
            collision.gameObject.GetComponent<Enemy>().damaged(hitDamage);
            Destroy(gameObject);
            return;
        }

        //add knockback usage
        if(fireLevel > 0)//ignite hit enemies
        {
            collision.gameObject.GetComponent<Enemy>().ignite(fireLevel);
        }
        if (pierces > 0) //pierce through objects first
        { 
            pierces--;  
            collision.gameObject.GetComponent<Enemy>().damaged(hitDamage);
            return; 
        }
        else //detonate into aoe
        {
            Debug.Log("Detonate");
            GameObject.Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(this.transform.position.x, this.transform.position.y ), aoeSize);
            foreach (var hitCollider in hitColliders)
            {
                Debug.Log(hitCollider.gameObject.tag);
                if (hitCollider.gameObject.tag == "enemy")
                {
                    hitCollider.gameObject.GetComponent<Enemy>().damaged(aoeDamage);
                }
            }

            for (int i = 0; i < fragLevel; i++)
            {
                //spawn 3-5 fragment prefabs
            }
        }



        Destroy(gameObject);
    }
}
