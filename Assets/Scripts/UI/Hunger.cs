using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class Hunger : MonoBehaviour
{

    //Game Variables
    private float hunger;
    private float time;

    public float hungerRate; //Higher is quicker
    public float updateRate;


    //Post Processing

    public PostProcessProfile effects;
    private Vignette vig;
    private ChromaticAberration chr;
    private Bloom bloom;

    private GameObject player;

    //UI Objects

    public RawImage hungerUI;

    public Texture hunger1;
    public Texture hunger2;
    public Texture hunger3;
    public Texture hunger4;
    public Texture hunger5;

    public GameObject deathScreen;


    void Start()
    {
        //Load saved data if any
        if (PlayerPrefs.HasKey("Hunger"))
        {
            hunger = PlayerPrefs.GetFloat("Hunger");
        }
        else
        {
            hunger = 100f;
            PlayerPrefs.SetFloat("Hunger", hunger);
        }

        //Hunger effects
        vig = effects.GetSetting<Vignette>();
        chr = effects.GetSetting<ChromaticAberration>();
        bloom = effects.GetSetting<Bloom>();

        player = GameObject.FindGameObjectWithTag("Player");
    }

    
    void FixedUpdate()
    {
        time += Time.deltaTime;         

        if (time > updateRate)
        {
            //Decrease hunger and save state
            hunger = PlayerPrefs.GetFloat("Hunger"); //Update the hunger
            hunger -= hungerRate;
            PlayerPrefs.SetFloat("Hunger", hunger); //Update the hunger

            //Effects
            if (hunger <= 100)
            {
                vig.intensity.value = 1 - (hunger / 60);
            }

            if (hunger <= 80)
            {
                chr.intensity.value = 1 - (0.0002f * Mathf.Pow(hunger, 2f));
            }
            else if (hunger > 80)
            {
                chr.intensity.value = 0;
            }

            if (hunger <= 100)
            {
                bloom.intensity.value = 0.02f * Mathf.Tan(1.572f - (0.002f * hunger));
            }
            time = 0;

            //Update Hunger UI
            switch (hunger)
            {
                case float hunger when hunger <= 1f:
                    DeathScreen();
                    break;
               /* case float hunger when hunger <= 25:
                    hungerUI.texture = hunger5;
                    break;
                case float hunger when hunger <= 50:
                    hungerUI.texture = hunger4;
                    break;
                case float hunger when hunger <= 75:
                    hungerUI.texture = hunger3;
                    break;
                case float hunger when hunger <= 99:
                    hungerUI.texture = hunger2;
                    break;
                case float hunger when hunger > 99:
                    hungerUI.texture = hunger1;
                    break;
               */
            }
        }        
    }

    //Show death UI
    public void DeathScreen()
    {
        deathScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;

        player.GetComponent<PlayerManager>().enabled = false;
        player.GetComponent<Pick>().enabled = false;
        player.GetComponent<SpearManager>().enabled = false;
    }
}