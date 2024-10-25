using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicZone : MonoBehaviour
{
    private AudioSource audioSource;

    public float fadeTime;
    public float maxVolume;

    private float targetVolume;

    private void Start()
    {
        targetVolume = 0f;
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = targetVolume;
        audioSource.Play();
    }

    private void Update()
    {
        if(!Mathf.Approximately(audioSource.volume, targetVolume))
        {

        }
    }
}
