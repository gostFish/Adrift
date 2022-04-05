using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private static bool GameIsPaused = false;

    private GameObject player;

    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;
    public GameObject playerUI;
    public GameObject journalUI;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");    
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        journalUI.SetActive(false);
        playerUI.SetActive(true);        

        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        AudioListener.pause = false;

        player.GetComponent<PlayerView1stPerson>().enabled = true;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        journalUI.SetActive(true);
        playerUI.SetActive(false);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        AudioListener.pause = true;

        player.GetComponent<PlayerView1stPerson>().enabled = false;
    }

    public void OpenSettings()
    {
        settingsMenuUI.SetActive(true);
    }
    public void CloseSettings()
    {
        settingsMenuUI.SetActive(false);
    }
    public void QuitGame()
    {
        Debug.Log("Quiting Game....");
        Application.Quit();
    }
}