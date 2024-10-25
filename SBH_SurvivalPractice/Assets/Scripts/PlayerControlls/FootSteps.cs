using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip[] footStepClips;

    private Rigidbody rb;

    public float footstepThreshhold;
    public float footstepRate;
    private float footstepTime;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(Mathf.Abs(rb.velocity.y) <= 0.1f)
        {
            if(rb.velocity.magnitude > footstepThreshhold)
            {
                if(Time.time - footstepTime > footstepRate)
                {
                    footstepTime = Time.time;
                    audioSource.PlayOneShot(footStepClips[Random.Range(0,footStepClips.Length)]);
                }
            }
        }
    }
}
