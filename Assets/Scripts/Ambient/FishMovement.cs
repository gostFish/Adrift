using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    float endPosX, endPosZ;
    public float timer, timeSpeed, timeToMove;
    public Vector3 newPos, raftPos;
    //public FishScattering fishScattering;

    // Start is called before the first frame update
    void Start()
    {
        NewPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //fishScattering.FishScatter();
            //Debug.Log("Fish are scattering");
        }
        else
        {
            Debug.Log("Normal swimming");
            LookMovingDirection(newPos);

            timer += Time.deltaTime * timeSpeed;
            if (timer >= timeToMove)
            {
                transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * 0.5f);

                if (Vector3.Distance(transform.position, newPos) <= 0.1f)
                {
                    NewPosition();
                    timer = 0;
                }

            }
        }

    }

    void NewPosition()
    {
        //Gets raft position every time the fish has to go to new position.
        GameObject raft = GameObject.FindGameObjectWithTag("Raft");
        raftPos = raft.transform.position;

        //Chooses random position around the raft for fish based on the raft position. 
        endPosX = Random.Range(raftPos.x - 5, raftPos.x + 5);
        endPosZ = Random.Range(raftPos.z - 5, raftPos.z + 5);

        //Assigns new position
        newPos = new Vector3(endPosX, transform.position.y, endPosZ);
    }

    void LookMovingDirection(Vector3 lookTo)
    {
        Quaternion lookRotation = Quaternion.LookRotation((lookTo - transform.position).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 0.5f);
    }
}
