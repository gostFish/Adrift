using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView1stPerson : MonoBehaviour
{
    //UI Objects
    public GameObject journal;
    private bool journalOpen;

    //Player variables
    public float moveSpeed;
    public float lookSpeed;
    public Camera mainCam;

    private float mouseX;
    private float mouseY;

    //Script variables

    private RaycastHit hit;
    private int raftMask;

    public GameObject forwardStop;
    public GameObject backStop;
    public GameObject leftStop;
    public GameObject rightStop;

    private Vector3 from;
    private Vector3 to;

    void Start()
    {

        // rb = gameObject.GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;

        mainCam = Camera.main;
        mainCam.transform.parent = gameObject.transform;
        mainCam.transform.localPosition = new Vector3(0, 0, 0);
        mainCam.transform.localRotation = Quaternion.Euler(0, 0, 0);

        raftMask = LayerMask.GetMask("Raft");

        StartCoroutine(CorrectView());

        //Get saved MoveSpeed
        if (PlayerPrefs.HasKey("MoveSpeed"))
        {
            moveSpeed = PlayerPrefs.GetFloat("MoveSpeed");
        }
        else
        {
            PlayerPrefs.SetFloat("MoveSpeed", 50); //Default value
            moveSpeed = 50f;
        }

        //Get saved LookSpeed
        if (PlayerPrefs.HasKey("LookSpeed"))
        {
            lookSpeed = PlayerPrefs.GetFloat("LookSpeed");
        }
        else
        {
            PlayerPrefs.SetFloat("LookSpeed", 50); //Default value
            lookSpeed = 50f;
        }
    }

    IEnumerator CorrectView()
    {
        yield return new WaitForSeconds(1); //wait one second before correcting view
        transform.rotation = Quaternion.Euler(0, 100, 0); //Default looking location
    }

    void FixedUpdate()
    {
        
       // Vector3 m_Input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //Debug.Log("m_Input = " + m_Input);
        //rb.MovePosition(transform.position + m_Input * moveSpeed * Time.deltaTime);

        Cursor.lockState = CursorLockMode.Locked;

        mouseY += Input.GetAxis("Mouse X") * lookSpeed;

        if ((mouseX - Input.GetAxis("Mouse Y") * lookSpeed) < 70 && //Limit view
            (mouseX - Input.GetAxis("Mouse Y") * lookSpeed) > - 50)
        {
            
            mouseX -= Input.GetAxis("Mouse Y") * lookSpeed;
        }        

        gameObject.transform.rotation = Quaternion.Euler(0, mouseY, 0);
        mainCam.transform.rotation = Quaternion.Euler(mouseX, mouseY, 0);

        if (Input.GetKey("up") || Input.GetKey("w"))
        {            
            from = (forwardStop.transform.position) + new Vector3(0,1,0);
            to = ((forwardStop.transform.position) + new Vector3(0, -1, 0)) - from;
            
            if(Physics.Raycast(from, to, out hit, Vector3.Distance(from, to), raftMask)
            && hit.transform.tag == "Raft")
            {
                gameObject.transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
            }
            
        }
        if (Input.GetKey("down") || Input.GetKey("s"))
        {
            from = (forwardStop.transform.position) + new Vector3(0, 1, 0);
            to = ((forwardStop.transform.position) + new Vector3(0, -1, 0)) - from;

            if (Physics.Raycast(from, to, out hit, Vector3.Distance(from, to), raftMask)
            && hit.transform.tag == "Raft")
            {
                gameObject.transform.Translate(Vector3.back * Time.deltaTime * moveSpeed);
            }
            
        }
        if (Input.GetKey("left") || Input.GetKey("a"))
        {
            from = (forwardStop.transform.position) + new Vector3(0, 1, 0);
            to = ((forwardStop.transform.position) + new Vector3(0, -1, 0)) - from;

            if (Physics.Raycast(from, to, out hit, Vector3.Distance(from, to), raftMask)
            && hit.transform.tag == "Raft")
            {
                gameObject.transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
            }            
        }
        if (Input.GetKey("right") || Input.GetKey("d"))
        {
            from = (forwardStop.transform.position) + new Vector3(0, 1, 0);
            to = ((forwardStop.transform.position) + new Vector3(0, -1, 0)) - from;

            if (Physics.Raycast(from, to, out hit, Vector3.Distance(from, to), raftMask)
            && hit.transform.tag == "Raft")
            {
                gameObject.transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
            }            
        }

        if (Input.GetKey("p"))
        {
            gameObject.GetComponent<Pick>().PickLog();
        }

        if (Input.GetKey("j"))
        {
            Debug.Log("Journal opened");
            if (journalOpen)
            {
                journal.SetActive(false);
                journalOpen = false;
            }
            else
            {
                journal.SetActive(true);
                journalOpen = true;
            }
        }
    }
}
