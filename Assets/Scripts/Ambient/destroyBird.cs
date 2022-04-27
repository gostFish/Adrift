using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyBird : MonoBehaviour
{
    private GameObject raft;
    public GameObject bird;
    // Update is called once per frame

    void Update()
    {
        raft = GameObject.FindGameObjectWithTag("Raft");
        if(raft.transform.position.x > (bird.transform.position.x + 150))
        {
            Object.Destroy(bird);
        }
    }
}
