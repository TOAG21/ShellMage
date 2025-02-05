using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driving : MonoBehaviour
{
    Rigidbody2D rb;

    float acceleration = 1f;
    float torque = 0.1f;

    float leftTrackStatus = 0.0f;
    float rightTrackStatus = 0.0f;
    float trackAccelerationTime = 1.75f;

    float reverseCoefficient = -0.7f;

    float speedLimit = 50f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float diminishingSpeed = 1 - ( rb.velocity.magnitude / speedLimit);

        if(Input.GetMouseButton(0))//left forward
        {
            leftTrackStatus += trackAccelerationTime * Time.deltaTime;
            if (leftTrackStatus > 1.0f) { leftTrackStatus = 1.0f; }

            //if (leftTrackStatus >= 0.0f)
            {
                rb.AddForce(this.transform.up * acceleration * leftTrackStatus * diminishingSpeed);
                rb.AddTorque(-1f * torque * leftTrackStatus);
            }
        }
        else if (Input.GetKey(KeyCode.Q))//left reverse
        {
            leftTrackStatus -= trackAccelerationTime * Time.deltaTime;
            if (leftTrackStatus < -1.0f) { leftTrackStatus = -1.0f; }

            //if (leftTrackStatus <= 0.0f)
            {
                rb.AddForce(-1f * reverseCoefficient * this.transform.up * acceleration * leftTrackStatus * diminishingSpeed);
                rb.AddTorque(reverseCoefficient * torque * leftTrackStatus);
            }
        }
        else
        {
            if (leftTrackStatus > 0.1f) { leftTrackStatus -= trackAccelerationTime * Time.deltaTime; }
            if (leftTrackStatus < 0.1f) { leftTrackStatus += trackAccelerationTime * Time.deltaTime; }

        }

        if (Input.GetMouseButton(1))//right forward
        {
            rightTrackStatus += trackAccelerationTime * Time.deltaTime;
            if (rightTrackStatus > 1.0f) { rightTrackStatus = 1.0f; }

           // if (rightTrackStatus >= 0.0f)
            {
                rb.AddForce(this.transform.up * acceleration * rightTrackStatus * diminishingSpeed);
                rb.AddTorque(torque * rightTrackStatus);
            }
        }
        else if (Input.GetKey(KeyCode.E))//right reverse
        {
            rightTrackStatus -= trackAccelerationTime * Time.deltaTime;
            if (rightTrackStatus < -1.0f) { rightTrackStatus = -1.0f; }

            //if (rightTrackStatus <= 0.0f)
            {
                rb.AddForce(-1f * reverseCoefficient * this.transform.up * acceleration * rightTrackStatus * diminishingSpeed);
                rb.AddTorque(-1f * reverseCoefficient * torque * rightTrackStatus);
            }
        }
        else
        {
            if (rightTrackStatus > 0.1f) { rightTrackStatus -= trackAccelerationTime * Time.deltaTime; }
            if (rightTrackStatus < 0.1f) { rightTrackStatus += trackAccelerationTime * Time.deltaTime; }

        }

        //Debug.Log(this.transform.up);
    }
}
