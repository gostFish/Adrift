using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour
{
    public static Waves instance;

    public float amplitude = 1f;
    public float length = 2f;
    public float speed = 1f;
    public float offset = 0f;
    private void Awake()
    {
        //Making into simpleton (only one occurence can exist)
        if(instance == null)
        {
            instance = this;
        }else if( instance != this)
        {
            Debug.Log("More than one instance of waves used! There should only be one - object destroyed");
            Destroy(this);
        }
    }

    private void FixedUpdate()
    {
        offset += Time.deltaTime * speed;        
    }

    public float GetWaveHeight(float xCoord)
    {
        return amplitude * Mathf.Sin(xCoord / length + offset);
    }
}
