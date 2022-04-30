using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public bool menuOpen = false;

    private bool settingsOpen;

    private GameObject player;

    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;
    public GameObject playerUI;
    public GameObject journalUI;

    public GameObject closeJournal;

    private bool journalOpen;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuOpen)
            {
                Resume();

            }
            else
            {
                Pause();
                Time.timeScale = 0f;
            }
        }
        if (Input.GetKeyDown("j"))
        {
            Debug.Log("Journal opened");
            if (journalOpen)
            {
                CloseJournal();
            }
            else
            {
                Debug.Log("Opening Journal");
                OpenJournal();
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
            player.GetComponent<PlayerView1stPerson>().enabled = true;

            journalUI.SetActive(false);            
        }
    }

    public void OpenJournal()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        player.GetComponent<PlayerView1stPerson>().enabled = false;
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
            AudioListener.pause = false;

            player.GetComponent<PlayerView1stPerson>().enabled = true;
            Time.timeScale = 1f;
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        journalUI.SetActive(true);
        playerUI.SetActive(false);

        menuOpen = true;
        Cursor.lockState = CursorLockMode.None;
        AudioListener.pause = true;

        player.GetComponent<PlayerView1stPerson>().enabled = false;
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