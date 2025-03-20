using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject explosionEffect;
    [SerializeField] GameObject fragment;
    [SerializeField] AudioClip explosion;
    [SerializeField] AudioClip pierce;

    public float speed = 10f;
    public int pierces = 0;
    public float hitDamage = 15;
    public float aoeDamage = 35;
    public float aoeSize = 0.2f;
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

        if (enhanced)
        {
            if (enhancement[8])
            {
                pierces = 0;
                isFragment = true;
                fragLevel++;
            }
            if (enhancement[13])
            {
                pierces = 0;
                fireLevel = 6;
            }
            if (enhancement[14])
            {
                pierces = 0;
                knockback = 66f;
            }
            if (enhancement[15])
            {
                pierces += 4;
            }
        }

        if (isFragment)
        {
            //what should the bullet do if its a fragment
            collision.gameObject.GetComponent<Enemy>().damaged(hitDamage);

            if (enhanced)
            {
                if (enhancement[1])
                {
                    Explosion(aoeSize, aoeDamage);
                }
                if (pierces > 0)//enhance 5
                {
                    pierces--;
                    return;
                }
                if (enhancement[9])
                {
                    collision.gameObject.GetComponent<Enemy>().ignite(2);
                }
                if (enhancement[10])
                {
                    //addfragment knocback
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
            AudioSource.PlayClipAtPoint(pierce, transform.position, 1.0f);

            pierces--;  
            collision.gameObject.GetComponent<Enemy>().damaged(hitDamage);

            if(enhanced)
            {
                if (enhancement[0])
                {
                    if (collision.gameObject.GetComponent<Enemy>().health <= 0)
                    {
                        GameObject.Instantiate(explosionEffect, collision.gameObject.transform.position, Quaternion.identity);
                        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(this.transform.position.x, this.transform.position.y), 1.5f);
                        foreach (var hitCollider in hitColliders)
                        {
                            if (hitCollider.gameObject.tag == "enemy")
                            {
                                hitCollider.gameObject.GetComponent<Enemy>().damaged(aoeDamage);
                            }
                        }
                    }
                }
                if (enhancement[4])
                {
                    Explosion(aoeSize, aoeDamage);
                }
                if (enhancement[6])
                {
                    if (collision.gameObject.GetComponent<Enemy>().isBurning())
                    {
                        Explosion(aoeSize, aoeDamage);
                    }
                }
                if (enhancement[7])
                {
                    hitDamage += 15f;
                }
                if (enhancement[12])
                {
                    if (collision.gameObject.GetComponent<Enemy>().isBurning())
                    {
                        collision.gameObject.GetComponent<Enemy>().damaged(75f);
                    }
                }
                if (enhancement[15])
                {
                    Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(this.transform.position.x, this.transform.position.y), 20f);
                    foreach (var hitCollider in hitColliders)
                    {
                        if (hitCollider.gameObject.tag == "enemy" && hitCollider.gameObject.GetInstanceID() != collision.gameObject.GetInstanceID())
                        {
                            float y = hitCollider.gameObject.transform.position.y - this.transform.position.y;
                            float x = hitCollider.gameObject.transform.position.x - this.transform.position.x;
                            Vector3 rot = transform.rotation.eulerAngles;
                            rot.z = Mathf.Atan2(y, x) * 57.3f - 90f;
                            transform.rotation = Quaternion.Euler(rot);

                            GetComponent<Rigidbody2D>().velocity = transform.up * speed;
                            break;
                        }
                    }
                }
            }

            return; 
        }
        else //detonate into aoe
        {
            AudioSource.PlayClipAtPoint(explosion, transform.position, 1.0f);

            collision.gameObject.GetComponent<Enemy>().damaged(hitDamage);
            if (enhancement[0])
            {
                if (collision.gameObject.GetComponent<Enemy>().health <= 0)
                {
                    GameObject.Instantiate(explosionEffect, collision.gameObject.transform.position, Quaternion.identity);
                    Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(this.transform.position.x, this.transform.position.y), 1.5f);
                    foreach (var hitCollider in hitColliders)
                    {
                        if (hitCollider.gameObject.tag == "enemy")
                        {
                            hitCollider.gameObject.GetComponent<Enemy>().damaged(aoeDamage);
                        }
                    }
                }
            }
            if (enhancement[8])
            {
                for (int i = 0; i < fragLevel; i++)
                {
                    for (int f = 0; f < 3; f++)
                    {
                        Vector3 rot;
                        fragHolder = GameObject.Instantiate(fragment, transform.position - new Vector3(0f, 0.3f, 0f), Quaternion.identity);
                        rot = fragHolder.transform.rotation.eulerAngles;
                        rot.z = UnityEngine.Random.Range(-30.0f, 30.0f);
                        fragHolder.transform.rotation = Quaternion.Euler(rot);
                        fragHolder.GetComponent<Rigidbody2D>().velocity = fragHolder.transform.up * fragHolder.GetComponent<Bullet>().speed;
                        fragHolder.GetComponent<Bullet>().pierces++;
                    }
                }

                Destroy(gameObject);
                return;
            }

            Explosion(aoeSize, aoeDamage);
            
            if (enhancement[2])
            {
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(this.transform.position.x, this.transform.position.y), aoeSize);
                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.gameObject.tag == "enemy")
                    {
                        hitCollider.gameObject.GetComponent<Enemy>().ignite(fireLevel + 1);
                    }
                }
            }


            for (int i = 0; i < fragLevel; i++)
            {
                for (int f = 0; f < 3; f++)
                {
                    Vector3 rot;
                    fragHolder = GameObject.Instantiate(fragment, transform.position - new Vector3(0f, 0.3f, 0f), Quaternion.identity);
                    rot = fragHolder.transform.rotation.eulerAngles;
                    rot.z = UnityEngine.Random.Range(0.0f, 360.0f);
                    fragHolder.transform.rotation = Quaternion.Euler(rot);
                    fragHolder.GetComponent<Rigidbody2D>().velocity = fragHolder.transform.up * fragHolder.GetComponent<Bullet>().speed;
                    if (enhanced)
                    {
                        if (enhancement[1]) { fragHolder.GetComponent<Bullet>().addEnhancement(1); }
                        if (enhancement[5]) { fragHolder.GetComponent<Bullet>().pierces++; }
                        if (enhancement[9]) { fragHolder.GetComponent<Bullet>().addEnhancement(9); }
                        if (enhancement[10]) { fragHolder.GetComponent<Bullet>().addEnhancement(10); }
                    }
                }
            }
        }



        Destroy(gameObject);
    }

    public void Airburst()
    {

    }

    void Explosion(float size, float damage)
    {
        GameObject.Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(this.transform.position.x, this.transform.position.y), size);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.tag == "enemy")
            {
                hitCollider.gameObject.GetComponent<Enemy>().damaged(damage);

                if (enhancement[4] && hitCollider.gameObject.GetComponent<Enemy>().health <= 0)
                {
                    Collider2D[] hitCollidersTwo = Physics2D.OverlapCircleAll(new Vector2(hitCollider.gameObject.transform.position.x, hitCollider.gameObject.transform.position.y), size);
                    foreach (var hitColliderTwo in hitCollidersTwo)
                    {
                        if (hitCollider.gameObject.tag == "enemy")
                        {
                            hitCollider.gameObject.GetComponent<Enemy>().damaged(damage);
                        }
                    }
                }
            }
        }
    }

    public void addEnhancement(int enhancementIn)
    {
        enhanced = true;
        enhancement[enhancementIn] = true;
    }
}
