using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RaftMove : MonoBehaviour
{
    //Varaibles
    private float time;
    private int currentMarker;

    public float raftSpeed; //Depending how much wind there is, the raft moves faster
    public float arrivalDist; //Distance to marker to qualify arrival
    public float moveToNextTimer;

    private Vector3 nextPos;
    

    //Game Objects
    public GameObject raft;

    private GameObject[] markers;
    private List<GameObject> markerList;
    

    void Start()
    {
        markers = GameObject.FindGameObjectsWithTag("MoveMarker");
        markerList = new List<GameObject>(markers);        
                                       
        foreach (GameObject marker in markers)
        {            
            markerList.Add(marker);
            
        }

        markerList.Sort(SortPriority); //Sort the markers by priority
        
        currentMarker = 0;
    }


    void FixedUpdate()
    {
        //Every 8 seconds look for new markers
        time += Time.deltaTime;
        if(time > moveToNextTimer)
        {
            time = 0;
          
            if(currentMarker < markerList.Count)
            {
                nextPos = markerList[currentMarker].GetComponent<MoveMarker>().pos;
            }            
        }
        if(currentMarker < markerList.Count)
        {
            if (Vector3.Distance(transform.position, nextPos) < arrivalDist)
            {
                currentMarker++;
            }
            transform.position = Vector3.MoveTowards(transform.position, nextPos, raftSpeed);
            raft.transform.LookAt(nextPos);
            
        }        
    }

    //Simple Unity sorting algorithm
    private int SortPriority(GameObject a, GameObject b)
    {
        return a.GetComponent<MoveMarker>().priority.CompareTo(b.GetComponent<MoveMarker>().priority);
    }
}
