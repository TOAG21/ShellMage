using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driving : MonoBehaviour
{
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            rb.AddForce(new Vector2(1f, 0f));
        }
    }
}
