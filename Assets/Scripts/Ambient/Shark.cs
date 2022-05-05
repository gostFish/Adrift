using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : MonoBehaviour
{

    //Script Objects
    private GameObject raft;
    private GameObject player;
    private Vector3 currentPos;
    private Vector3 oldPos;
    private Vector3 movePos;    
    private Vector3 fleePos;

    private Vector3 lookPos;

    //Variables
    public float time; //Public for testing
    
    public float passivePeriod;
    public float circlePeriod;
    public float approachPeriod;    
    public float aggressivePeriod;

    public float fleeTime;

    public float passiveSpeed;
    public float aggressiveSpeed;
    public float fleeSpeed;

    public float circleRadius;
    public float aggressiveRadius;
    public float fleeRadius;

    public float circleOffset;

    public float circleDepth;
    public float passiveDepth;

    private float dynamicRadius; //For approaching
    public float dynamicSpeed;

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
        /*circleRadius = 15;
        aggressiveRadius = 4f;
        fleeRadius = 50;

        passiveSpeed = 0.3f;
        agressiveSpeed = 0.9f;
        fleeSpeed = 0.01f;

        passivePeriod = 100f; //Time before is aggressive
        circlePeriod = 60f; //Stalk time
        approachPeriod = 10; //Approach time
        agressivePeriod = 10; //Time it terrorised the player

        fleeTime = 20;
        circleDepth = -0.3f;        */ 

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
        else if (flee) //From agressive to passive
        {            
            transform.localPosition = Vector3.Lerp(currentPos, fleePos, fleeTime/time);
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
            GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
            if (time < (passivePeriod + circlePeriod)) //Stalking
            {               
                movePos = Circling(circleDepth, circleRadius, passiveSpeed, 0f);
                lookPos = Circling(circleDepth, circleRadius, passiveSpeed, 0.5f);

                gameObject.transform.position = movePos;               
                transform.LookAt(lookPos);

                oldPos = movePos;
            }
            else if(time >= (passivePeriod + circlePeriod) && time < (passivePeriod + circlePeriod + approachPeriod)) //Gradually approach raft
            {
                

                dynamicRadius = Mathf.Lerp(circleRadius, aggressiveRadius,  (time- (passivePeriod + circlePeriod))/approachPeriod);
                dynamicSpeed = Mathf.Lerp(passiveSpeed, aggressiveSpeed, (time - (passivePeriod + circlePeriod)) / approachPeriod);

                movePos = Circling(circleDepth, dynamicRadius, dynamicSpeed, 0f);
                lookPos = Circling(circleDepth, dynamicRadius, dynamicSpeed, 0.001f);

                gameObject.transform.position = movePos;
                transform.LookAt(lookPos);
                oldPos = movePos;
            }
            else if (time >= (passivePeriod + circlePeriod + approachPeriod) && time < (passivePeriod + circlePeriod + approachPeriod + aggressivePeriod)) //Actively terrorising raft
            {
                //Will circle the boat closer, sometimes rise and cross under the boat
                if (!crossing)
                {
                    //dynamicSpeed = Mathf.Lerp(passiveSpeed, aggressiveSpeed *1.5f, (time - (passivePeriod + circlePeriod + approachPeriod)) / aggressivePeriod);

                    //Agressive circling
                    movePos = Crossing(circleDepth, aggressiveRadius, dynamicSpeed, 0f);
                    lookPos = Crossing(circleDepth, aggressiveRadius, dynamicSpeed,  0.001f);

                    gameObject.transform.position = movePos;
                    transform.LookAt(lookPos);
                    oldPos = movePos;
                }
                if (Vector3.Distance(gameObject.transform.position, raft.transform.position) < 2)
                {
                    //audioPlaying = true;
                    if (!audioSource.isPlaying)
                    {
                        audioSource.PlayOneShot(sharkUnderRaft);
                    }                    
                }

            }else if(time > (passivePeriod + circlePeriod + approachPeriod + approachPeriod)) //Reset to passive (lost a raft piece)
            {
                player.GetComponent<Pick>().ReduceLogs();
                audioSource.PlayOneShot(sharkTakesPlank);
                time = 0;
                GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            }
        }
        else //Avoiding the raft
        {
            movePos = Circling(passiveDepth, fleeRadius, fleeSpeed, 0);
            lookPos = Circling(passiveDepth, fleeRadius, fleeSpeed, 0 + 0.1f);

            gameObject.transform.position = movePos;
            transform.LookAt(lookPos);

            oldPos = movePos;
        }                
    }

    private Vector3 Circling(float height,float radius, float speed, float offset)
    {
        Vector3 circlePos = new Vector3();
        circlePos.y = height;
        circlePos.x = raft.transform.position.x + (((radius / (2 - circleOffset)) * ((2 - circleOffset) + (circleOffset * Mathf.Sin(time))) * (Mathf.Cos((time + offset) * speed))));
        circlePos.z = raft.transform.position.z + (((radius / (2 - circleOffset)) * ((2 - circleOffset) + (circleOffset * Mathf.Sin(time))) * (Mathf.Sin((time + offset) * speed))));

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
        time = 0;
        dynamicOffset = 0;
        flee = true;
        
        
    }


}
