using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    public bool thrown;
    public float speed;

    private Rigidbody rb;
    private GameObject throwSource;
    // Start is called before the first frame update
    void Start()
    {
        thrown = false;
        rb = gameObject.GetComponent<Rigidbody>();
        throwSource = GameObject.FindGameObjectWithTag("Player"); //Player is the throw source
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (thrown)
        {
            //transform.Translate(Vector3.forward * speed);

            GetComponent<BoxCollider>().enabled = true;
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddForce(throwSource.transform.forward * speed);
        }
    }

    public void Throw(float strength)
    {
        //gameObject.transform.parent = null;
        GetComponent<BoxCollider>().enabled = true;
        speed = strength;
        thrown = true;
    }
}
