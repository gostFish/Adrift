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

    private AudioSource audioSource;
    public AudioClip whales;
    public AudioClip seaguls;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        volumeRising = false;
        volumeLowering = false;
        normalPlaying = true;
    }

    private void FixedUpdate()
    {
        time += Time.deltaTime;
    }
    void Update()
    {
        if(!hallPlaying && hallManager.GetComponent<HallucinationManager>().haveActive)
        {
            //audioSource.clip = whales;
            //audioSource.Play();
            time = 0;
            volumeLevel = PlayerPrefs.GetFloat("volume");
            StartCoroutine(ToWhales());
            audioSource.loop = true;            
            hallPlaying = true;
            normalPlaying = false;
        }
        else if (!hallPlaying && !normalPlaying)
        {
            //audioSource.clip = seaguls;
            //audioSource.Play();
            time = 0;
            volumeLevel = PlayerPrefs.GetFloat("volume");
            StartCoroutine(ToNormal());
            audioSource.loop = true;
            normalPlaying = true;
        }
        else if(!hallManager.GetComponent<HallucinationManager>().haveActive)
        {
            hallPlaying = false;
        }

        if (volumeLowering) //Dramatic rise in volume
        {
            audioSource.volume -= ((time * volumeLevel)/100);
        }
        else if (volumeRising)
        {
            if(audioSource.volume < volumeLevel)
            {
                audioSource.volume += ((time * volumeLevel) / 100);
            }
            
        }

    }

    IEnumerator ToNormal()
    {        
        volumeLowering = true;
        volumeRising = false;
        yield return new WaitForSeconds(10);
        time = 0;
        volumeRising = true;
        volumeLowering = false;
        audioSource.clip = seaguls;
        audioSource.volume = 0;
        audioSource.Play();
    }

    IEnumerator ToWhales()
    {
        volumeLowering = true;
        volumeRising = false;
        yield return new WaitForSeconds(10);
        time = 0;
        volumeRising = true;
        volumeLowering = false;
        audioSource.clip = whales;
        audioSource.volume = 0;
        audioSource.Play();        
    }
}
