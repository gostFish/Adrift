using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : MonoBehaviour
{

    //Script Objects
    private GameObject raft;
    private GameObject player;
    private Vector3 currentPos;
    private Vector3 movePos;    
    private Vector3 fleePos;
    //private Vector3 crossPos;

    private Vector3 lookPos;

    //Variables
    private float time; //Public for testing
    
    public float passivePeriod;
    public float circlePeriod;
    public float approachPeriod;    
    public float agressivePeriod;

   // private float timeStamp;
    //public float crossTime;
    public float fleeTime;

    public float passiveSpeed;
    public float agressiveSpeed;
    public float fleeSpeed;

    public float circleRadius;
    public float aggressiveRadius;
    public float fleeRadius;

    public float circleDepth;
    public float passiveDepth;

    private float dynamicRadius; //For approaching
    private float dynamicSpeed;

    private float dynamicOffset; //For shark attacking

    //Shark states    

    public bool aggressive; //In attack mode
    public bool isNear;

    public bool flee;
    private bool crossing;

    //private bool audioPlaying;

    //Shark Sounds

    private AudioSource audioSource;
    public AudioClip sharkHit;
    public AudioClip sharkUnderRaft;
    public AudioClip sharkTakesPlank;

    void Start()
    {
        raft = GameObject.FindGameObjectWithTag("Raft");
        player = GameObject.FindGameObjectWithTag("Player");
        audioSource = GetComponent<AudioSource>();
        //Temp values
     /*   circleRadius = 15;
        aggressiveRadius = 4f;
        fleeRadius = 50;

        passiveSpeed = 0.3f;
        agressiveSpeed = 0.9f;
        fleeSpeed = 0.01f;

        passivePeriod = 3f; //Time before is aggressive
        circlePeriod = 10f; //Stalk time
        approachPeriod = 10; //Approach time
        agressivePeriod = 10; //Time it terrorised the player

        fleeTime = 20;
        circleDepth = -0.3f;         */

        dynamicOffset = 0;
        crossing = false;

        time = 0f;
    }

    void FixedUpdate()
    {
        time += Time.deltaTime;

        if(!aggressive && time > passivePeriod)
        {
            aggressive = true;
            //transform.Translate(transform.forward);
        }
        else if (flee)
        {
            Debug.Log("fleeing");
            transform.localPosition = Vector3.Lerp(currentPos, fleePos, time/fleeTime);
            transform.LookAt(fleePos);

            if (time > fleeTime)
            {
                time = 0;
                aggressive = false;
                flee = false;
            }
        }
        else if (aggressive) //Interacting with raft
        {
            Debug.Log("agressive");
            if (time < circlePeriod) //Stalking
            {               
                movePos = Circling(circleDepth, circleRadius, passiveSpeed, 0f);
                lookPos = Circling(circleDepth, circleRadius, passiveSpeed, 0.5f);

                gameObject.transform.position = movePos;
                transform.LookAt(lookPos);
            }
            else if(time >= circlePeriod && time < (circlePeriod + approachPeriod)) //Gradually approach raft
            {
                dynamicRadius = Mathf.Lerp(circleRadius, aggressiveRadius, (time - circlePeriod) / approachPeriod);
                dynamicSpeed = Mathf.Lerp(passiveSpeed*0.7f, agressiveSpeed*0.7f, (time - circlePeriod) / approachPeriod);

                movePos = Circling(circleDepth, dynamicRadius, dynamicSpeed, 0f);
                lookPos = Circling(circleDepth, dynamicRadius, dynamicSpeed, 0.1f);

                gameObject.transform.position = movePos;
                transform.LookAt(lookPos);
            }
            else if (time >= circlePeriod && time < (circlePeriod + approachPeriod + approachPeriod)) //Actively terrorising raft
            {
                //Will circle the boat closer, sometimes rise and cross under the boat
                if (!crossing)
                {
                    //Agressive circling
                    movePos = Crossing(circleDepth, aggressiveRadius, agressiveSpeed, dynamicOffset);
                    lookPos = Crossing(circleDepth, aggressiveRadius, agressiveSpeed, dynamicOffset + 0.15f);

                    gameObject.transform.position = movePos;
                    transform.LookAt(lookPos);                                        
                }
                if (Vector3.Distance(gameObject.transform.position, raft.transform.position) < 2)
                {
                    //audioPlaying = true;
                    if (!audioSource.isPlaying)
                    {
                        audioSource.PlayOneShot(sharkUnderRaft);
                    }
                    
                }

            }else if(time > (circlePeriod + approachPeriod + approachPeriod))
            {
                //player.GetComponent<Pick>().ReduceLogs();
                audioSource.PlayOneShot(sharkTakesPlank);
                time = 0;
            }
        }
        else //Avoiding the raft
        {
            movePos = Circling(passiveDepth, fleeRadius, fleeSpeed, 0);
            lookPos = Circling(passiveDepth, fleeRadius, fleeSpeed, 0 + 0.15f);

            gameObject.transform.position = movePos;
            transform.LookAt(lookPos);
        }                
    }

    private Vector3 Circling(float height,float radius, float speed, float offset)
    {
        Vector3 circlePos = new Vector3();
        circlePos.y = height;
        circlePos.x = raft.transform.position.x + (radius * (Mathf.Cos((time + offset) * speed)));
        circlePos.z = raft.transform.position.z + (radius * (Mathf.Sin((time + offset) * speed)));

        return circlePos;
    }
    private Vector3 Crossing(float height, float radius, float speed, float offset)
    {
        Vector3 circlePos = new Vector3();
        circlePos.y = height;
        circlePos.x = raft.transform.position.x + (radius * (Mathf.Cos((time + offset) * speed)));
        circlePos.z = raft.transform.position.z + (radius *(Mathf.Sin(2*((time + offset) * speed))));

        return circlePos;
    }

    public void Stabbed()
    {
        currentPos = transform.position;
        audioSource.PlayOneShot(sharkHit);
        fleePos = Circling(passiveDepth, fleeRadius, passiveSpeed, 0f);
        flee = true;
        dynamicOffset = 0;
        time = 0;
    }


}
