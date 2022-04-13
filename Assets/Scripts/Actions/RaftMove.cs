using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RaftMove : MonoBehaviour
{

    public float windLevel; //Depending how much wind there is, the raft moves faster
    public float arrivalDist; //Distance to marker to qualify arrival
    public float moveToNextTimer;

    private float reached; //Egnore already visited priorities

    private Vector3 nextPos;

    private GameObject[] markers;
    private List<GameObject> markerList;

    private float time;

    void Start()
    {
        markers = GameObject.FindGameObjectsWithTag("MoveMarker");
        markerList = new List<GameObject>(markers);
        markerList.Sort(SortPriority); //Sort the markers by priority       

        reached = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Every 8 seconds look for new markers
        time += Time.deltaTime;
        if(time > moveToNextTimer)
        {
            time = 0;
            markers = GameObject.FindGameObjectsWithTag("MoveMarker"); //Expensive, should optimise
            markerList = new List<GameObject>();

            foreach(GameObject marker in markers){
                if (marker.GetComponent<MoveMarker>().priority > reached)
                {
                    markerList.Add(marker);
                }
            }

            markerList.Sort(SortPriority); //Sort the markers by priority
            if(markerList.Count > 0)
            {
                nextPos = markerList[0].GetComponent<MoveMarker>().pos;
               // Debug.Log("next marker = " + markerList[0].name);
            }            
        }
        if(markerList.Count > 0)
        {
            if (Vector3.Distance(transform.position, nextPos) < arrivalDist)
            {
                reached = markerList[0].GetComponent<MoveMarker>().priority;
            }
            transform.position = Vector3.MoveTowards(transform.position, nextPos, windLevel);
        }        
    }

    private int SortPriority(GameObject a, GameObject b)
    {
        return a.GetComponent<MoveMarker>().priority.CompareTo(b.GetComponent<MoveMarker>().priority);
    }
}
