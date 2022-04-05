using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hunger : MonoBehaviour
{

    private float hunger;
    private float time;

    public float hungerRate; //Higher is quicker
    public float updateRate;

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
        gameObject.GetComponent<Text>().text = "Hunger or something = " + hunger.ToString("F2") + "%";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time += Time.deltaTime;         

        if (time > updateRate)
        {
            hunger = PlayerPrefs.GetFloat("Hunger"); //Update the hunger
            hunger -= hungerRate;
            gameObject.GetComponent<Text>().text = "Hunger or something = " + hunger.ToString("F2") + "%";
            PlayerPrefs.SetFloat("Hunger", hunger); //Update the hunger
            time = 0;
        }

        
    }
}