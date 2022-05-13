using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class RaftMove : MonoBehaviour
{

    public float daySpeed; //Depending how much wind there is, the raft moves faster
    public float nightSpeed;
    public bool isDay;
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
    void FixedUpdate()
    {
        //Every 8 seconds look for new markers
        time += Time.deltaTime;
        if (time > moveToNextTimer)
        {
            time = 0;
            markers = GameObject.FindGameObjectsWithTag("MoveMarker"); //Expensive, should optimise
            markerList = new List<GameObject>();

            foreach (GameObject marker in markers)
            {
                if (marker.GetComponent<MoveMarker>().priority > reached)
                {
                    markerList.Add(marker);
                }
            }

            markerList.Sort(SortPriority); //Sort the markers by priority
            if (markerList.Count > 0)
            {
                nextPos = markerList[0].GetComponent<MoveMarker>().pos;
            }
        }
        if (markerList.Count > 0)
        {
            if (Vector3.Distance(transform.position, nextPos) < arrivalDist)
            {
                reached = markerList[0].GetComponent<MoveMarker>().priority;
                if (markerList[0].GetComponent<MoveMarker>().gameEnder)
                {
                    SceneManager.LoadScene("Credits");
                }
            }
            if (isDay)
            {
                transform.position = Vector3.MoveTowards(transform.position, nextPos, daySpeed);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, nextPos, nightSpeed);
            }
            
        }
    }

    private int SortPriority(GameObject a, GameObject b)
    {
        return a.GetComponent<MoveMarker>().priority.CompareTo(b.GetComponent<MoveMarker>().priority);
    }
}
