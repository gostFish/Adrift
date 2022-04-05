using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Journal : MonoBehaviour
{
    //Pages

    public List<GameObject> pages;

    public GameObject page1;
    public GameObject page2;
    public GameObject page3;
    public GameObject page4;
    public GameObject page5;
    public GameObject page6;
    public GameObject page7;
    public GameObject page8;
    //Paragraphs

    //Other vars
    public GameObject nextPageButton;
    public GameObject previousPageButton;

    public GameObject openJournal;
    public GameObject closeJournal;

    public int currentPage;
    public int lastRevieled;

    void Start()
    {
        if (PlayerPrefs.HasKey("lastRevieled"))
        {
            lastRevieled = PlayerPrefs.GetInt("lastRevieled");
        }
        previousPageButton.SetActive(false);
        nextPageButton.SetActive(false);
        closeJournal.SetActive(false);

        pages = new List<GameObject>();
        pages.Add(page1);
        pages.Add(page2);
        pages.Add(page3);
        pages.Add(page4);
        pages.Add(page5);
        pages.Add(page6);
        pages.Add(page7);
        pages.Add(page8);
    }

    public void OpenJournal()
    {
        nextPageButton.SetActive(false);
        openJournal.SetActive(false);
        closeJournal.SetActive(true);

        currentPage = lastRevieled; //Default opens book on the last written page

        if (lastRevieled == 0)
        {            
            Debug.Log("Tried to open Journal, but no pages");
        }

        if (lastRevieled <= 2)
        {
            previousPageButton.SetActive(false);
        }
        else{
            previousPageButton.SetActive(true);
        }
        ShowPage();
    }

    public void CloseJournal()
    {
        openJournal.SetActive(true);
        closeJournal.SetActive(false);

        nextPageButton.SetActive(false);
        previousPageButton.SetActive(false);

        currentPage = 0;

        ShowPage();
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
        
        if((currentPage + 1) >= lastRevieled)
        {
            if (lastRevieled % 2 != 0)
            {
                currentPage++;
            }                      
        }
        else if(lastRevieled > 2)
        {
            currentPage += 2;
        }
        ShowPage();
    }

    public void PreviousPage()
    {
        if (currentPage == lastRevieled) //When pressed, on 0 (now no longer first page
        {
            nextPageButton.SetActive(true);
        }
        if(currentPage <= 4)
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
        else if(currentPage > 1)
        {
            currentPage -= 2;
        }

        ShowPage();
    }

    public void ShowPage() //Scroll through page
    {
        for(int i = 0; i < pages.Count; i++)
        {
            pages[i].SetActive(false);
        }

        if(currentPage > 0)
        {
            pages[currentPage - 1].SetActive(true);
        }        
    }

    public void UpdatePage() //Bring next thing to be visible in the pages
    {
        lastRevieled = PlayerPrefs.GetInt("lastRevieled");
        lastRevieled++;
        PlayerPrefs.SetInt("lastRevieled", lastRevieled);
    }
}
