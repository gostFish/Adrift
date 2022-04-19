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

    //Throwing / stabbing

    public float despawnDist;
    public float throwForce;

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
    private Camera mainCam;

    public GameObject splash;
    public GameObject blood;

    private GameObject splashInst;
    private GameObject bloodInst;

    public GameObject spearPrefab;
    public GameObject spear; //As an instance

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
        

    void Start()
    {
        //Temporary for testing (will always start with spear)
        PlayerPrefs.SetInt("HasSpear", 0);
        PlayerPrefs.SetFloat("Hunger", 100);

        //Find Essential things
        mainCam = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player");

        audioSource = GetComponent<AudioSource>();

        fishMask = LayerMask.GetMask("Interactable"); //Engore everything except fish
        waterMask = LayerMask.GetMask("Water");
        raftMask = LayerMask.GetMask("Raft");

        holdPos = new Vector3(0.4f, 0.1f, 0.4f);  //Position spear to view in 1st person
        stabDrawBackPos = new Vector3(0.4f, 0.25f, -0.5f);
        stabPoint = new Vector3(0.4f, 0, 1f);

        stabAnimTime = 3;
        GetSpear();

        //Remember when last playing
        if (PlayerPrefs.HasKey("HasSpear"))
        {
            hasSpear = PlayerPrefs.GetInt("HasSpear"); //If 0, no spear, if 1 there is a spear
            if (hasSpear == 1)
            {
                RefreshSpear();
                clickToStab = true;
                // Debug.Log("Spear should be active");
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
        bloodInst = Instantiate(blood, new Vector3(0, 0, 0), Quaternion.identity);

        splashInst.transform.parent = instanceManager.transform;
        bloodInst.transform.parent = instanceManager.transform;

        splashInst.SetActive(false);
        bloodInst.SetActive(false);
    }


    void FixedUpdate()
    {
        //Debug.DrawRay(mainCam.transform.position, mainCam.transform.forward * stabRange, Color.green, 2, false);

        if (stabAnimTime > 1.1 && stabAnimTime < 1.2f) //Time for strike to count
        {
            Strike();
        }

        if (spear != null) //reposition spear if exists
        {
            spear.transform.localPosition = holdPos;
            spear.transform.rotation = mainCam.transform.rotation * Quaternion.Euler(15, 15, 0);
            spear.transform.rotation = mainCam.transform.rotation * Quaternion.Euler(0, 90, 10);

            if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, stabRange, fishMask))
            {
                if (PlayerPrefs.GetInt("HasSpear") == 1)
                {
                    if(hit.transform.tag == "Shark")
                    {
                        crosshair.texture = redCH;
                    }
                    else
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
                    player.GetComponent<PlayerView1stPerson>().canPickLog = true;
                }
                else
                {
                    crosshair.texture = greyCH;
                }
                player.GetComponent<PlayerView1stPerson>().lookingAtLog = true;

            }
            else
            {
                crosshair.texture = greyCH;
                player.GetComponent<PlayerView1stPerson>().canPickLog = false;
                player.GetComponent<PlayerView1stPerson>().lookingAtLog = false;
            }
        }

        if (stabbing == true)
        {

            if (stabAnimTime < 1)
            {
                stabAnimTime += Time.deltaTime / 0.4f;
                spear.transform.localPosition = Vector3.Lerp(holdPos, stabDrawBackPos, stabAnimTime);
            }
            else if (stabAnimTime <= 2 && hit.point != null)
            {
                stabAnimTime += Time.deltaTime / 0.2f;
                spear.transform.localPosition = Vector3.Lerp(stabDrawBackPos, stabPoint, stabAnimTime - 1f);
            }
            else if (stabAnimTime <= 3)
            {
                stabAnimTime += Time.deltaTime / 0.5f;
                spear.transform.localPosition = Vector3.Lerp(stabPoint, holdPos, stabAnimTime - 2f);
            }
            else
            {
                stabbing = false;
            }
        }
    }

    void Update()
    {
        if (Input.GetKey("t")) //Throwing spear
        {
            if (PlayerPrefs.GetInt("HasSpear") == 1)//Does not have a spear
            {
                // Throw();
                Debug.Log("Throwing is removed btw");
            }
        }

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

        spear.transform.parent = player.transform;
        spear.transform.parent = mainCam.transform;
        spear.transform.localPosition = holdPos;

        spear.GetComponent<Rigidbody>().isKinematic = true;
        spear.GetComponent<Rigidbody>().useGravity = false;
        spear.GetComponent<BoxCollider>().enabled = false;

        spear.SetActive(false);
    }

    public void RefreshSpear()
    {
        holding = true;

        PlayerPrefs.SetInt("SpearHealth", maxSpearHits);
        spearHealth = maxSpearHits;
        spear.SetActive(true);

        RefreshUI();
    }

    private void Strike()
    {
        //Debug.Log("Stabbing with health: " + spearHealth);        

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
                hit.transform.gameObject.active = false;

            }
            else if (hit.transform.tag == "Shark")
            {
                //Something happens with the shark
                bloodInst.transform.position = hit.point;
                StartCoroutine(Bleed());
            }

            spearHealth--;
            PlayerPrefs.SetInt("SpearHealth", spearHealth);
            PlayerPrefs.SetFloat("Hunger", hunger);
            

            if (spearHealth == 0)
            {
                spear.SetActive(false);
                PlayerPrefs.SetInt("HasSpear", 0);
                clickToStab = false;
            }

            StartCoroutine(BreakDelay());
            //RefreshUI();
        }
        else if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, stabRange, waterMask))
        {
            if (hit.transform.tag == "Water")
            {
                //GameObject newSplash = Instantiate(splash, hit.point, Quaternion.identity);
                splashInst.transform.position = hit.point;
                StartCoroutine(Splash());               
            }
        }
    }

    IEnumerator Splash()
    {
        splashInst.SetActive(true);
        yield return new WaitForSeconds(0.8f); //Lasts 0.8 seconds
        splashInst.SetActive(false);
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
        RefreshUI();
    }

    private void RefreshUI()
    {
        //Debug.Log("Updating with health == " + spearHealth);

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

    private void Throw() //No longer used
    {
        //Debug.Log("Throwing");
        spear.transform.position = player.GetComponent<PlayerView1stPerson>().mainCam.transform.position;
        spear.transform.rotation = player.GetComponent<PlayerView1stPerson>().mainCam.transform.rotation;

        spear.transform.position += gameObject.transform.forward * 1; //Throw starts in fron of player (to avoid collision)
        //spear.transform.Rotate(0,20,0);                                                                      
        spear.GetComponent<Spear>().Throw(throwForce);

        PlayerPrefs.SetInt("HasSpear", 0);
        holding = false;
    }
}