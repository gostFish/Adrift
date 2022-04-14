using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView1stPerson : MonoBehaviour
{

    //Player variables
    public float moveSpeed;
    public float lookSpeed;
    public Camera mainCam;

    private float mouseX;
    private float mouseY;

    //Other variables

    private GameObject raft;

    //Script variables

    private RaycastHit hit;
    private int raftMask;

    public GameObject forwardStop;
    public GameObject backStop;
    public GameObject leftStop;
    public GameObject rightStop;

    private Vector3 from;
    private Vector3 to;

    private Vector3 playerPos;

    private bool moveUp;
    private bool moveBack;
    private bool moveLeft;
    private bool moveRight;

    public bool lookingAtLog;
    public bool canPickLog;

    void Start()
    {

        // rb = gameObject.GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;

        mainCam = Camera.main;
        mainCam.transform.parent = gameObject.transform;
        mainCam.transform.localPosition = new Vector3(0, 0, 0);
        mainCam.transform.localRotation = Quaternion.Euler(0, 0, 0);

        raftMask = LayerMask.GetMask("Raft");
        raft = GameObject.FindGameObjectWithTag("Raft");
        StartCoroutine(CorrectView());

        //Get saved MoveSpeed
        if (PlayerPrefs.HasKey("MoveSpeed"))
        {
            moveSpeed = PlayerPrefs.GetFloat("MoveSpeed");
        }
        else
        {
            PlayerPrefs.SetFloat("MoveSpeed", 1); //Default value
            moveSpeed = 1f;
        }

        //Get saved LookSpeed
        if (PlayerPrefs.HasKey("LookSpeed"))
        {
            lookSpeed = PlayerPrefs.GetFloat("LookSpeed");
        }
        else
        {
            PlayerPrefs.SetFloat("LookSpeed", 10); //Default value
            lookSpeed = 10f;
        }
    }

    IEnumerator CorrectView()
    {
        yield return new WaitForSeconds(1); //wait one second before correcting view
        transform.rotation = Quaternion.Euler(0, 100, 0); //Default looking location
    }

    void FixedUpdate() //Physics related processes
    {
        transform.localPosition = new Vector3(transform.localPosition.x, raft.transform.position.y, transform.localPosition.z);

        if (moveUp)
        {
            from = (forwardStop.transform.position) + new Vector3(0, 1, 0);
            to = ((forwardStop.transform.position) + new Vector3(0, -1, 0)) - from;

            if (Physics.Raycast(from, to, out hit, Vector3.Distance(from, to), raftMask)
            && hit.transform.tag == "Raft")
            {
                gameObject.transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
            }
        }

        if (moveBack)
        {
            from = (backStop.transform.position) + new Vector3(0, 1, 0);
            to = ((backStop.transform.position) + new Vector3(0, -1, 0)) - from;

            if (Physics.Raycast(from, to, out hit, Vector3.Distance(from, to), raftMask)
            && hit.transform.tag == "Raft")
            {
                gameObject.transform.Translate(Vector3.back * Time.deltaTime * moveSpeed);
            }
        }

        if (moveLeft)
        {
            from = (leftStop.transform.position) + new Vector3(0, 1, 0);
            to = ((leftStop.transform.position) + new Vector3(0, -1, 0)) - from;

            if (Physics.Raycast(from, to, out hit, Vector3.Distance(from, to), raftMask)
            && hit.transform.tag == "Raft")
            {
                gameObject.transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
            }
        }

        if (moveRight)
        {
            from = (rightStop.transform.position) + new Vector3(0, 1, 0);
            to = ((rightStop.transform.position) + new Vector3(0, -1, 0)) - from;

            if (Physics.Raycast(from, to, out hit, Vector3.Distance(from, to), raftMask)
            && hit.transform.tag == "Raft")
            {
                gameObject.transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
            }
        }

    }

    void Update() //Non pyhsics related processes
    {
        Cursor.lockState = CursorLockMode.Locked;

        mouseY += Input.GetAxis("Mouse X") * lookSpeed;
        if ((mouseX - Input.GetAxis("Mouse Y") * lookSpeed) < 70 && //Limit view
            (mouseX - Input.GetAxis("Mouse Y") * lookSpeed) > -50)
        {
            mouseX -= Input.GetAxis("Mouse Y") * lookSpeed;
        }
        gameObject.transform.rotation = Quaternion.Euler(0, mouseY, 0);
        mainCam.transform.rotation = Quaternion.Euler(mouseX, mouseY, 0);


        if (Input.GetKey("up") || Input.GetKey("w"))
        {
            moveUp = true;
        }
        else
        {
            moveUp = false;
        }

        if (Input.GetKey("down") || Input.GetKey("s"))
        {
            moveBack = true;
        }
        else
        {
            moveBack = false;
        }

        if (Input.GetKey("left") || Input.GetKey("a"))
        {
            moveLeft = true;
        }
        else
        {
            moveLeft = false;
        }

        if (Input.GetKey("right") || Input.GetKey("d"))
        {
            moveRight = true;
        }
        else
        {
            moveRight = false;
        }

        if (Input.GetMouseButtonDown(0) && canPickLog) //Was previously "p"
        {
            gameObject.GetComponent<Pick>().PickLog();
            StartCoroutine(PickDelay());
        }
    }

    IEnumerator PickDelay()
    {
        yield return new WaitForSeconds(1);
        gameObject.GetComponent<HoldSpear>().clickToStab = true;

    }
}
