using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAndThrow : MonoBehaviour
{
    //Variables
    public int hasSpear;
    public float despawnDist;
    public float throwForce;

    private bool holding;

    public Vector3 holdPos;

    //Game Objects

    private GameObject raft;
    private GameObject player;

    public GameObject spearPrefab;
    private GameObject spear; //Refers to instance in script

    private Camera mainCam;

    
    void Start()
    {
        
        PlayerPrefs.SetInt("HasSpear", 1);//Temporary for testing

        //Find Essential things
        mainCam = Camera.main;
        raft = GameObject.FindGameObjectWithTag("Raft");
        player = GameObject.FindGameObjectWithTag("Player");

        if (PlayerPrefs.HasKey("HasSpear"))
        {
            hasSpear = PlayerPrefs.GetInt("HasSpear"); //If 0, no spear, if 1 there is a spear
            
        }
        else
        {
            PlayerPrefs.SetInt("HasSpear",0); //Default, no spear
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

    //if (Input.GetMouseButtonDown(0)) {
        if (Input.GetKey("space"))
        {
            if (PlayerPrefs.GetInt("HasSpear") == 0)//Does not have a spear
            {
                Pick();//Needs more information                
            }
            else //Has a spear
            {
                Throw();
                holding = false;
                PlayerPrefs.SetInt("HasSpear", 0);
            }
        }
        
        if(spear != null)
        {
            if (Vector3.Distance(spear.transform.position, player.transform.position) > despawnDist)
            {
                Destroy(spear);
            }
            else if(holding)
            {
                //Debug.Log(Time.deltaTime + " Spear pos was at " + spear.transform.position);
                //Debug.Log(Time.deltaTime + " Spear local pos was at " + spear.transform.localPosition);
                spear.transform.localPosition = holdPos;
                //Debug.Log(Time.deltaTime + " Spear pos is at " + spear.transform.position);
                //Debug.Log(Time.deltaTime + " Spear local pos is at " + spear.transform.localPosition);
                spear.transform.rotation = mainCam.transform.rotation * Quaternion.Euler(15, 15, 0);                
            }
        } 
    }

    private void Pick()
    {
        if (raft.GetComponent<Boat>().remainingPlanks > raft.GetComponent<Boat>().minPlanks)
        {
            raft.GetComponent<Boat>().minPlanks += -1;
            PlayerPrefs.SetInt("HasSpear", 1);
            GetSpear();
        }        
    }
    private void GetSpear()
    {
        spear = Instantiate(spearPrefab);
        spear.transform.parent = player.transform;
        spear.transform.localPosition = holdPos;
        spear.transform.rotation = Quaternion.Euler(15, 15, 0);
        spear.GetComponent<Rigidbody>().isKinematic = true;
        spear.GetComponent<Rigidbody>().useGravity = false;
        spear.GetComponent<BoxCollider>().enabled = false;
        holding = true;
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