using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallucinationManager : MonoBehaviour
{
    
    //Game Objects
    public GameObject[] hallucinations;
    public GameObject raft;

    //Game variables
    private float time;
    private int activeCount;

    public bool haveActive;

    public float checkTimer;
    public float showDist;

    void Start()
    {        
        raft = GameObject.FindGameObjectWithTag("Raft");
    }
    
    void FixedUpdate()
    {
        time += Time.deltaTime;
        if(time > checkTimer) //Timer to check distances (Optimised so not every step)
        {
            CheckDistances();
            time = 0;
        }
    }

    private void CheckDistances()
    {
        foreach(GameObject obj in hallucinations)
        {
            if (Vector3.Distance(obj.transform.position, raft.transform.position) < showDist)
            {
                obj.SetActive(true);
            }
            else
            {
                obj.SetActive(false);
            }
        }
        activeCount = 0;
        foreach (GameObject go in hallucinations)
        {
            if (go.activeSelf)
            {
                activeCount++;
            }
        }
        if(activeCount > 0)
        {
            haveActive = true;
        }
        else
        {
            haveActive = false;
        }
    }
}
