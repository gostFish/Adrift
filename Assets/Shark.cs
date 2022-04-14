using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : MonoBehaviour
{
     //Variable values
    public float speed;
    public int radius;
    public float depth;

    //Script variables
    private GameObject raft;
    private Vector3 pos;
    private float time;
    private float time_depth = 0f;


    void Start()
    {
        raft = GameObject.FindGameObjectWithTag("Raft");

        radius = 8;
        speed = 0.1f;
        depth = 5f;
    }

    void FixedUpdate()
    {
        pos = transform.position;
        time += Time.deltaTime;

        //do
        //{
            if (time < 17)
            {
                time_depth += 0.00001f;
                depth = depth - time_depth;
            }
            else if (time > 32)
            {
                time_depth += 0.00001f;
                depth = depth + time_depth;
            }
        //} while (depth != 5.01f);
      

        pos.y = raft.transform.position.y - depth;

        pos.x = raft.transform.position.x + (radius * (Mathf.Cos(time * speed)));
        pos.z = raft.transform.position.z + (radius * (Mathf.Sin(time * speed)));
        gameObject.transform.position = pos;
    }
}
