using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    float endPosX, endPosZ;
    public float timer, timeSpeed, timeToMove;
    public Vector3 newPos, raftPos;
    private RaycastHit spearHit;
    private Vector3 clickSpot;

    // Start is called before the first frame update
    void Start()
    {
        NewPosition();
        clickSpot = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        LookMovingDirection(newPos);
        timer += Time.deltaTime * timeSpeed;

        if (Input.GetMouseButtonDown(0))
        {
            GameObject fish = GameObject.FindGameObjectWithTag("Fish");
            Vector3 forceDirection = (fish.transform.position - clickSpot);

            endPosX = forceDirection.x + fish.transform.position.x;
            endPosZ = forceDirection.z + fish.transform.position.z;

            //Assigns new position
            newPos = new Vector3(endPosX, transform.position.y, endPosZ);
            transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * 1f);

        }
        else if (timer >= timeToMove)
        {
            transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * 0.5f);

            if (Vector3.Distance(transform.position, newPos) <= 0.1f)
            {
                NewPosition();
                timer = 0;
            }

        }
    }

    void NewPosition()
    {
        //Gets raft position every time the fish has to go to new position.
        GameObject raft = GameObject.FindGameObjectWithTag("Raft");
        raftPos = raft.transform.position;

        //Chooses random position around the raft for fish based on the raft position. 
        endPosX = Random.Range(raftPos.x - 10, raftPos.x + 10);
        endPosZ = Random.Range(raftPos.z - 10, raftPos.z + 10);

        //Assigns new position
        newPos = new Vector3(endPosX, transform.position.y, endPosZ);
    }

    void LookMovingDirection(Vector3 lookTo)
    {
        Quaternion lookRotation = Quaternion.LookRotation((lookTo - transform.position).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 1f);
    }
}
