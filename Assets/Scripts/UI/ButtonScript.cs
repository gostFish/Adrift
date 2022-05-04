using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    //Buttons may be assigned a function from the below
    //This script should be attached to the "Scene Manager" object, drag that to the "On click" to get its function
    public void StartGame()
    {
        SceneManager.LoadScene("Level");
    }
    public void LoadGame()
    {
        //Not done yet
    }
    public void OpenSettings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
