using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    //Variables

    private GameObject player;

    public bool menuOpen;
    private bool settingsOpen;
    private bool journalOpen;

    public bool isNight;

    //UI Objects

    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;
    public GameObject playerUI;
    public GameObject journalUI;

    public GameObject closeJournal;

    public bool canPause;

    
    void Start()
    {        
        player = GameObject.FindGameObjectWithTag("Player");
        menuOpen = false;
        canPause = true;

    }

    void Update()
    {
        //Journal opening interractions
        if (Input.GetKeyDown(KeyCode.Escape) && canPause)
        {
            if (menuOpen && settingsOpen)
            {
                CloseSettings();
                Resume();
            }else if (menuOpen)
            {                
                Resume();
            }
            else
            {
                Pause();
                Time.timeScale = 0f;
            }
        }
        if (Input.GetKeyDown("j") &&!menuOpen && !settingsOpen && !isNight)
        {            
            if (journalOpen)
            {
                CloseJournal();
            }
            else
            {
                OpenJournal(true);
            }
        }
    }
    public void CloseJournal()
    {        
        journalUI.GetComponent<Journal>().CloseJournal();
        closeJournal.SetActive(false); //Button to close Journal
        journalOpen = false;

        if (menuOpen)
        {            
            journalUI.GetComponent<Journal>().openJournal.SetActive(true);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            player.GetComponent<PlayerManager>().enabled = true;

            journalUI.SetActive(false);            
        }
    }

    public void OpenJournal(bool moveMouse)
    {
        if (moveMouse)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            player.GetComponent<PlayerManager>().enabled = false;
        }        

        journalUI.SetActive(true);
        journalUI.GetComponent<Journal>().openJournal.SetActive(false);
        journalUI.GetComponent<Journal>().closeJournal.SetActive(false);
        journalUI.GetComponent<Journal>().OpenJournal();

        closeJournal.SetActive(true);

        journalOpen = true;
    }

    public void Resume()
    {
        if (!settingsOpen)
        {
            pauseMenuUI.SetActive(false);
            journalUI.SetActive(false);
            playerUI.SetActive(true);

            menuOpen = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            AudioListener.pause = false;

            player.GetComponent<PlayerManager>().enabled = true;
            Time.timeScale = 1f;
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        journalUI.SetActive(true);
        playerUI.SetActive(false);
        CloseJournal();
        menuOpen = true;
        Cursor.lockState = CursorLockMode.None;
        AudioListener.pause = true;
        Cursor.visible = true;
        player.GetComponent<PlayerManager>().enabled = false;
    }

    public void OpenSettings()
    {
        settingsMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        journalUI.SetActive(false);

        settingsOpen = true;
    }
    public void CloseSettings()
    {
        settingsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
        journalUI.SetActive(true);

        settingsOpen = false;
    }
    public void QuitGame()
    {
        Debug.Log("Quiting Game....");
        Application.Quit();
    }
}