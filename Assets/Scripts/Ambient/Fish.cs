using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    //Variable values
    public float speed;
    public int radius;
    public float depth;

    //Script variables
    private GameObject raft;
    private Vector3 pos;
    private float time;


    void Start()
    {
        raft = GameObject.FindGameObjectWithTag("Raft");

        radius = 3;
        speed = 0.1f;
        depth = 0.5f;
    }

    void FixedUpdate()
    {
        pos = transform.position;
        time += Time.deltaTime;

        pos.y = raft.transform.position.y - depth;

        pos.x = raft.transform.position.x + (radius * (Mathf.Cos(time * speed)));
        pos.z = raft.transform.position.z + (radius * (Mathf.Sin(time * speed)));
        gameObject.transform.position = pos;
    }
}
