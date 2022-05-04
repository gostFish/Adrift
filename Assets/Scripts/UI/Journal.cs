using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Journal : MonoBehaviour
{
    //Pages and Animation

    public List<GameObject> pages;
    public List<Texture> pageTextures;
    public List<Texture> animation;

    //Game objects (UI)
    public GameObject nextPageButton;
    public GameObject previousPageButton;

    public GameObject openJournal;
    public GameObject closeJournal;

    //Other vars
    public int currentPage;
    public int lastRevieled;

    private float time;

    private bool animatingForward;
    private bool animatingBackward;

    void Awake()
    {
        //Save to memory which the last page the player unlocked is
        if (PlayerPrefs.HasKey("lastRevieled"))
        {
            lastRevieled = PlayerPrefs.GetInt("lastRevieled");
        }

        previousPageButton.SetActive(false);
        nextPageButton.SetActive(false);
        closeJournal.SetActive(false);

        animatingForward = false;
        animatingBackward = false;
    }

    void Update()
    {
        
        //Forward animations
        if (animatingForward)
        {
            ShowPage();
            time += Time.deltaTime;
            if (time < 0.05)
            {
                pages[currentPage - 1].GetComponent<RawImage>().texture = animation[0];
            }
            else if (time < 0.1)
            {
                pages[currentPage - 1].GetComponent<RawImage>().texture = animation[1];
            }
            else if (time < 0.15)
            {
                pages[currentPage - 1].GetComponent<RawImage>().texture = animation[2];
            }
            else if (time < 0.2)
            {
                pages[currentPage - 1].GetComponent<RawImage>().texture = animation[3];
            }
            else if (time < 0.25)
            {
                pages[currentPage - 1].GetComponent<RawImage>().texture = animation[4];
            }
            else if (time < 0.3)
            {
                pages[currentPage - 1].GetComponent<RawImage>().texture = animation[5];
            }
            else if (time < 0.35)
            {
                pages[currentPage - 1].GetComponent<RawImage>().texture = animation[6];
            }
            else if (time < 0.4)
            {
                pages[currentPage - 1].GetComponent<RawImage>().texture = animation[7];
            }
            else if (time < 0.45)
            {
                pages[currentPage - 1].GetComponent<RawImage>().texture = animation[0];
                ActualPages();
                animatingForward = false;
            }
        }
        if (animatingBackward)
        {
            ShowPage();
            time += Time.deltaTime;
            if (time < 0.05)
            {
                pages[currentPage - 1].GetComponent<RawImage>().texture = animation[0];
            }
            else if (time < 0.1)
            {
                pages[currentPage - 1].GetComponent<RawImage>().texture = animation[7];
            }
            else if (time < 0.15)
            {
                pages[currentPage - 1].GetComponent<RawImage>().texture = animation[6];
            }
            else if (time < 0.2)
            {
                pages[currentPage - 1].GetComponent<RawImage>().texture = animation[5];
            }
            else if (time < 0.25)
            {
                pages[currentPage - 1].GetComponent<RawImage>().texture = animation[4];
            }
            else if (time < 0.3)
            {
                pages[currentPage - 1].GetComponent<RawImage>().texture = animation[3];
            }
            else if (time < 0.35)
            {
                pages[currentPage - 1].GetComponent<RawImage>().texture = animation[2];
            }
            else if (time < 0.4)
            {
                pages[currentPage - 1].GetComponent<RawImage>().texture = animation[1];
            }
            else if (time < 0.45)
            {
                pages[currentPage - 1].GetComponent<RawImage>().texture = animation[0];
                ActualPages();
                animatingBackward = false;
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

        //Reset UI
        nextPageButton.SetActive(false);
        openJournal.SetActive(false);
        closeJournal.SetActive(true);

        currentPage = lastRevieled; //Default opens book on the last written page

        if (lastRevieled == 0)
        {
            Debug.Log("Tried to open Journal, but no pages");
        }

        //Hide buttons to prevent going out of the list
        if (lastRevieled <= 2)
        {
            previousPageButton.SetActive(false);
        }
        else
        {
            previousPageButton.SetActive(true);
        }
        ShowPage();
        ActualPages();
    }

    public void CloseJournal()
    {
        openJournal.SetActive(true);
        closeJournal.SetActive(false);

        nextPageButton.SetActive(false);
        previousPageButton.SetActive(false);

        currentPage = 0;
        ShowPage();

        if (gameObject.GetComponentInParent<PauseMenu>().menuOpen)
        {
            Time.timeScale = 0f; //Pause again if menu was open
        }
    }


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
        time = 0;
        animatingForward = true;
    }

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
        time = 0;
        animatingBackward = true;
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
        lastRevieled = PlayerPrefs.GetInt("lastRevieled");
        lastRevieled++;
        PlayerPrefs.SetInt("lastRevieled", lastRevieled); //Save to disk which is last revealed
    }
}
