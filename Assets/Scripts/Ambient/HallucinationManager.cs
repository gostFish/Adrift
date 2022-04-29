using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallucinationManager : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject instanceManager;
    public GameObject[] hallucinations;
    public GameObject raft;

    private float time;

    public float checkTimer;
    public float showDist;

    void Start()
    {
        //hallucinations = GameObject.FindGameObjectsWithTag("Hallucination"); //Doesnt work if they are inactive
        raft = GameObject.FindGameObjectWithTag("Raft");
        instanceManager = GameObject.FindGameObjectWithTag("InstanceManager");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time += Time.deltaTime;
        if(time > checkTimer)
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
    }
}
