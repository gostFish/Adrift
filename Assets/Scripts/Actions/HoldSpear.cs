using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoldSpear : MonoBehaviour
{
    //Variables

    public int hasSpear;

    private RaycastHit hit;
    private int spearHealth;    
    public Vector3 holdPos;

    //Throwing / stabbing

    public float despawnDist;
    public float throwForce;

    public int stabRange;
    public int maxSpearHits;

    public float fishHungerUp;
    private float hunger;

    private bool holding;
    private int fishMask;
    private int waterMask;


    //Game Objects

    private GameObject player;
    private Camera mainCam;

    public GameObject splash;

    public GameObject spearPrefab;
    public GameObject spear; //As an instance

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


    void Start()
    {
        //Temporary for testing (will always start with spear)
        PlayerPrefs.SetInt("HasSpear", 0);
        PlayerPrefs.SetFloat("Hunger", 100);

        //Find Essential things
        mainCam = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player");

        fishMask = LayerMask.GetMask("Interactable"); //Engore everything except fish
        waterMask = LayerMask.GetMask("Water");
        holdPos = new Vector3(0.4f, 0.1f, 0.4f);  //Position spear to view in 1st person
        GetSpear();

        //Remember when last playing
        if (PlayerPrefs.HasKey("HasSpear"))
        {
            hasSpear = PlayerPrefs.GetInt("HasSpear"); //If 0, no spear, if 1 there is a spear
            if (hasSpear == 1)
            {
                RefreshSpear();
                Debug.Log("Spear should be active");
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
            PlayerPrefs.SetFloat("Hunger",100f);
        }
        
        RefreshUI();
    }


    void FixedUpdate()
    {
        Debug.DrawRay(mainCam.transform.position, mainCam.transform.forward * stabRange, Color.green, 2, false);

        if (Input.GetKey("t")) //Throwing spear
        {
            if (PlayerPrefs.GetInt("HasSpear") == 1)//Does not have a spear
            {
                // Throw();
                Debug.Log("Throwing is removed btw");
            }
        }

        //if (Input.GetMouseButtonDown(0)) //stab
        if (Input.GetKey("space")) //stab
        {
            if (PlayerPrefs.GetInt("HasSpear") == 1)//Does not have a spear
            {
                Stab();
            }
        }

        if (spear != null) //reposition spear if exists
        {           
            spear.transform.localPosition = holdPos;
            spear.transform.rotation = mainCam.transform.rotation * Quaternion.Euler(15, 15, 0);
            spear.transform.rotation = mainCam.transform.rotation * Quaternion.Euler(0, 90, 10);

            if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, stabRange, fishMask))
            {
                crosshair.texture = greenCH;
            }
            else
            {
                crosshair.texture = greyCH;
            }//To add shark

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

    private void Stab()
    {
        Debug.Log("Stabbing with health: " + spearHealth);
        

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

                spearHealth--;
                PlayerPrefs.SetInt("SpearHealth", spearHealth);
                PlayerPrefs.SetFloat("Hunger", hunger);
                hit.transform.gameObject.active = false;

                if (spearHealth == 0)
                {
                    spear.SetActive(false);
                    PlayerPrefs.SetInt("HasSpear", 0);
                }

                RefreshUI();
            }
            else
            {
                Debug.Log("struck a " + hit.transform.gameObject.name);
            }
        }
        else if(Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, stabRange, waterMask))
        {
            if (hit.transform.tag == "Water")
            {
                Instantiate(splash, hit.point, Quaternion.identity);
                Debug.Log("Fish not hit, water hit instead");
            }
        }
    }

    private void RefreshUI()
    {        

        Debug.Log("Updating with health == " + spearHealth);

        if (spearHealth == 0)
        {
            if(spear != null)
            {
                spearUI.texture = transparent;
                spear.SetActive(false);
            }            
        }
        else if (spearHealth == 1)
        {
            spearUI.texture = spear5;
        }
        else if (spearHealth == 2)
        {
            spearUI.texture = spear4;
        }
        else if (spearHealth == 3)
        {
            spearUI.texture = spear3;
        }
        else if (spearHealth == 4)
        {
            spearUI.texture = spear2;
        }
        else if (spearHealth == 5)
        {
            spearUI.texture = spear1;
        }
    }

    private void Throw()
    {
        Debug.Log("Throwing");
        spear.transform.position = player.GetComponent<PlayerView1stPerson>().mainCam.transform.position;
        spear.transform.rotation = player.GetComponent<PlayerView1stPerson>().mainCam.transform.rotation;

        spear.transform.position += gameObject.transform.forward * 1; //Throw starts in fron of player (to avoid collision)
        //spear.transform.Rotate(0,20,0);                                                                      
        spear.GetComponent<Spear>().Throw(throwForce);

        PlayerPrefs.SetInt("HasSpear", 0);
        holding = false;
    }
}