using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class DayCycle : MonoBehaviour
{
    //Variables
    public int dayTime;     //Number of ticks till day expires
    public int nightTime;   //Number of ticks till night expires

    private float time;
    private float timeslice;
    public int dayNum;
    public int isDay;  //Starts day or night scene (1 = day, 0 = night)

    public float updateTime;    //Check new time

    private bool fadeIn;
    private bool fadeOut;

    //Game objects
    private Camera mainCam;
    private GameObject raft;

    private GameObject sceneManager;
    public GameObject hungerUi;
    public GameObject spearUI;
    public GameObject crosshairUI;

    public GameObject dayShark;
    public GameObject nightShark1;
    public GameObject nightShark2;

    public CanvasGroup fadeUI;

    public Vector3[] pos;
    private int currentPos;
    
    public PostProcessProfile effects;
    private DepthOfField dof;

 

    void Start()
    {
        currentPos = 0;

        dayTime = 10;
        nightTime = 6;

        fadeIn = false;
        fadeOut = false;

        raft = GameObject.FindGameObjectWithTag("Raft");
        sceneManager = GameObject.FindGameObjectWithTag("SceneManager");
        mainCam = Camera.main;

        dof = effects.GetSetting<DepthOfField>();

        //Start at daytime
        MakeDay();

        if (PlayerPrefs.HasKey("DayNum"))
        {
            dayNum = PlayerPrefs.GetInt("DayNum");
        }
        else
        {
            dayNum = 1;
            PlayerPrefs.SetInt("DayNum",dayNum);
        }

        if (PlayerPrefs.HasKey("IsDay"))
        {
            isDay = PlayerPrefs.GetInt("IsDay");
        }
        else
        {
            isDay = 1;
            PlayerPrefs.SetInt("IsDay", isDay);
        }
    }
    
    void FixedUpdate()
    {
        time = time + Time.deltaTime;

        if (fadeOut)
        {
            fadeUI.alpha = Mathf.Lerp(0, 1, time / updateTime);
        }
        else if (fadeIn)
        {
            fadeUI.alpha = Mathf.Lerp(1, 0, time / updateTime);
        }
        else if (time > updateTime)
        {
            dayTime -= 1;
            nightTime -= 1;
            time = 0;

            if(isDay == 1 && dayTime < 0)
            {
                PlayerPrefs.SetInt("DayNum", dayNum+1);
                NextPeriod();                
            }
            else if(isDay == 0 && nightTime < 0)
            {                
                NextPeriod();
            }            
        }

               
    }

    void NextPeriod()
    {
        dayTime = 10;
        nightTime = 6;
        
        StartCoroutine(FadeOut());
    }

    public void MakeDay()
    {
        //Set stats and player prefs
        dayNum = dayNum + 1;
        isDay = 1;

        PlayerPrefs.SetInt("DayNum", dayNum); //Is new day, so also increase day timer
        PlayerPrefs.SetInt("IsDay", isDay);

        //Enable/Disable UI
        hungerUi.GetComponent<Hunger>().hungerRate = 0.1f;
        hungerUi.GetComponent<RawImage>().enabled = true;
        spearUI.SetActive(true);
        crosshairUI.SetActive(true);

        //Set effects
        dof.active = false;

        //Activate Day things
        sceneManager.GetComponent<SpawnBirds>().enabled = true;
        sceneManager.GetComponent<SpawnFish>().enabled = true;
        nightShark1.SetActive(false);
        nightShark2.SetActive(false);
        dayShark.SetActive(true);

        mainCam.clearFlags = CameraClearFlags.Skybox;
        mainCam.GetComponent<AmbientAudio>().PlaySeaguls();
    }

    public void MakeNight()
    {
        //Set stats and player prefs
        isDay = 0;
        PlayerPrefs.SetInt("IsDay", isDay);

        //Enable/Disable UI
        hungerUi.GetComponent<Hunger>().hungerRate = 0.01f; //hunger decrease 10x slower
        hungerUi.GetComponent<RawImage>().enabled = false;
        spearUI.SetActive(false);
        crosshairUI.SetActive(false);

        //Set effects
        dof.active = true;

        //Activate night things
        sceneManager.GetComponent<SpawnBirds>().enabled = false;
        sceneManager.GetComponent<SpawnFish>().enabled = false;
        nightShark1.SetActive(true);
        nightShark2.SetActive(true);
        dayShark.SetActive(false);
        mainCam.clearFlags = CameraClearFlags.SolidColor;
        mainCam.GetComponent<AmbientAudio>().PlayWhales();
    }

    IEnumerator FadeOut()
    {
        timeslice = time;
        fadeOut = true;
        yield return new WaitForSeconds(2);
        fadeOut = false;
        currentPos++;
        raft.transform.position = pos[currentPos];
        if (isDay == 1)
        {
            MakeNight();
        }
        else
        {
            MakeDay();
        }
        StartCoroutine(FadeIn());
    }
    IEnumerator FadeIn()
    {
        timeslice = time;
        fadeIn = true;
        yield return new WaitForSeconds(2);
        fadeIn = false;
    }
}
