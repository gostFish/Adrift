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

    //Throwing / stabbing

    public float despawnDist;

    public int stabRange;
    public int maxSpearHits;

    public float fishHungerUp;
    private float hunger;

    private bool holding;
    public bool clickToStab;

    private int fishMask;
    private int waterMask;
    private int raftMask;

    private float stabAnimTime;
    private bool stabbing;

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

    public GameObject spearPrefab;
    public GameObject fishPrefab;
    public GameObject spear; //As an instance
    private GameObject fish;

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

        stabAnimTime = 3;
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
                spear.SetActive(false);
                Debug.Log("Spear should not be active");
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

        if (stabAnimTime > 1.1 && stabAnimTime < 1.2f) //Time for strike to count
        {
            Strike();
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

            }
            else
            {
                crosshair.texture = greyCH;
                player.GetComponent<PlayerManager>().canPickLog = false;
                player.GetComponent<PlayerManager>().lookingAtLog = false;
            }
        }

        //Animate Stabbing
        if (stabbing == true)
        {

            if (stabAnimTime < 1) //draw back
            {
                stabAnimTime += Time.deltaTime / 0.3f;
                spear.transform.localPosition = Vector3.Lerp(holdPos, stabDrawBackPos, stabAnimTime);
                //fish.transform.localPosition = spear.transform.localPosition + new Vector3(-0.1f,-0.1f,0.7f);
                spear.transform.localRotation = Quaternion.Lerp(holdAngle, drawAngle, stabAnimTime);
            }
            else if (stabAnimTime <= 2 && hit.point != null) //stab
            {
                stabAnimTime += Time.deltaTime / 0.2f;
                spear.transform.localPosition = Vector3.Lerp(stabDrawBackPos, stabPoint, stabAnimTime - 1f);
                fish.transform.localPosition = fishStabbed;
                spear.transform.localRotation = Quaternion.Lerp(drawAngle, stabAngle, stabAnimTime-1f);
            }
            else if (stabAnimTime <= 3) //Return to origin
            {
                stabAnimTime += Time.deltaTime / 0.5f;
                spear.transform.localPosition = Vector3.Lerp(stabPoint, holdPos, stabAnimTime - 2f);
                fish.transform.localPosition = Vector3.Lerp(fishStabbed, fishBack , stabAnimTime - 2f);
                spear.transform.localRotation = Quaternion.Lerp(stabAngle, holdAngle, stabAnimTime-2f);
            }
            else
            {
                stabbing = false;
            }
        }else if (spear != null) //reposition spear if exists
        {
            spear.transform.localPosition = holdPos;
            spear.transform.rotation = mainCam.transform.rotation * Quaternion.Euler(15, 15, 0);
            spear.transform.rotation = mainCam.transform.rotation * Quaternion.Euler(0, 90, 10);

            if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, stabRange, fishMask))
            {
                if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, stabRange, raftMask))
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
                }
                else if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, stabRange, fishMask))
                {
                    if (PlayerPrefs.GetInt("HasSpear") == 1)
                    {
                        if (hit.transform.tag == "Shark")
                        {
                            crosshair.texture = redCH;
                        }
                        else if (hit.transform.tag == "Fish")
                        {
                            crosshair.texture = greenCH;
                        }
                    }
                }
            }            
            else
            {
                crosshair.texture = greyCH;
                player.GetComponent<PlayerManager>().canPickLog = false;
                player.GetComponent<PlayerManager>().lookingAtLog = false;
            }
        }        
    }

    void Update()
    {
        
        if (Input.GetMouseButtonDown(0) && !stabbing && clickToStab) //stab animation
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
        spear = Instantiate(spearPrefab);

        //spear.transform.parent = player.transform;
        spear.transform.parent = mainCam.transform;
        spear.transform.localPosition = holdPos;

        spear.GetComponent<Rigidbody>().isKinematic = true;
        spear.GetComponent<Rigidbody>().useGravity = false;
        spear.GetComponent<BoxCollider>().enabled = false;

        spear.SetActive(false);
    }

    public void GetFish()
    {
        fish = Instantiate(fishPrefab);
        fish.GetComponent<FishMovement>().enabled = false;
        fish.GetComponent<BoxCollider>().enabled = false;
        fish.transform.parent = mainCam.transform;
        fish.transform.localPosition = new Vector3(0.35f, -0.1f, 1.4f);
        fish.transform.localRotation = Quaternion.Euler(-50f, -50f, -150);
        fish.SetActive(false);
    }
    //Update Spear UI
    public void RefreshSpear()
    {
        holding = true;

        PlayerPrefs.SetInt("SpearHealth", maxSpearHits);
        spearHealth = maxSpearHits;
        spear.SetActive(true);

        RefreshUI();
    }

    //Hit something
    private void Strike()
    {      
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, stabRange, fishMask))
        {
            if (hit.transform.tag == "Fish")
            {
                hunger = PlayerPrefs.GetFloat("Hunger");
                hunger += fishHungerUp;

                if (hunger > 100)
                {
                    hunger = 100f;
                }
                PlayerPrefs.SetFloat("Hunger", hunger);

                hit.transform.gameObject.active = false;
                audioSource.PlayOneShot(splashSound);
                if (spearHealth > 1)
                {
                    fish.SetActive(true);
                }
            }
            else if (hit.transform.tag == "Shark")
            {
                //Something happens with the shark
                bloodInst.transform.position = hit.point;
                hit.transform.GetComponent<Shark>().Stabbed();
                StartCoroutine(Bleed());
                audioSource.PlayOneShot(splashSound);
            }
            spearHealth--;
            PlayerPrefs.SetInt("SpearHealth", spearHealth);
            

            if (spearHealth == 0)
            {
                spear.SetActive(false);
                PlayerPrefs.SetInt("HasSpear", 0);
                clickToStab = false;
            }

            StartCoroutine(BreakDelay());
        }
        else if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, stabRange, waterMask))
        {
            if (hit.transform.tag == "Water")
            {
                //splashInst.transform.position = hit.point;
                //rippleInst.transform.position = hit.point;
                //rippleInst.transform.rotation = Quaternion.Euler(90,0,0);
                
                StartCoroutine(Splash());
                audioSource.PlayOneShot(splashSound);
            }
        }
    }

    //Effects (with delays)
    IEnumerator Splash()
    {
        //splashInst.SetActive(true);
        //rippleInst.SetActive(true);
        GameObject newSplash = Instantiate(splash, hit.point, Quaternion.identity);
        GameObject newRipple = Instantiate(ripple, hit.point, Quaternion.Euler(90, 0, 0));
        yield return new WaitForSeconds(2f); //Lasts 2 seconds
        Destroy(newSplash);
        Destroy(newRipple);
        //splashInst.SetActive(false);
        //rippleInst.SetActive(false);
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
        fish.SetActive(false);
        RefreshUI();
    }

    private void RefreshUI()
    {
        switch (spearHealth)
        {
            case 0:
                if (spear != null)
                {
                    spearUI.texture = transparent;
                    spear.SetActive(false);
                }
                break;
            case 1:
                spearUI.texture = spear5;
                break;
            case 2:
                spearUI.texture = spear4;
                break;
            case 3:
                spearUI.texture = spear3;
                break;
            case 4:
                spearUI.texture = spear2;
                break;
            case 5:
                spearUI.texture = spear1;
                break;
        }
    }
}