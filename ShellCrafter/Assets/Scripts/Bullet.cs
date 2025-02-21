using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject explosionEffect;
    [SerializeField] GameObject fragment;

    public float speed = 10f;
    public int pierces = 0;
    public float hitDamage = 15;
    public float aoeDamage = 35;
    public float aoeSize = 0.25f;
    public int fireLevel = 0;
    public int fragLevel = 0;
    public float knockback = 0f;

    public bool isFragment = false;
    public bool enhanced = false;
    bool[] enhancement = new bool[16];

    private GameObject fragHolder;

    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<Rigidbody2D>().velocity = transform.up * speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.collider.gameObject.tag);
        if (collision.gameObject.tag == "wall")
        { Destroy(gameObject); }
        if (collision.gameObject.tag != "enemy")
        { return; }

        if (isFragment)
        {
            //what should the bullet do if its a fragment
            collision.gameObject.GetComponent<Enemy>().damaged(hitDamage);

            if (enhanced)
            {
                if (enhancement[1])
                {

                }
                if (enhancement[4])
                {

                }
                if (enhancement[5])
                {

                }
                if (enhancement[9])
                {

                }
                if (enhancement[10])
                {

                }
            }

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

            if(enhanced)
            {
                if (enhancement[0])
                {

                }
                if (enhancement[6])
                {

                }
                if (enhancement[7])
                {

                }
                if (enhancement[12])
                {

                }
                if (enhancement[15])
                {

                }
            }

            return; 
        }
        else //detonate into aoe
        {
            collision.gameObject.GetComponent<Enemy>().damaged(hitDamage);

            GameObject.Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(this.transform.position.x, this.transform.position.y ), aoeSize);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.tag == "enemy")
                {
                    hitCollider.gameObject.GetComponent<Enemy>().damaged(aoeDamage);
                    if (enhancement[4])
                    {

                    }
                }
            }

            if (enhanced)
            {
                if (enhancement[2])
                {

                }
                if (enhancement[8])
                {

                }
                if (enhancement[13])
                {

                }
                if (enhancement[14])
                {

                }
            }

            for (int i = 0; i < fragLevel; i++)
            {
                for (int f = 0; f < 3; f++)
                {
                    Vector3 rot;
                    fragHolder = GameObject.Instantiate(fragment, transform.position - new Vector3(0f, 0.3f, 0f), Quaternion.identity);
                    rot = fragHolder.transform.rotation.eulerAngles;
                    rot.z = Random.Range(0.0f, 360.0f);
                    fragHolder.transform.rotation = Quaternion.Euler(rot);
                    fragHolder.GetComponent<Rigidbody2D>().velocity = fragHolder.transform.up * fragHolder.GetComponent<Bullet>().speed;
                }
            }
        }



        Destroy(gameObject);
    }

    public void Airburst()
    {

    }

    public void addEnhancement(int enhancementIn)
    {
        enhancement[enhancementIn] = true;
    }
}
