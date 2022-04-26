using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : MonoBehaviour
{

    //Script Objects
    private GameObject raft;
    private Vector3 movePos;    
    private Vector3 fleePos;
    private Vector3 crossPos;

    private Vector3 lookPos;

    //Variables
    public float time; //Public for testing

    
    public float passivePeriod;
    public float approachPeriod;
    public float circlePeriod;
    public float agressivePeriod;

    private float timeStamp;
    public float crossTime;
    public float fleeTime;

    public float passiveSpeed;
    public float agressiveSpeed;
    public float fleeSpeed;

    public float circleRadius;
    public float aggressiveRadius;
    public float fleeRadius;

    public float circleDepth;
    public float passiveDepth;
    public float maxAttackDepth;
    public float minAttackDepth;

    public float rotateSpeed;


    private float dynamicRadius; //For approaching
    private float dynamicSpeed;

    private float dynamicOffset; //For shark attacking

    //Shark states    

    public bool aggressive; //In attack mode
    public bool isNear;

    public bool flee;
    private bool crossing;


    void Start()
    {
        raft = GameObject.FindGameObjectWithTag("Raft");

        //Temp values
        circleRadius = 15;
        aggressiveRadius = 4f;
        fleeRadius = 20;

        passiveSpeed = 0.3f;
        agressiveSpeed = 0.9f;
        fleeSpeed = 0.01f;

        passivePeriod = 3f; //Time before is aggressive
        circlePeriod = 10f; //Stalk time
        approachPeriod = 10; //Approach time
        agressivePeriod = 10; //Time it terrorised the player

        fleeTime = 20;

        circleDepth = -0.3f;
        maxAttackDepth = 0.5f;
        minAttackDepth = 4.5f;

        dynamicOffset = 0;
        crossing = false;

        time = 0f;
    }

    void FixedUpdate()
    {
        time += Time.deltaTime;

        if(!aggressive && time > passivePeriod) //Leave raft alone
        {
            aggressive = true;
            transform.Translate(transform.forward);
        }
        else if (flee)
        {            
            fleePos = Circling(passiveDepth, fleeRadius, passiveSpeed, 0f);
            
            transform.Translate(transform.forward * fleeSpeed); //Move forward while turning
            transform.LookAt(transform.forward);
            //transform.Rotate((fleePos - transform.position).normalized * Time.deltaTime * rotateSpeed);// = Quaternion.Slerp(transform.localRotation,fleeAngle,rotateTime);
            
            if (time > fleeTime)
            {
                time = 0;
                aggressive = false;
                flee = false;
            }
        }
        else if (aggressive) //Interacting with raft
        {           

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
                dynamicSpeed = Mathf.Lerp(passiveSpeed, agressiveSpeed, (time - circlePeriod) / approachPeriod);

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

            }else if(time > (circlePeriod + approachPeriod + approachPeriod))
            {
                time = 0;
            }
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
        flee = true;
        dynamicOffset = 0;
        time = 0;
    }


}
