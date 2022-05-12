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
    void Update()
    {
        /*
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
            
        }*/
    }

    IEnumerator ToNormal()
    {
        audioSource.Stop();
        volumeLowering = true;
        volumeRising = false;
        yield return new WaitForSeconds(1);
        time = 0;
        volumeRising = true;
        volumeLowering = false;
        audioSource.clip = seaguls;
        audioSource.volume = 0;
        audioSource.Play();
    }

    IEnumerator ToWhales()
    {
        audioSource.Stop();
        volumeLowering = true;
        volumeRising = false;
        yield return new WaitForSeconds(1);
        time = 0;
        volumeRising = true;
        volumeLowering = false;
        audioSource.clip = whales;
        audioSource.volume = 0;
        audioSource.Play();        
    }

    public void PlaySeaguls()
    {
        time = 0;
        nightAudio.Stop();
        audioSource.Play();
        //audioSource.clip = seaguls;
        //audioSource.volume = 0;
        //audioSource.Play();
    }

    public void PlayWhales()
    {
        time = 0;
        audioSource.Stop();
        nightAudio.Play();
        //audioSource.clip = whales;
        //audioSource.volume = 0;
        //audioSource.Play();
    }
}
