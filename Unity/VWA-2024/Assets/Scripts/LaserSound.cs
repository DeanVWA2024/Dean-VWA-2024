using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSound : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (gameObject.GetComponent<LineRenderer>().enabled == true)
        {
            // Play the audio if it's not already playing
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            // Stop the audio if the character is not moving
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}
