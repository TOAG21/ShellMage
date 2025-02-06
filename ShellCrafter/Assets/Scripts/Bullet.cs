using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public int pierces = 0;
    public float hitDamage = 15;
    public float aoeDamage = 35;
    public float aoeSize = 0.25f;

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

        if (pierces > 0) //pierce through objects first
        { 
            pierces--;  
            collision.gameObject.GetComponent<Enemy>().damaged(hitDamage);
            return; 
        }
        else //detonate into aoe
        {
            Debug.Log("Detonate");
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(this.transform.position.x, this.transform.position.y ), aoeSize);
            foreach (var hitCollider in hitColliders)
            {
                Debug.Log(hitCollider.gameObject.tag);
                if (hitCollider.gameObject.tag == "enemy")
                {
                    hitCollider.gameObject.GetComponent<Enemy>().damaged(aoeDamage);
                }
            }
        }



        Destroy(gameObject);
    }
}
