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

    //Paragraphs

    //Other vars
    public GameObject nextPageButton;
    public GameObject previousPageButton;

    private int currentPage;
    public int lastPage;
    public int lastRevieled;

    void Start()
    {
        if (PlayerPrefs.HasKey("lastRevieled"))
        {
            lastRevieled = PlayerPrefs.GetInt("lastRevieled");
        }    
        
        pages = new List<GameObject>();
        pages.Add(page1);
        pages.Add(page2);
        pages.Add(page3);
        pages.Add(page4);
    }

    public void OpenBook()
    {
        if (lastRevieled >= 2)
        {
            currentPage = 2;
        }else if (lastRevieled == 1)
        {
            currentPage = 1;
        }
        else
        {
            Debug.Log("Tried to open Journal, but no pages");
        }
    }
    public void NextPage()
    {
        
        if (currentPage == 0) //When pressed, on 0 (now no longer first page
        {
            previousPageButton.SetActive(true);
        }


        if ((currentPage + 1) <= lastRevieled)
        {
            currentPage += 2;
        }else if((currentPage) <= lastRevieled)
        {
            currentPage++;
        }
        else
        {
            nextPageButton.SetActive(false);
        }
        FlipPage();
    }

    public void PreviousPage()
    {
        if (currentPage == lastRevieled) //When pressed, on 0 (now no longer first page
        {
            nextPageButton.SetActive(false);
        }


        if ((currentPage + 1) <= lastRevieled && currentPage > 0) //is not at the end
        {            
             currentPage -= 2;            
        }
        else if (currentPage + 1 > 0)
        {
            if (currentPage % 2 == 0) //is even number
            {
                currentPage -= 2;
            }
            else
            {
                currentPage--;
            }
        }
       // if()
       // {
      //      nextPageButton.SetActive(false);
     //   }
        FlipPage();
    }

    public void FlipPage() //Scroll through page
    {
        for(int i = 0; i < pages.Count; i++)
        {
            pages[i].SetActive(false);
        }

        pages[currentPage].SetActive(false);
    }

    public void UpdatePage() //Bring next thing to be visible in the pages
    {
        lastRevieled = PlayerPrefs.GetInt("lastRevieled");
        lastRevieled++;
        PlayerPrefs.SetInt("lastRevieled", lastRevieled);
    }

}
