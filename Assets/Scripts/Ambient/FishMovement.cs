using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    //Variables
    private float endPosX, endPosZ;
    private float xPos, yPos, zPos;
    private float xLook, yLook, zLook;

    public float timer, timeSpeed, timeToMove;
    public float fishSpeed;


    private Vector3 newFishPos, newfishDir;

    private float fleeTimer;
    private bool react, escaping, moving;

    public Vector3 newPos, raftPos;

    private Vector3 clickSpot, stabSpot;
    private Vector3 initialPos;    

    private float newPosAngle, newPosRadius;
    private float randomRot;

    //Game objects

    public GameObject fish;
    public GameObject raft;
    private GameObject shark;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
        shark = GameObject.FindGameObjectWithTag("Shark");
        player = GameObject.FindGameObjectWithTag("Player");
        raft = GameObject.FindGameObjectWithTag("Raft");

        react = false;

        NewPosition();
    }

    // Update is called once per frame
    void Update()
    {
        
        //Player is stabbing (might not need to react)
        if (player.GetComponent<SpearManager>().stabbing) 
        {            
            if (!react) //Is not already reacting (so test react)
            {                            
                StartCoroutine(checkReact());
            }
        }

        //Player is hitting something (for 0.1 seconds)
        if (player.GetComponent<SpearManager>().strikeContact) 
        {            
            if (!escaping) //Escape if not escaping already
            {
                randomRot = Random.Range(-180 * Mathf.Deg2Rad, 180 * Mathf.Deg2Rad);
                StartCoroutine(checkReact());
            }
        }

        //Movement timer
        
        if (Vector3.Distance(transform.position, newPos) >= 0.1f)
        {
            if (!moving) //Set new Destination
            {     
                 moving = true;                                     
            }
        }   
        else
        {
            NewPosition();
            moving = false;
        }

        if (timer > timeToMove)
        {
            NewPosition();
            timer = 0;
        }        
    }

    

    //float decelerate = 1f;
    void FixedUpdate()
    {
        //Timer should be in fixed update (due to physics)
        timer = timer + Time.deltaTime;

        if (moving)
        {
            MoveNewPos();
        }

        if (escaping) //Playing the escape motion
        {
            MoveEscape();
        }
    }

    
    //Applying normal move motion
    private void MoveNewPos()
    {

        
        
            //Move forward (as fish do when they move)
            transform.position += transform.forward * Time.deltaTime * fishSpeed; 
            //Rotate fish to needed position
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(newPos - transform.position), Time.deltaTime * 2f);
                
    }

    //Applying Escape motion
    private void MoveEscape()
    {
        fleeTimer += Time.deltaTime * Mathf.Lerp(4, 0, fleeTimer / 2);  //Start fast and decelerate
                
        //Make new axis with Formula
        xPos = initialPos.x + (Mathf.Pow(fleeTimer, 3) - (fleeTimer)) * Mathf.Sin(randomRot);
        yPos = Mathf.Lerp(initialPos.y, initialPos.y, fleeTimer * 0.1f);
        zPos = initialPos.z + (fleeTimer) * Mathf.Cos(randomRot);

        //Same formula with offset to get direction
        xLook = initialPos.x + (Mathf.Pow(fleeTimer + 0.1f, 3) - (fleeTimer + 0.1f)) * Mathf.Sin(randomRot);
        yLook = Mathf.Lerp(initialPos.y, initialPos.y, (fleeTimer * 0.1f));
        zLook = initialPos.z + (fleeTimer + 0.1f) * Mathf.Cos(randomRot);

        newFishPos = new Vector3(xPos, yPos, zPos);
        newfishDir = new Vector3(xLook, yLook, zLook);

        //Apply motion
        transform.position = newFishPos;
        transform.LookAt(newfishDir);
    }
        
    
    private void NewPosition()
    {        
        //Determin vars for pos (also used to lerp and turn to it)
        newPosAngle = Random.Range(-180 * Mathf.Deg2Rad, 180 * Mathf.Deg2Rad);
        newPosRadius = Random.Range(0.8f, 5.5f);

        //Chooses random position around the raft for fish based on the raft position. 
        if (Random.value > 0.5)
        {
            endPosX = raft.transform.position.x + (Mathf.Sin(newPosAngle) * newPosRadius);
        }
        else
        {
            endPosX = raft.transform.position.x - (2 * (Mathf.Sin(newPosAngle) * newPosRadius));
        }

        if (Random.value > 0.5)
        {
            endPosZ = raft.transform.position.z + (Mathf.Cos(newPosAngle) * newPosRadius);
        }
        else
        {
            endPosZ = raft.transform.position.z - (2 * (Mathf.Cos(newPosAngle) * newPosRadius));
        }
        //endPosX = raft.transform.position.x + (Mathf.Sin(newPosAngle) * newPosRadius); 
        //endPosZ = raft.transform.position.z + (Mathf.Cos(newPosAngle) * newPosRadius);

        //Assigns new position
        if (!shark.GetComponent<Shark>().isNear)
        {
            newPos = new Vector3(endPosX, 0.4f, endPosZ); //Swim at normal depth
        }
        else
        {
            newPos = new Vector3(endPosX, -25f, endPosZ); //Swim deeper to avoid shark
        }
            
    }

    void NewPositionShark()
    {
        endPosX = Random.Range(raftPos.x - 20, raftPos.x - 10);
        endPosZ = Random.Range(raftPos.z - 20, raftPos.z - 10);

        //Assigns new position
        newPos = new Vector3(endPosX, transform.position.y, endPosZ);
    }

    void LookMovingDirection(Vector3 lookTo)
    {
        Quaternion lookRotation = Quaternion.LookRotation((lookTo - transform.position).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2f);
    }

    
    IEnumerator checkReact() //check if the fish should react
    {        
        yield return new WaitForSeconds(0.05f); //Wait for click position to be returned from spear update        
        
        //Get positions of last clicked spot and last stabbed spot
        clickSpot = player.GetComponent<SpearManager>().clickPos;
        stabSpot = player.GetComponent<SpearManager>().hitPos;

        if (!player.GetComponent<SpearManager>().strikeContact) //Checking because player struck something
        {
            //If spear is aiming at the fish, slow react            
            if (Vector3.Distance(transform.position, clickSpot) < 0.5f) 
            {                
                react = true;
                StartCoroutine(SlowReact());
            }            
        }
        else //Checking because player clicked (not necessarily hit something)
        {
            //If something is hit and is close, immediately escape
            if (Vector3.Distance(transform.position, stabSpot) < 5) 
            {                
                react = true;
                StartCoroutine(Escape());
            }            
        }        
    }

    IEnumerator SlowReact() //Time to notice player is attacking them (different from water being hit)
    {
        yield return new WaitForSeconds(1.2f);
        StartCoroutine(Escape());        
    }

    IEnumerator Escape()
    {
        initialPos = transform.position;
        fleeTimer = 0;
        escaping = true;
        yield return new WaitForSeconds(1f);
        react = false;
        escaping = false;
    }

}

