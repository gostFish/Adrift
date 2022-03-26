using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldSpear : MonoBehaviour
{
    //Variables
    public int hasSpear;
    public float despawnDist;
    public float throwForce;
    public int stabRange;

    public float fishHungerUp;

    private bool holding;
    private int mask;

    public Vector3 holdPos;

    //Game Objects

    private GameObject player;

    public GameObject spearPrefab;
    private GameObject spear; //Refers to instance in script

    private Camera mainCam;


    void Start()
    {

        PlayerPrefs.SetInt("HasSpear", 0);//Temporary for testing (will always start with spear)

        //Find Essential things
        mainCam = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player");

        mask = LayerMask.GetMask("Interactable");
        holdPos = new Vector3(0.4f, 0.1f, 0.4f);

        if (PlayerPrefs.HasKey("HasSpear"))
        {
            hasSpear = PlayerPrefs.GetInt("HasSpear"); //If 0, no spear, if 1 there is a spear            
        }
        else
        {
            PlayerPrefs.SetInt("HasSpear", 0); //Default, no spear
        }
        if (PlayerPrefs.GetInt("HasSpear") == 1)
        {
            GetSpear();
        }
        //Temporary default values
        throwForce = 50f;
        despawnDist = 50;
        holdPos = new Vector3(0.4f, 0.1f, 0.4f);

    }



    void FixedUpdate()
    {
        Debug.DrawRay(mainCam.transform.position, mainCam.transform.forward * stabRange, Color.green, 2, false);

        //if (Input.GetMouseButtonDown(0)) {
        if (Input.GetKey("t")) //Throw
        {
            if (PlayerPrefs.GetInt("HasSpear") == 1)//Does not have a spear
            {
                // Throw();
                Debug.Log("Throwing is removed btw");

            }
        }

        if (Input.GetKey("space")) //stab
        {
            if (PlayerPrefs.GetInt("HasSpear") == 1)//Does not have a spear
            {
                Stab();
            }
        }

        if (spear != null)
        {
            if (Vector3.Distance(spear.transform.position, player.transform.position) > despawnDist)
            {
                Destroy(spear);
            }
            else if (holding)
            {
                spear.transform.localPosition = holdPos;
                spear.transform.rotation = mainCam.transform.rotation * Quaternion.Euler(15, 15, 0);
                spear.transform.rotation = mainCam.transform.rotation * Quaternion.Euler(0, 90, 10);
            }
        }
    }

    public void GetSpear()
    {
        PlayerPrefs.SetInt("HasSpear", 1);
        spear = Instantiate(spearPrefab);
        spear.transform.parent = player.transform;
        spear.transform.parent = mainCam.transform;
        spear.transform.localPosition = holdPos;
        //spear.transform.rotation = Quaternion.Euler(15, 15, 0);
        //spear.transform.rotation = Quaternion.Euler(0, 90, 10);
        spear.GetComponent<Rigidbody>().isKinematic = true;
        spear.GetComponent<Rigidbody>().useGravity = false;
        spear.GetComponent<BoxCollider>().enabled = false;
        holding = true;
    }

    private void Stab()
    {
        Debug.Log("Stabbing");
        RaycastHit hit;

        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, stabRange, mask))
        {

            if (hit.transform.tag == "Fish")
            {                
                float hunger = PlayerPrefs.GetFloat("Hunger");
                hunger += fishHungerUp;
                if(hunger > 100)
                {
                    hunger = 100f;
                }
                PlayerPrefs.SetFloat("Hunger", hunger);
                Destroy(hit.transform.gameObject);
            }
            else
            {
                Debug.Log("struck a " + hit.transform.gameObject.name);
            }
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