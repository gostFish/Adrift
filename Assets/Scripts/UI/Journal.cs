using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Journal : MonoBehaviour
{
    //Pages and Animation

    public List<GameObject> pages;
    public List<Texture> pageTextures;
    public List<Texture> anim;

    private bool animatingForward;
    private bool animatingBackward;

    //Game objects (UI)
    public GameObject nextPageButton;
    public GameObject previousPageButton;

    public GameObject openJournal;
    public GameObject closeJournal;

    public GameObject toStory;
    public GameObject toTut;
    public GameObject toNew;

    private GameObject player;

    //Other vars
    public int currentPage;
    public int lastRevieled;

    private float time;
    private bool leaveBlank;

    private bool firstOpen;

    void Awake()
    {
        //Save to memory which the last page the player unlocked is
        /*if (PlayerPrefs.HasKey("lastRevieled"))
        {
            lastRevieled = PlayerPrefs.GetInt("lastRevieled");
        }*/
        player = GameObject.FindGameObjectWithTag("Player");
        previousPageButton.SetActive(false);
        nextPageButton.SetActive(false);
        closeJournal.SetActive(false);

        animatingForward = false;
        animatingBackward = false;

        firstOpen = true;
    }

    void Update()
    {
        
        //Forward animations
        if (animatingForward)
        {
            ShowPage();
            time += Time.deltaTime;
            switch (time)
            {
                case float time when time < 0.05:
                    pages[currentPage - 1].GetComponent<RawImage>().texture = anim[0];
                    break;
                case float time when time < 0.1:
                    pages[currentPage - 1].GetComponent<RawImage>().texture = anim[1];
                    break;
                case float time when time < 0.15:
                    pages[currentPage - 1].GetComponent<RawImage>().texture = anim[2];
                    break;
                case float time when time < 0.2:
                    pages[currentPage - 1].GetComponent<RawImage>().texture = anim[3];
                    break;
                case float time when time < 0.25:
                    pages[currentPage - 1].GetComponent<RawImage>().texture = anim[4];
                    break;
                case float time when time < 0.3:
                    pages[currentPage - 1].GetComponent<RawImage>().texture = anim[5];
                    break;
                case float time when time < 0.35:
                    pages[currentPage - 1].GetComponent<RawImage>().texture = anim[6];
                    break;
                case float time when time < 0.4:
                    pages[currentPage - 1].GetComponent<RawImage>().texture = anim[7];
                    break;
                case float time when time < 0.45:
                    pages[currentPage - 1].GetComponent<RawImage>().texture = anim[0];
                    if (!leaveBlank)
                    {
                        ActualPages();
                    }                    
                    animatingForward = false;
                    break;
            }            
        }
        if (animatingBackward)
        {
            ShowPage();
            time += Time.deltaTime;
            switch (time)
            {
                case float time when time < 0.05:
                    pages[currentPage - 1].GetComponent<RawImage>().texture = anim[0];
                    break;
                case float time when time < 0.1:
                    pages[currentPage - 1].GetComponent<RawImage>().texture = anim[7];
                    break;
                case float time when time < 0.15:
                    pages[currentPage - 1].GetComponent<RawImage>().texture = anim[6];
                    break;
                case float time when time < 0.2:
                    pages[currentPage - 1].GetComponent<RawImage>().texture = anim[5];
                    break;
                case float time when time < 0.25:
                    pages[currentPage - 1].GetComponent<RawImage>().texture = anim[4];
                    break;
                case float time when time < 0.3:
                    pages[currentPage - 1].GetComponent<RawImage>().texture = anim[3];
                    break;
                case float time when time < 0.35:
                    pages[currentPage - 1].GetComponent<RawImage>().texture = anim[2];
                    break;
                case float time when time < 0.4:
                    pages[currentPage - 1].GetComponent<RawImage>().texture = anim[1];
                    break;
                case float time when time < 0.45:
                    pages[currentPage - 1].GetComponent<RawImage>().texture = anim[0];
                    ActualPages();
                    animatingBackward = false;
                    break;
            }            
        }
    }

    public void ActualPages()
    {
        //Assigning the textures to the pages (for those revealed)
        for (int i = 0; i < currentPage; i++)
        {
            pages[i].GetComponent<RawImage>().texture = pageTextures[i];
        }
    }

    public void OpenJournal()
    {
        Time.timeScale = 1f; //Unpause if paused

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // player.GetComponent<PlayerManager>().enabled = false;
        player.GetComponent<SpearManager>().enabled = false;
        //Reset UI
        nextPageButton.SetActive(false);
        openJournal.SetActive(false);
        closeJournal.SetActive(true);

        if (firstOpen)
        {
            currentPage = 2;
            firstOpen = false;
            previousPageButton.SetActive(false);
            nextPageButton.SetActive(true);
        }
        else
        {
            currentPage = lastRevieled; //Default opens book on the last written page
                                        
            if (lastRevieled <= 2)//Hide buttons to prevent going out of the list
            {
                previousPageButton.SetActive(false);
            }
            else
            {
                previousPageButton.SetActive(true);
            }
        }
        

        if (lastRevieled == 0)
        {
            Debug.Log("Tried to open Journal, but no pages");
        }

        
        ShowPage();
        ActualPages();
    }

    public IEnumerator OpenBlank()
    {        
        
        yield return new WaitForSeconds(1);
        time = 0;
        if(lastRevieled % 2 == 0)
        {
            animatingForward = true;
            leaveBlank = true;
        }
        
    }

    //Button to close UI
    public void CloseJournal()
    {
        openJournal.SetActive(true);
        closeJournal.SetActive(false);

        nextPageButton.SetActive(false);
        previousPageButton.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        player.GetComponent<PlayerManager>().enabled = true;
        player.GetComponent<SpearManager>().enabled = true;

        currentPage = 0;
        ShowPage();

        if (gameObject.GetComponentInParent<PauseMenu>().menuOpen)
        {
            Time.timeScale = 0f; //Pause again if menu was open
        }
    }

    //Get next possible page
    public void NextPage()
    {
        if (currentPage <= 2) //When pressed, on 0 (now no longer first page)
        {
            previousPageButton.SetActive(true);
        }
        if ((currentPage + 2) >= lastRevieled) //is last page (or last half page)
        {
            nextPageButton.SetActive(false);
        }

        if ((currentPage + 1) >= lastRevieled)
        {
            if (lastRevieled % 2 != 0)
            {
                currentPage++;
            }
        }
        else if (lastRevieled > 2)
        {
            currentPage += 2;
        }

        //For the animation
        time = 0;
        animatingForward = true;
        leaveBlank = false;
    }

    //Get previous possible page
    public void PreviousPage()
    {
        if (currentPage == lastRevieled) //When pressed, on 0 (now no longer first page
        {
            nextPageButton.SetActive(true);
        }
        if (currentPage <= 4)
        {
            previousPageButton.SetActive(false);
        }

        if ((currentPage) >= lastRevieled)
        {
            if (lastRevieled % 2 != 0)
            {
                currentPage--;
            }
            else
            {
                currentPage -= 2;
            }
        }
        else if (currentPage > 1)
        {
            currentPage -= 2;
        }

        //For the animation
        time = 0;
        animatingBackward = true;
    }

    public void ToLore()
    {
        if(currentPage > 2)
        {
            time = 0;
            animatingBackward = true;
            currentPage = 2;
            nextPageButton.SetActive(true);
            //ShowPage();
        }
        
    }

    public void ToTut()
    {
        if (currentPage > 10)
        {
            time = 0;
            animatingBackward = true;
            currentPage = 10;
            
           // ShowPage();
        }else if (currentPage < 10)
        {
            time = 0;
            animatingForward = true;
            currentPage = 10;
            ShowPage();
        }
        nextPageButton.SetActive(true);
        previousPageButton.SetActive(true);

    }

    public void ToNew()
    {
        if(currentPage != lastRevieled)
        {
            time = 0;
            animatingForward = true;
            currentPage = lastRevieled;
            //ShowPage();
        }

        nextPageButton.SetActive(false);
        previousPageButton.SetActive(true);
    }

    public void ShowPage() //Scroll through page
    {
        
        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].SetActive(false);
        }
        if (currentPage > 0 && (currentPage - 1) < pages.Count)
        {
            pages[currentPage - 1].SetActive(true);
        }
        
    }

    public void UpdatePage() //Bring next thing to be visible in the pages
    {
        //lastRevieled = PlayerPrefs.GetInt("lastRevieled");
        lastRevieled = lastRevieled + 1;
        //PlayerPrefs.SetInt("lastRevieled", lastRevieled); //Save to disk which is last revealed
    }
}
