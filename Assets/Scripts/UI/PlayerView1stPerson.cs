using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView1stPerson : MonoBehaviour
{
    //UI Objects
    // public GameObject fishingLabel;

    //Player variables
    public float moveSpeed;
    public float lookSpeed;
    public Camera mainCam;

    private float mouseX;
    private float mouseY;

    //Script variables
    private RaycastHit hit;
    private Rigidbody rb;

    void Start()
    {

        // rb = gameObject.GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;

        mainCam = Camera.main;
        mainCam.transform.parent = gameObject.transform;
        mainCam.transform.localPosition = new Vector3(0, 0, 0);
        mainCam.transform.localRotation = Quaternion.Euler(0, 0, 0);

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

        if ((mouseX - Input.GetAxis("Mouse Y") * lookSpeed) < 50 && //Limit view
            (mouseX - Input.GetAxis("Mouse Y") * lookSpeed) > - 50)
        {
            
            mouseX -= Input.GetAxis("Mouse Y") * lookSpeed;
        }        

        gameObject.transform.rotation = Quaternion.Euler(0, mouseY, 0);
        mainCam.transform.rotation = Quaternion.Euler(mouseX, mouseY, 0);

        if (Input.GetKey("up") || Input.GetKey("w"))
        {
            //rb.MovePosition(transform.position + (m_Input * Time.deltaTime * moveSpeed));
            gameObject.transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
        }
        if (Input.GetKey("down") || Input.GetKey("s"))
        {
            gameObject.transform.Translate(Vector3.back * Time.deltaTime * moveSpeed);
        }
        if (Input.GetKey("left") || Input.GetKey("a"))
        {
            gameObject.transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
        }
        if (Input.GetKey("right") || Input.GetKey("d"))
        {
            gameObject.transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
        }

        if (Input.GetKey("p"))
        {
            gameObject.GetComponent<Pick>().PickLog();
        }


        // if raycast hits, it checks if it hit an object with the tag Player
        /*if (Physics.Raycast(mainCam.transform.position, transform.forward, out hit, 30) &&
                    hit.collider.gameObject.CompareTag("Fish"))
        {
            fishingLabel.SetActive(true);
            if (Input.GetKey("f"))
            {
                Debug.Log("Started fishing");
            }
        }
        else
        {
            fishingLabel.SetActive(false);
        }*/
    }
}
