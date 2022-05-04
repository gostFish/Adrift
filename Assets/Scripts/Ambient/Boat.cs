using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{

    //Variables

    public float waterOffset = 1f;
    public float floatStrength = 3f;
    public float rotationLevel = 3f;

    public int remainingPlanks;
    public int minPlanks;

    private Vector3 pos;
    private Quaternion rot;


    void Update()
    {
        //Ensure in line with the waves (Position)
        float x = Waves.instance.GetWaveHeight(transform.position.x);
        float z = Waves.instance.GetWaveHeight(transform.position.z);

        pos = transform.position;
        rot = transform.rotation;

        //Ensure in line with waves (Rotation)
        rot.x = (Mathf.Acos(x / (Waves.instance.length + Waves.instance.offset)) * Mathf.Rad2Deg) - 90;
        rot.z = (Mathf.Acos(z / (Waves.instance.length + Waves.instance.offset)) * Mathf.Rad2Deg) - 90;

        pos.y = x + z + waterOffset;

        transform.position = pos;
        transform.rotation = Quaternion.Euler(rot.x, 0, rot.z);
    }
}