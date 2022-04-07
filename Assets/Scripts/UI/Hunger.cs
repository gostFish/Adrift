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

    public RawImage hungerUI;

    public Texture hunger1;
    public Texture hunger2;
    public Texture hunger3;
    public Texture hunger4;
    public Texture hunger5;

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
       // gameObject.GetComponent<Text>().text = "Hunger or something = " + hunger.ToString("F2") + "%";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time += Time.deltaTime;         

        if (time > updateRate)
        {
            hunger = PlayerPrefs.GetFloat("Hunger"); //Update the hunger
            hunger -= hungerRate;
            //gameObject.GetComponent<Text>().text = "Hunger or something = " + hunger.ToString("F2") + "%";
            PlayerPrefs.SetFloat("Hunger", hunger); //Update the hunger
            time = 0;

            Debug.Log("Hunger = " + hunger);
            if (hunger <= 25)
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

        }

        
    }
}