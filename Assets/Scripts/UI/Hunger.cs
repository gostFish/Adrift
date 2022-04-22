using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class Hunger : MonoBehaviour
{
    public PostProcessProfile effects;
    private Vignette vig;
    private ChromaticAberration chr;
    private Bloom bloom;

    private float hunger;
    private float time;

    public float hungerRate; //Higher is quicker
    public float updateRate;

    public RawImage hungerUI;

    public Texture hunger1;
    public Texture hunger2;
    public Texture hunger3;
    public Texture hunger4;
    public Texture hunger5;

    public GameObject deathScreen;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Hunger"))
        {
            hunger = PlayerPrefs.GetFloat("Hunger");
        }
        else
        {
            hunger = 100f;
            PlayerPrefs.SetFloat("Hunger", hunger);
        }

        vig = effects.GetSetting<Vignette>();
        chr = effects.GetSetting<ChromaticAberration>();
        bloom = effects.GetSetting<Bloom>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time += Time.deltaTime;         

        if (time > updateRate)
        {
            hunger = PlayerPrefs.GetFloat("Hunger"); //Update the hunger
            hunger -= hungerRate;
            PlayerPrefs.SetFloat("Hunger", hunger); //Update the hunger
            

            //Debug.Log("Hunger = " + hunger);
            if(hunger <= 0)
            {
                DeathScreen();
            }
            else if (hunger <= 25)
            {
                hungerUI.texture = hunger5;
            }
            else if (hunger <= 50)
            {
                hungerUI.texture = hunger4;
            }
            else if (hunger <= 75)
            {
                hungerUI.texture = hunger3;
            }
            else if (hunger <= 99)
            {
                hungerUI.texture = hunger2;
            }
            else
            {
                hungerUI.texture = hunger1;
            }

            //Effects
            if(hunger <= 100 && hunger >= 0)
            {
                vig.intensity.value = 1 - (hunger/60);
            }
            
            if (hunger <= 80 && hunger >= 0)
            {
                chr.intensity.value = 1 - (0.0002f * Mathf.Pow(hunger, 2f));
            }
            if (hunger <= 100 && hunger >= 0)
            {
                bloom.intensity.value = 0.02f * Mathf.Tan(1.572f - (0.002f * hunger));
            }

           /* Debug.Log("Hunger = " + hunger);
            Debug.Log("vig = " + (1 - (0.3157 * Mathf.Pow(hunger, 0.25f))));
            Debug.Log("chr = " + (1 - (0.0001 * Mathf.Pow(hunger, 2))));
            Debug.Log("bloom = " + (0.02f * Mathf.Tan(1.572f - (0.005f * hunger))));*/

            time = 0;
        }        
    }

    public void DeathScreen()
    {
        deathScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }
}