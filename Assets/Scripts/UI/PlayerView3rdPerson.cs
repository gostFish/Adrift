using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView3rdPerson : MonoBehaviour
{
    public float moveSpeed;
    public float mouseSensitivity;
    private Camera mainCam;

    private float mouseX;
    private float mouseY;

    void Start()
    {
        mainCam = Camera.main;
        mainCam.transform.position = new Vector3(8, 7, -8);
        mainCam.transform.parent = gameObject.transform;
        mainCam.transform.rotation = Quaternion.Euler(25, -45, 0);

        moveSpeed = 5f;
        mouseSensitivity = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("up") || Input.GetKey("w"))
        {
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
    }
}
