using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpearManager : MonoBehaviour
{
    //Variables

    public int hasSpear;

    private RaycastHit hit;
    private int spearHealth;

    private Vector3 holdPos;
    private Vector3 stabDrawBackPos;
    private Vector3 stabPoint;

    private Vector3 fishStabbed;
    private Vector3 fishBack;

    private Quaternion holdAngle;
    private Quaternion drawAngle;
    private Quaternion stabAngle;

    public Vector3 hitPos;
    public Vector3 clickPos;

    //Throwing / stabbing

    public float despawnDist;

    public int stabRange;
    public int maxSpearHits;
    private int activeSpear;

    public float fishHungerUp;
    private float hunger;

    private bool holding;
    public bool clickToStab;

    private int fishMask;
    private int waterMask;
    private int raftMask;

    private float stabAnimTime;
    public bool stabbing;
    public bool strikeContact;

    private int planksRemaning;

            //Shark hits
    public int minHits;
    private int currentHits;

    private bool nothingHit;

    //Game Objects

    private GameObject player;
    private GameObject shark;
    private Camera mainCam;

    public GameObject splash;
    public GameObject ripple;
    public GameObject blood;

    private GameObject splashInst;
    private GameObject rippleInst;
    private GameObject bloodInst;

    public GameObject[] spearPrefab;
    public GameObject[] fishPrefab;
    public GameObject[] spear; //As an instance
    private GameObject[] fish;
    private GameObject targetFish;

    private GameObject instanceManager;

    // Images and Textures 
    public RawImage spearUI;

    public Texture transparent;
    public Texture spear1;
    public Texture spear2;
    public Texture spear3;
    public Texture spear4;
    public Texture spear5;

    public RawImage crosshair;
    public Text plankCountImg;

    public Texture greyCH;
    public Texture redCH;
    public Texture greenCH;
    public Texture orangeCH;

    // Audio

    private AudioSource audioSource;
    public AudioClip spearDegredation;
    public AudioClip splashSound;


    void Start()
    {
        //Temporary for testing (will always start with spear)
        PlayerPrefs.SetInt("HasSpear", 0);
        PlayerPrefs.SetFloat("Hunger", 100);

        //Find Essential things
        mainCam = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player");
        shark = GameObject.FindGameObjectWithTag("Shark");

        fishHungerUp = 8;
        audioSource = GetComponent<AudioSource>();

        fishMask = LayerMask.GetMask("Interactable", "Raft"); //Engore everything except fish
        waterMask = LayerMask.GetMask("Water");
        raftMask = LayerMask.GetMask("Raft");

        holdPos = new Vector3(0.4f, 0.1f, 0.4f);  //Position spear to view in 1st person
        stabDrawBackPos = new Vector3(0.6f, 0.35f, -0.4f);
        stabPoint = new Vector3(0.4f, 0.1f, 1.2f);

        fishStabbed = new Vector3(0.2f, -0.016f, 1.78f);
        fishBack = new Vector3(0.35f, -0.06f, 1.45f);

        holdAngle = Quaternion.Euler(0, 90f, 10f);
        drawAngle = Quaternion.Euler(0, 85f, 9f);
        stabAngle = Quaternion.Euler(0, 73f,4.5f);

        fish = new GameObject[fishPrefab.Length];
        StartCoroutine(CheckPlanksDelay());

        stabAnimTime = 3;
        nothingHit = true;
        GetSpear();
        GetFish();
        //Remember when last playing
        if (PlayerPrefs.HasKey("HasSpear"))
        {
            hasSpear = PlayerPrefs.GetInt("HasSpear"); //If 0, no spear, if 1 there is a spear
            if (hasSpear == 1)
            {
                RefreshSpear();
                clickToStab = true;
            }
            else
            {
                spear[0].SetActive(false);
            }
        }
        else
        {
            PlayerPrefs.SetInt("HasSpear", 0); //Default, no spear
            clickToStab = false;
        }
        if (PlayerPrefs.HasKey("SpearHealth"))
        {
            spearHealth = PlayerPrefs.GetInt("spearHealth");
        }
        if (PlayerPrefs.HasKey("Hunger"))
        {
            hunger = PlayerPrefs.GetFloat("Hunger");
        }
        else
        {
            PlayerPrefs.SetFloat("Hunger", 100f);
        }

        RefreshUI();

        //Instantiate once (Hide and unhide splash instead of intantiating)
        instanceManager = GameObject.FindGameObjectWithTag("InstanceManager");

        splashInst = Instantiate(splash, new Vector3(0,0,0), Quaternion.identity);
        rippleInst = Instantiate(ripple, new Vector3(0, 0, 0), Quaternion.identity);
        bloodInst = Instantiate(blood, new Vector3(0, 0, 0), Quaternion.identity);

        splashInst.transform.parent = instanceManager.transform;
        rippleInst.transform.parent = instanceManager.transform;
        bloodInst.transform.parent = instanceManager.transform;

        splashInst.SetActive(false);
        rippleInst.SetActive(false);
        bloodInst.SetActive(false);
    }

  
    void FixedUpdate()
    {
        
        //Animate Stabbing
        if (stabbing == true)
        {
            if (stabAnimTime < 1) //draw back
            {
                stabAnimTime += Time.deltaTime / 0.3f;
                spear[activeSpear].transform.localPosition = Vector3.Lerp(holdPos, stabDrawBackPos, stabAnimTime);
                //fish.transform.localPosition = spear.transform.localPosition + new Vector3(-0.1f,-0.1f,0.7f);
                spear[activeSpear].transform.localRotation = Quaternion.Lerp(holdAngle, drawAngle, stabAnimTime);
            }
            else if (stabAnimTime <= 2 && hit.point != null) //stab
            {
                stabAnimTime += Time.deltaTime / 0.2f;
                spear[activeSpear].transform.localPosition = Vector3.Lerp(stabDrawBackPos, stabPoint, stabAnimTime - 1f);
                if(targetFish != null)
                {
                    targetFish.transform.localPosition = fishStabbed;
                }                
                spear[activeSpear].transform.localRotation = Quaternion.Lerp(drawAngle, stabAngle, stabAnimTime-1f);
            }
            else if (stabAnimTime <= 3) //Return to origin
            {
                stabAnimTime += Time.deltaTime / 0.5f;
                spear[activeSpear].transform.localPosition = Vector3.Lerp(stabPoint, holdPos, stabAnimTime - 2f);
                if (targetFish != null)
                {
                    targetFish.transform.localPosition = Vector3.Lerp(fishStabbed, fishBack, stabAnimTime - 2f);
                }                
                spear[activeSpear].transform.localRotation = Quaternion.Lerp(stabAngle, holdAngle, stabAnimTime-2f);
            }
            else
            {                                
                if (targetFish != null)
                {
                    targetFish.SetActive(false);
                }
                stabbing = false;
            }
        }else if (spear != null) //reposition spear if exists
        {
            spear[activeSpear].transform.localPosition = holdPos;
            spear[activeSpear].transform.rotation = mainCam.transform.rotation * Quaternion.Euler(15, 15, 0);
            spear[activeSpear].transform.rotation = mainCam.transform.rotation * Quaternion.Euler(0, 90, 10);

            if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, stabRange, fishMask))
            {
                if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, stabRange, raftMask))
                {
                    if (PlayerPrefs.GetInt("HasSpear") == 0)
                    {
                        crosshair.texture = orangeCH;
                        player.GetComponent<PlayerManager>().canPickLog = true;
                        plankCountImg.text = planksRemaning.ToString();
                    }
                    else
                    {
                        crosshair.texture = greyCH;
                        plankCountImg.text = "";
                    }
                    player.GetComponent<PlayerManager>().lookingAtLog = true;
                }
                else if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, stabRange, fishMask))
                {
                    if (PlayerPrefs.GetInt("HasSpear") == 1)
                    {
                        if (hit.transform.tag == "Shark")
                        {
                            crosshair.texture = redCH;
                            plankCountImg.text = "";
                        }
                        else if (hit.transform.tag == "Fish")
                        {
                            crosshair.texture = greenCH;
                            plankCountImg.text = "";
                        }
                    }
                }
            }            
            else
            {
                crosshair.texture = greyCH;
                plankCountImg.text = "";
                player.GetComponent<PlayerManager>().canPickLog = false;
                player.GetComponent<PlayerManager>().lookingAtLog = false;
            }
        }
               
    }

    void Update()
    {

        if (stabAnimTime > 1.1 && stabAnimTime < 1.2f && nothingHit) //Time for strike to count
        {
            Strike();
        }
        else
        {
            nothingHit = true;
            strikeContact = false;
        }


        if (spear != null) //reposition spear if exists
        {
            //Change crosshair
            if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, stabRange, fishMask))
            {
                if (PlayerPrefs.GetInt("HasSpear") == 1)
                {
                    if (hit.transform.tag == "Shark")
                    {
                        crosshair.texture = redCH;
                    }
                    else if (hit.transform.tag == "Raft")
                    {
                        crosshair.texture = greyCH;
                    }
                    else if (hit.transform.tag == "Fish")
                    {
                        crosshair.texture = greenCH;
                    }
                }
                if (Input.GetMouseButtonDown(0)) //Player clicking
                {
                    clickPos = hit.point;
                }
            }
            else if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, stabRange, raftMask))
            {

                if (PlayerPrefs.GetInt("HasSpear") == 0)
                {
                    crosshair.texture = orangeCH;
                    player.GetComponent<PlayerManager>().canPickLog = true;
                }
                else
                {
                    crosshair.texture = greyCH;
                }
                player.GetComponent<PlayerManager>().lookingAtLog = true;

            }else if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, stabRange, waterMask)) //Clicking water
            {
                if (Input.GetMouseButtonDown(0)) //Player clicking
                {
                    clickPos = hit.point;
                }
            }
            else
            {
                crosshair.texture = greyCH;
                player.GetComponent<PlayerManager>().canPickLog = false;
                player.GetComponent<PlayerManager>().lookingAtLog = false;
            }
        }

        if (Input.GetMouseButtonDown(0) && !stabbing && clickToStab && 
            !Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, stabRange, raftMask) &&
            Time.timeScale == 1) //stab animation
        {
            if (PlayerPrefs.GetInt("HasSpear") == 1)//Does not have a spear
            {
                stabbing = true;
                stabAnimTime = 0;
            }
        }
    }

    public void GetSpear() //Instantiate the spear
    {
        for(int i = 0; i < spearPrefab.Length;i++)
        {
            spear[i] = Instantiate(spearPrefab[i]);

            spear[i].transform.parent = mainCam.transform;
            spear[i].transform.localPosition = holdPos;

            spear[i].GetComponent<Rigidbody>().isKinematic = true;
            spear[i].GetComponent<Rigidbody>().useGravity = false;
            spear[i].GetComponent<BoxCollider>().enabled = false;

            spear[i].SetActive(false);
        }        
    }

    public void GetFish()
    {
        for(int i = 0; i < fish.Length; i++)
        {
            fish[i] = Instantiate(fishPrefab[i]);
            fish[i].GetComponent<FishMovement>().enabled = false;
            fish[i].GetComponent<BoxCollider>().enabled = false;
            fish[i].transform.parent = mainCam.transform;
            fish[i].transform.localPosition = new Vector3(0.35f, -0.1f, 1.4f);
            fish[i].transform.localRotation = Quaternion.Euler(-50f, -50f, -150);
            fish[i].SetActive(false);
        }
        
    }
    //Update Spear UI
    public void RefreshSpear()
    {
        holding = true;

        PlayerPrefs.SetInt("SpearHealth", maxSpearHits);
        spearHealth = maxSpearHits;
        spear[0].SetActive(true);
        activeSpear = 0;
        RefreshUI();
    }

    //Hit something
    private void Strike()
    {      
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, stabRange, fishMask))
        {
            if (hit.transform.tag == "Fish" && nothingHit)
            {
                targetFish = fish[hit.transform.GetComponent<FishType>().type - 1];
                hunger = PlayerPrefs.GetFloat("Hunger");
                hunger += targetFish.GetComponent<FishType>().hungerFill;
                Debug.Log("Captured fish " + targetFish.GetComponent<FishType>().type + " and restored " + targetFish.GetComponent<FishType>().hungerFill + "% hunger");

                if (hunger > 100)
                {
                    hunger = 100f;
                }
                PlayerPrefs.SetFloat("Hunger", hunger);

                hit.transform.gameObject.active = false;
                spearHealth--;

                audioSource.PlayOneShot(spearDegredation);
                RefreshUI();
                //StartCoroutine(BreakDelay());
               // if (spearHealth > 0)
               // {
                    StartCoroutine(showFish());
                    //targetFish.SetActive(true);
               // }
                nothingHit = false;
            }
            else if (hit.transform.tag == "Shark" && nothingHit)
            {
                //Something happens with the shark

                 
                currentHits += 1;
                if( currentHits >= minHits)
                {
                    hit.transform.GetComponent<Shark>().Stabbed();
                    hit.transform.GetComponent<Shark>().Sound();
                    currentHits = 0;
                }

                bloodInst.transform.position = hit.point;                
                StartCoroutine(Bleed());

                //audioSource.PlayOneShot(splashSound);
                spearHealth--;

                audioSource.PlayOneShot(spearDegredation);
                RefreshUI();
                //StartCoroutine(BreakDelay());
                nothingHit = false;
            }

            PlayerPrefs.SetInt("SpearHealth", spearHealth);            

            if (spearHealth == 0)
            {                
                
                PlayerPrefs.SetInt("HasSpear", 0);
                clickToStab = false;

                audioSource.PlayOneShot(spearDegredation);
                RefreshUI();
                StartCoroutine(lastBreakDelay());
            }

            strikeContact = true;
            hitPos = hit.point;
        }
        else if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, stabRange, waterMask)&& nothingHit)
        {
            if (!strikeContact)
            {
                strikeContact = true;


                if (hit.transform.tag == "Water")
                {
                    StartCoroutine(Splash());
                    audioSource.PlayOneShot(splashSound);

                }
                hitPos = hit.point;
            }            
        }        

    }

    IEnumerator showFish()
    {
        targetFish.SetActive(true);
        yield return new WaitForSeconds(1.1f);
        targetFish.SetActive(false);
    }

    //Effects (with delays)
    IEnumerator Splash()
    {

        GameObject newSplash = Instantiate(splash, hit.point, Quaternion.identity);
        GameObject newRipple = Instantiate(ripple, hit.point, Quaternion.Euler(90, 0, 0));
        yield return new WaitForSeconds(2f); //Lasts 2 seconds
        Destroy(newSplash);
        Destroy(newRipple);
    }
    IEnumerator Bleed()
    {
        bloodInst.SetActive(true);
        yield return new WaitForSeconds(0.8f); //Lasts 0.8 seconds
        bloodInst.SetActive(false);
    }
    IEnumerator BreakDelay() //Spear breaks after short delay
    {
              
        yield return new WaitForSeconds(0.5f);
        audioSource.PlayOneShot(spearDegredation);
        targetFish.SetActive(false);
        RefreshUI();
    }

    IEnumerator CheckPlanksDelay()
    {
        yield return new WaitForSeconds(0.5f);
        planksRemaning = player.GetComponent<Pick>().LogsLeft();
    }

    public void HideSpears()
    {
        for (int i = 0; i < spearPrefab.Length; i++)
        {
            spear[i].SetActive(false);
        }
    }
    public void RefreshUI()
    {
        planksRemaning = player.GetComponent<Pick>().LogsLeft();
        
            for (int i = 0; i < spearPrefab.Length-1; i++)
            {
                spear[i].SetActive(false);
            }
            for(int i = 0;i < fish.Length; i++)
            {
                fish[i].SetActive(false);
            }
        
        
        switch (spearHealth)
        {
            case 0:
                if (spear != null)
                {
                    spearUI.texture = transparent;                    
                }
                break;
            case 1:
                
                spearUI.texture = spear5;
                spear[4].SetActive(true);
                activeSpear = 4;
                break;
            case 2:
                spearUI.texture = spear4;
                //spear = spearPrefab[1];
                spear[3].SetActive(true);
                activeSpear = 3;
                break;
            case 3:
                spearUI.texture = spear3;
                //spear = spearPrefab[2];
                spear[2].SetActive(true);
                activeSpear = 2;                
                break;
            case 4:
                spearUI.texture = spear2;
                //spear = spearPrefab[3];
                spear[1].SetActive(true);
                activeSpear = 1;                
                break;
            case 5:
                spearUI.texture = spear1;
                //spear = spearPrefab[4];
                spear[0].SetActive(true);
                activeSpear = 0;                
                break;
        }
    }

    IEnumerator lastBreakDelay()
    {
        yield return new WaitForSeconds(0.8f);
        for (int i = 0; i < spearPrefab.Length; i++)
        {
            spear[i].SetActive(false);
        }
    }
}