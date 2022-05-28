using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class DayCycle : MonoBehaviour
{
    //Variables
    public int currentTick;

    private int dayTime;     //Number of ticks till day expires
    private int nightTime;   //Number of ticks till night expires

    public int dayTicks;
    public int nightTicks;

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
    private GameObject player;

    private GameObject sceneManager;
    private GameObject instanceManager;

    public GameObject hungerUi;
    public GameObject spearUI;
    public GameObject crosshairUI;
    public GameObject journalIndication;

    public GameObject journal;
    public GameObject ui;

    public GameObject dayShark;
    public GameObject nightShark1;
    public GameObject nightShark2;

    public CanvasGroup fadeUI;

    public Vector3[] pos;
    private int currentPos;
    
    public PostProcessProfile effects;
    private DepthOfField dof;

    public GameObject water;
    public Material dayWater;
    public Material nightWater;
 

    void Start()
    {
        currentPos = 0;

        //dayTicks;
        dayTime = dayTicks;
        nightTime = nightTicks;

        fadeIn = false;
        fadeOut = false;

        raft = GameObject.FindGameObjectWithTag("Raft");
        player = GameObject.FindGameObjectWithTag("Player");
        sceneManager = GameObject.FindGameObjectWithTag("SceneManager");
        instanceManager = GameObject.FindGameObjectWithTag("InstanceManager");
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
            //dayTime -= 1;
            //nightTime -= 1;
            currentTick += 1;
            time = 0;

            if(dayNum >= 5)
            {
                SceneManager.LoadScene("Credits");
            }
            if(isDay == 1 && dayTicks < currentTick)
            {
                PlayerPrefs.SetInt("DayNum", dayNum+1);
                currentTick = 0;
                NextPeriod();                
            }
            else if(isDay == 0 && nightTicks < currentTick)
            {
                currentTick = 0;
                NextPeriod();
            }            
        }

        if(isDay == 1 && currentTick > (dayTicks-1) && !journal.activeSelf && dayNum < 5)
        {
            journal.SetActive(true);
            ui.GetComponent<PauseMenu>().OpenJournal(false);
            StartCoroutine(journal.GetComponent<Journal>().OpenBlank());

        }

               
    }

    void NextPeriod()
    {
        dayTime = dayTicks;
        nightTime = nightTicks;
        
        StartCoroutine(FadeOut());
    }

    public void MakeDay()
    {
        //Set stats and player prefs
        if(dayNum > 1)
        {
            journal.SetActive(true);
            journal.GetComponent<Journal>().UpdatePage();
            ui.GetComponent<PauseMenu>().OpenJournal(true);

            player.GetComponent<SpearManager>().enabled = false;
        }
        water.GetComponent<MeshRenderer>().material = dayWater;
        dayNum = dayNum + 1;
        isDay = 1;
        SendFishToRaft(); //Fish teleported back to raft

        PlayerPrefs.SetInt("DayNum", dayNum); //Is new day, so also increase day timer
        PlayerPrefs.SetInt("IsDay", isDay);

        //Enable/Disable UI
        hungerUi.GetComponent<Hunger>().hungerRate = 0.1f;
        hungerUi.GetComponent<RawImage>().enabled = true;
        spearUI.SetActive(true);
        crosshairUI.SetActive(true);
        journalIndication.SetActive(true);

        //Set effects
        dof.active = false;

        //Activate Day things
        sceneManager.GetComponent<SpawnBirds>().enabled = true;
        sceneManager.GetComponent<SpawnFish>().enabled = true;
        nightShark1.SetActive(false);
        nightShark2.SetActive(false);
        dayShark.SetActive(true);

        raft.GetComponent<RaftMove>().isDay = true;

        mainCam.clearFlags = CameraClearFlags.Skybox;
        mainCam.GetComponent<AmbientAudio>().PlaySeaguls();
        
        player.GetComponent<SpearManager>().RefreshUI();
        ui.GetComponent<PauseMenu>().isNight = false;
    }

    public void MakeNight()
    {
        //Set stats and player prefs
        journal.SetActive(false);
        isDay = 0;
        PlayerPrefs.SetInt("IsDay", isDay);
        water.GetComponent<MeshRenderer>().material = nightWater;
        player.GetComponent<SpearManager>().enabled = false;
        Cursor.visible = false;

        //Enable/Disable UI
        hungerUi.GetComponent<Hunger>().hungerRate = 0.01f; //hunger decrease 10x slower
        hungerUi.GetComponent<RawImage>().enabled = false;
        spearUI.SetActive(false);
        crosshairUI.SetActive(false);
        journalIndication.SetActive(false);

        //Set effects
        dof.active = true;

        raft.GetComponent<RaftMove>().isDay = false;

        //Activate night things
        sceneManager.GetComponent<SpawnBirds>().enabled = false;
        sceneManager.GetComponent<SpawnFish>().enabled = false;
        nightShark1.SetActive(true);
        nightShark2.SetActive(true);
        dayShark.SetActive(false);

        mainCam.clearFlags = CameraClearFlags.SolidColor;
        mainCam.GetComponent<AmbientAudio>().PlayWhales();

        raft.GetComponentInChildren<SpearManager>().HideSpears();
        raft.GetComponentInChildren<SpearManager>().enabled = false;
        ui.GetComponent<PauseMenu>().isNight = true;
    }

    private void SendFishToRaft()
    {
        Vector3 raftPos = raft.transform.position;
        raftPos.y = 0.4f;
        for (int i = 0; i < instanceManager.transform.childCount; i++)
        {
            if (instanceManager.transform.GetChild(i).tag == "Fish")
            {

                instanceManager.transform.GetChild(i).transform.position = raftPos;
            }
        }
    }

    IEnumerator FadeOut()
    {
        //timeslice = time;
        time = 0;
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
        //timeslice = time;
        time = 0;
        fadeIn = true;
        yield return new WaitForSeconds(2);
        fadeIn = false;
    }
}
