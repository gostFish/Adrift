using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Journal : MonoBehaviour
{
    //Pages

    public List<GameObject> pages;
    public List<Texture> pageTextures;

    public GameObject page1;
    public GameObject page2;
    public GameObject page3;
    public GameObject page4;
    public GameObject page5;
    public GameObject page6;
    public GameObject page7;
    public GameObject page8;

    public Texture texturePage1;
    public Texture texturePage2;
    public Texture texturePage3;
    public Texture texturePage4;
    public Texture texturePage5;
    public Texture texturePage6;
    public Texture texturePage7;
   /* public Texture texturePage8;*/

    public Texture anim1;
    public Texture anim2;
    public Texture anim3;
    public Texture anim4;
    public Texture anim5;
    public Texture anim6;
    public Texture anim7;
    public Texture anim8;

    //Paragraphs

    //Other vars
    public GameObject nextPageButton;
    public GameObject previousPageButton;

    public GameObject openJournal;
    public GameObject closeJournal;

    public int currentPage;
    public int lastRevieled;

    private float time;

    private bool animatingForward;
    private bool animatingBackward;

    void Awake()
    {
        if (PlayerPrefs.HasKey("lastRevieled"))
        {
            lastRevieled = PlayerPrefs.GetInt("lastRevieled");
        }
        previousPageButton.SetActive(false);
        nextPageButton.SetActive(false);
        closeJournal.SetActive(false);

        pageTextures.Add(texturePage1);
        pageTextures.Add(texturePage2);
        pageTextures.Add(texturePage3);
        pageTextures.Add(texturePage4);
        pageTextures.Add(texturePage5);
        pageTextures.Add(texturePage6);
        pageTextures.Add(texturePage7);
       /* pageTextures.Add(texturePage8); */


        pages = new List<GameObject>();
        pages.Add(page1);
        pages.Add(page2);
        pages.Add(page3);
        pages.Add(page4);
        pages.Add(page5);
        pages.Add(page6);
        pages.Add(page7);
        pages.Add(page8);

        animatingForward = false;
        animatingBackward = false;
    }

    void Update()
    {
        //Debug.Log("updating");
        if (animatingForward)
        {
            ShowPage();
            time += Time.deltaTime;
            if(time < 0.05)
            {
                pages[currentPage-1].GetComponent<RawImage>().texture = anim1;
            }
            else if (time < 0.1)
            {
                pages[currentPage - 1].GetComponent<RawImage>().texture = anim2;
            }
            else if (time < 0.15)
            {
                pages[currentPage - 1].GetComponent<RawImage>().texture = anim3;
            }
            else if (time < 0.2)
            {
                pages[currentPage - 1].GetComponent<RawImage>().texture = anim4;
            }
            else if (time < 0.25)
            {
                pages[currentPage - 1].GetComponent<RawImage>().texture = anim5;
            }
            else if (time < 0.3)
            {
                pages[currentPage - 1].GetComponent<RawImage>().texture = anim6;
            }
            else if (time < 0.35)
            {
                pages[currentPage - 1].GetComponent<RawImage>().texture = anim7;
            }
            else if (time < 0.4)
            {
                pages[currentPage - 1].GetComponent<RawImage>().texture = anim8;
            }
            else if (time < 0.45)
            {
                pages[currentPage - 1].GetComponent<RawImage>().texture = anim1;
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
                pages[currentPage - 1].GetComponent<RawImage>().texture = anim1;
            }
            else if (time < 0.1)
            {
                pages[currentPage - 1].GetComponent<RawImage>().texture = anim8;
            }
            else if (time < 0.15)
            {
                pages[currentPage - 1].GetComponent<RawImage>().texture = anim7;
            }
            else if (time < 0.2)
            {
                pages[currentPage - 1].GetComponent<RawImage>().texture = anim6;
            }
            else if (time < 0.25)
            {
                pages[currentPage - 1].GetComponent<RawImage>().texture = anim5;
            }
            else if (time < 0.3)
            {
                pages[currentPage - 1].GetComponent<RawImage>().texture = anim4;
            }
            else if (time < 0.35)
            {
                pages[currentPage - 1].GetComponent<RawImage>().texture = anim3;
            }
            else if (time < 0.4)
            {
                pages[currentPage - 1].GetComponent<RawImage>().texture = anim2;
            }
            else if (time < 0.45)
            {
                pages[currentPage - 1].GetComponent<RawImage>().texture = anim1;
                ActualPages();
                animatingBackward = false;
            }

        }

    }
    public void ActualPages()
    {
        Debug.Log("current Page = " + currentPage);
        for(int i = 0; i < currentPage; i++)
        {
            Debug.Log("i = " + i);
            pages[i].GetComponent<RawImage>().texture = pageTextures[i];
        }
        
    }
    public void OpenJournal()
    {
        Time.timeScale = 1f;

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
            Time.timeScale = 0f; //Pause again if menu is opened
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
        time = 0;
        animatingForward = true;
        //ShowPage();
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
        time = 0;
        animatingBackward = true;
        //ShowPage();
    }

    public void ShowPage() //Scroll through page
    {
        for(int i = 0; i < pages.Count; i++)
        {
            pages[i].SetActive(false);
        }

        if(currentPage > 0 && (currentPage - 1) < pages.Count)
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
