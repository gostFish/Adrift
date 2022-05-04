using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    
    private AudioSource audioSource;
    public AudioClip stomachRumbling;

    public float minHungerTime; //Minimum time interval
    public float maxHungerTime; //Maximum time interval

    private float hungerNoiseTimeNext; //Randomly decided time for next sound

    private float time;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        hungerNoiseTimeNext = UnityEngine.Random.Range(minHungerTime, maxHungerTime);
        time = 0f;
    }

    void FixedUpdate()
    {
        time += Time.deltaTime;

        //Play noise at random intervals once below 70 health
        if (PlayerPrefs.GetFloat("Hunger") < 70)
        {
            if (time > hungerNoiseTimeNext)
            {
                hungerNoiseTimeNext = UnityEngine.Random.Range(minHungerTime, maxHungerTime);
                audioSource.PlayOneShot(stomachRumbling);
                time = 0f;
            }
        }
    }
}
