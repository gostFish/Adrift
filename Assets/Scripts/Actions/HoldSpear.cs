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

    private Vector3 holdPos;
    private Vector3 stabDrawBackPos;
    private Vector3 stabPoint;

    //Throwing / stabbing

    public float despawnDist;

    public int stabRange;
    public int maxSpearHits;

    public float fishHungerUp;
    private float hunger;

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

    public GameObject spearPrefab;
    public GameObject spear; //As an instance

    //Spear UI
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

    

    void Start()
    {
        //Temporary for testing (will always start with spear) - when removed, game will remember player states
        PlayerPrefs.SetInt("HasSpear", 0);
        PlayerPrefs.SetFloat("Hunger", 100);

        //Find Essential things
        mainCam = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player");

        //Layers for raycasts
        fishMask = LayerMask.GetMask("Interactable");
        waterMask = LayerMask.GetMask("Water");
        raftMask = LayerMask.GetMask("Raft");

        //Animation positions

        holdPos = new Vector3(0.4f, 0.1f, 0.4f);  
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
    }


    void FixedUpdate()
    {
        //Time for stab effect to occur
        if (stabAnimTime > 1.1 && stabAnimTime < 1.2f) 
        {
            Strike();
        }

        if (spear != null) //reposition spear if exists
        {
            //Keep spear in position
            spear.transform.localPosition = holdPos;
            spear.transform.rotation = mainCam.transform.rotation * Quaternion.Euler(15, 15, 0);
            spear.transform.rotation = mainCam.transform.rotation * Quaternion.Euler(0, 90, 10);

            //Change crosshair
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

        //Animate stabbing
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
        //Check if stabbing
        if (Input.GetMouseButtonDown(0) && !stabbing && clickToStab) //stab animation
        {
            if (PlayerPrefs.GetInt("HasSpear") == 1)//Does not have a spear
            {
                stabbing = true;
                stabAnimTime = 0;
            }
        }
    }

    //Make spear available
    public void GetSpear() 
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

    //Reset spear stats
    public void RefreshSpear()
    {
        PlayerPrefs.SetInt("SpearHealth", maxSpearHits);
        spearHealth = maxSpearHits;
        spear.SetActive(true);

        RefreshUI();
    }

    private void Strike()
    {  
        //Hit something interractable
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

                PlayerPrefs.SetFloat("Hunger", hunger);
            }            

            spearHealth--;
            PlayerPrefs.SetInt("SpearHealth", spearHealth);
            
            
            //Spear now out of health, destroy
            if (spearHealth == 0)
            {
                spear.SetActive(false);
                PlayerPrefs.SetInt("HasSpear", 0);
                clickToStab = false;
            }

            RefreshUI();
        }
        //Hit water
        else if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, stabRange, waterMask))
        {
            if (hit.transform.tag == "Water")
            {
                Instantiate(splash, hit.point, Quaternion.identity);
            }
        }
    }

    //Update UI in game
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