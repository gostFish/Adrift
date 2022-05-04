using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMarker : MonoBehaviour
{
    //Objects properties
    public float priority;
    public Vector3 pos;
        
    void Start()
    {
        pos = gameObject.transform.position; //Ensure position is public
        gameObject.tag = "MoveMarker";       //Ensure tag is "MoveMarker"
    }
}
