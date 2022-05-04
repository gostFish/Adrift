using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdsMovement : MonoBehaviour
{
    public Vector3 newPos;

    // Update is called once per frame
    void Update()
    {
        newPos = new Vector3(transform.position.x - 200, transform.position.y, transform.position.z + 100);
        LookMovingDirection(newPos);
        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * 0.05f);  
    }

    void LookMovingDirection(Vector3 lookTo)
    {
        Quaternion lookRotation = Quaternion.LookRotation((lookTo - transform.position).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 1f);
    }
}
