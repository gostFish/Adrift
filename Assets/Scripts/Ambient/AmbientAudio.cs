using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientAudio : MonoBehaviour
{
    public GameObject hallManager;

    private bool hallPlaying;
    private bool normalPlaying;

    private float time;
    private float volumeLevel;

    private bool volumeLowering;
    private bool volumeRising;

    public AudioSource audioSource;
    public AudioSource nightAudio;
    public AudioClip whales;
    public AudioClip seaguls;

    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
        //nightAudio = GetComponentInChildren<AudioSource>();
        volumeRising = false;
        volumeLowering = false;
        normalPlaying = true;

        PlaySeaguls();
    }

    private void FixedUpdate()
    {
        time += Time.deltaTime;
    }
    
    public void PlaySeaguls()
    {
        time = 0;
        nightAudio.Stop();
        audioSource.Play();

        audioSource.volume = PlayerPrefs.GetFloat("volume");
            
        //audioSource.clip = seaguls;
        //audioSource.volume = 0;
        //audioSource.Play();
    }

    public void PlayWhales()
    {
        time = 0;
        audioSource.Stop();
        nightAudio.Play();

        audioSource.volume = PlayerPrefs.GetFloat("volume");
        //audioSource.clip = whales;
        //audioSource.volume = 0;
        //audioSource.Play();
    }
}
