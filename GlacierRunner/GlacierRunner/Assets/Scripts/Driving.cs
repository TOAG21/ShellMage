using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Driving : MonoBehaviour
{
    [SerializeField] ParticleSystem[] iceParticles;
    [SerializeField] Rigidbody2D[] cabooses;

    Rigidbody2D rb;
    AudioSource audioSource;

    float acceleration = 170f * 5.5f;
    float torque = 17f * 5.5f;

    float leftTrackStatus = 0.0f;
    float rightTrackStatus = 0.0f;
    float trackAccelerationTime = 1.75f;

    float reverseCoefficient = -0.7f;

    float speedLimit = 60f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
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
                rb.AddForce(this.transform.up * acceleration * leftTrackStatus * diminishingSpeed * Time.deltaTime);
                rb.AddTorque(-1f * torque * leftTrackStatus * Time.deltaTime);
            }
        }
        else if (Input.GetKey(KeyCode.Q))//left reverse
        {
            leftTrackStatus -= trackAccelerationTime * Time.deltaTime;
            if (leftTrackStatus < -1.0f) { leftTrackStatus = -1.0f; }

            //if (leftTrackStatus <= 0.0f)
            {
                rb.AddForce(-1f * reverseCoefficient * this.transform.up * acceleration * leftTrackStatus * diminishingSpeed * Time.deltaTime);
                rb.AddTorque(reverseCoefficient * torque * leftTrackStatus * Time.deltaTime);
            }
        }
        else
        {
            if (leftTrackStatus > 0.1f) { leftTrackStatus -= trackAccelerationTime * Time.deltaTime; }
            if (leftTrackStatus < 0.1f) { leftTrackStatus += trackAccelerationTime * Time.deltaTime; }

        }

        ///////////////////////////////////////////////////////////////////
        if (Input.GetMouseButton(1))//right forward
        {
            rightTrackStatus += trackAccelerationTime * Time.deltaTime;
            if (rightTrackStatus > 1.0f) { rightTrackStatus = 1.0f; }

           // if (rightTrackStatus >= 0.0f)
            {
                rb.AddForce(this.transform.up * acceleration * rightTrackStatus * diminishingSpeed * Time.deltaTime);
                rb.AddTorque(torque * rightTrackStatus * Time.deltaTime);
            }
        }
        else if (Input.GetKey(KeyCode.E))//right reverse
        {
            rightTrackStatus -= trackAccelerationTime * Time.deltaTime;
            if (rightTrackStatus < -1.0f) { rightTrackStatus = -1.0f; }

            //if (rightTrackStatus <= 0.0f)
            {
                rb.AddForce(-1f * reverseCoefficient * this.transform.up * acceleration * rightTrackStatus * diminishingSpeed * Time.deltaTime);
                rb.AddTorque(-1f * reverseCoefficient * torque * rightTrackStatus * Time.deltaTime);
            }
        }
        else
        {
            if (rightTrackStatus > 0.1f) { rightTrackStatus -= trackAccelerationTime * Time.deltaTime; }
            if (rightTrackStatus < 0.1f) { rightTrackStatus += trackAccelerationTime * Time.deltaTime; }

        }

        updateParticles();
        audioSource.volume = Mathf.Abs(leftTrackStatus) + Mathf.Abs(rightTrackStatus) / 3f;

        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene("Level1");
        }
        //Debug.Log(this.transform.up);
    }

    void updateParticles()
    {
        ParticleSystem.EmissionModule emitter;
        emitter = iceParticles[0].emission;
        emitter.rateOverTime = 163f * Mathf.Abs(leftTrackStatus);
        emitter = iceParticles[1].emission;
        emitter.rateOverTime = 163f * Mathf.Abs(rightTrackStatus);

        for (int i = 2; i < iceParticles.Length; i+=2)
        {
            emitter = iceParticles[i].emission;
            emitter.rateOverTime = 83f * (cabooses[i / 2 - 1].velocity.magnitude / 20);
            emitter = iceParticles[i+1].emission;
            emitter.rateOverTime = 83f * (cabooses[i / 2 - 1].velocity.magnitude / 20);
        }
    }
}
