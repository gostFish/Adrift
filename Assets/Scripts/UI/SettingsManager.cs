using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{

    //Setting variables
    public GameObject sound;
    public GameObject moveSpeed;
    public GameObject lookSpeed;

    //Objects
    private GameObject player;

    private void Start()
    {
        //Load saved data is any (otherwise scene defaults)
        if (PlayerPrefs.HasKey("volume"))
        {
            sound.GetComponent<Slider>().value = PlayerPrefs.GetFloat("volume");
            UpdateSound();
        }
        if (PlayerPrefs.HasKey("MoveSpeed"))
        {
            moveSpeed.GetComponent<Slider>().value = PlayerPrefs.GetFloat("MoveSpeed");
            UpdateMoveSpeed();
        }
        if (PlayerPrefs.HasKey("LookSpeed"))
        {
            lookSpeed.GetComponent<Slider>().value = PlayerPrefs.GetFloat("LookSpeed");
            UpdateLookSpeed();
        }
    }

    //Update the Sound scroller
    public void UpdateSound()
    {
        sound.GetComponentInChildren<Text>().text = "Volume " + Mathf.RoundToInt(((sound.GetComponent<Slider>().value) * 100)) + " %";
        AudioListener.volume = sound.GetComponent<Slider>().value;
        PlayerPrefs.SetFloat("volume", sound.GetComponent<Slider>().value);
    }

    //Update Player Moving speed
    public void UpdateMoveSpeed()
    {
        moveSpeed.GetComponentInChildren<Text>().text = "Player speed: " + Mathf.RoundToInt(((moveSpeed.GetComponent<Slider>().value)-0.1f)*2.5f);
        PlayerPrefs.SetFloat("MoveSpeed", moveSpeed.GetComponent<Slider>().value);

        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)//Player must be active to use its property
        {
            player.GetComponent<PlayerManager>().moveSpeed = moveSpeed.GetComponent<Slider>().value;
        }
    }

    //Update Looking speed
    public void UpdateLookSpeed()
    {
        lookSpeed.GetComponentInChildren<Text>().text = "Look speed: " + Mathf.RoundToInt(((lookSpeed.GetComponent<Slider>().value)-1)/4);
        PlayerPrefs.SetFloat("LookSpeed", lookSpeed.GetComponent<Slider>().value);

        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)//Player must be active to use its property
        {
            player.GetComponent<PlayerManager>().lookSpeed = lookSpeed.GetComponent<Slider>().value;
        }
    }
}
