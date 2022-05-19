using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : MonoBehaviour
{
    public bool night;
    public bool delayed;
    //Script Objects
    private GameObject raft;
    private GameObject player;

    private Vector3 movePos;    
    private Quaternion stabbedPos;

    private Vector3 lookPos;

    //Variables
    public float time; //Public for testing
    public float cameraShakeLvl;

    public float passivePeriod;
    public float circlePeriod;
    public float approachPeriod;    
    public float aggressivePeriod;

    public float fleeTime;

    public float passiveSpeed;
    public float aggressiveSpeed;
    public float fleeSpeed;

    public float circleOffset;
    public float circleRadius;
    public float aggressiveRadius;
    public float fleeRadius;

    public float circleDepth;
    public float passiveDepth;
    private float fleeDepth;

    private float dynamicRadius; //For approaching
    private float dynamicSpeed;
    private float dynamicDepth;

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

        if (night)
        {
            circleRadius = 15;
            aggressiveRadius = 2f;
            fleeRadius = 50;

            passiveSpeed = 0.3f;
            aggressiveSpeed = 1.1f;
            fleeSpeed = 0.035f;

            passivePeriod = 20f; //Time before is aggressive
            circlePeriod = 30f; //Stalk time
            approachPeriod = 20; //Approach time
            aggressivePeriod = 55; //Time it terrorised the player

            fleeTime = 10;
            circleDepth = -0.3f;
            passiveDepth = -50;
            fleeDepth = -8;

            gameObject.transform.localScale = new Vector3(1.4f, 1.4f, 1.2f);
        }
        else
        {
            /*circleRadius = 20;
            aggressiveRadius = 2.5f;
            fleeRadius = 50;

            passiveSpeed = 0.1f;
            aggressiveSpeed = 0.8f;
            fleeSpeed = 0.025f;

            passivePeriod = 180f; //Time before is aggressive
            circlePeriod = 90f; //Stalk time
            approachPeriod = 30; //Approach time
            aggressivePeriod = 45; //Time it terrorised the player

            fleeTime = 20;
            circleDepth = -0.3f;
            passiveDepth = -50;
            fleeDepth = -8;*/

            gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        }                

        crossing = false;
        if (delayed)
        {
            time = 10f;            
        }
        else
        {
            time = 0f;
            circlePeriod = 20f;
        }        
    }

    private void OnEnable()
    {
        crossing = false;
        if (delayed)
        {
            time = 10f;
        }
        else
        {
            time = 0f;
        }
    }


    void FixedUpdate()
    {
        time += Time.deltaTime;
        
        if (flee) //From agressive to passive
        {
            dynamicDepth = Mathf.Lerp(circleDepth, fleeDepth, time / fleeTime);
            dynamicRadius = Mathf.Lerp(circleRadius, fleeRadius, time / fleeTime);

            movePos = Fleeing(dynamicDepth, dynamicRadius, fleeSpeed, 0f);
            lookPos = Fleeing(dynamicDepth, dynamicRadius, fleeSpeed, 0.5f);

            gameObject.transform.position = movePos;
            transform.LookAt(lookPos);

            if (time > fleeTime)
            {
                time = 0;
                aggressive = false;
                flee = false;
            }
            isNear = false;
        }else if (time < passivePeriod)//Is passive
        {
            aggressive = false;

            dynamicDepth = Mathf.Lerp(passiveDepth, circleDepth, time / passivePeriod);
            dynamicRadius = Mathf.Lerp(fleeRadius, circleRadius, time / passivePeriod);

            movePos = Circling(dynamicDepth, dynamicRadius, passiveSpeed, 0f);
            lookPos = Circling(dynamicDepth, dynamicRadius, passiveSpeed, 0.5f);

            gameObject.transform.position = movePos;
            transform.LookAt(lookPos);
            isNear = false;
        }
        else if (time > passivePeriod) //Interacting with raft
        {
            aggressive = true;
            if (time < (passivePeriod + circlePeriod) || night) //Stalking
            {               
                movePos = Circling(circleDepth, circleRadius, passiveSpeed, 0f);
                lookPos = Circling(circleDepth, circleRadius, passiveSpeed, 0.5f);

                gameObject.transform.position = movePos;               
                transform.LookAt(lookPos);
            }
            else if(time >= (passivePeriod + circlePeriod) && time < (passivePeriod + circlePeriod + approachPeriod)) //Gradually approach raft
            {                
                dynamicRadius = Mathf.Lerp(circleRadius, 0,  (time- (passivePeriod + circlePeriod))/approachPeriod);
                if (night)
                {
                    dynamicSpeed = Mathf.Lerp(passiveSpeed, passiveSpeed + 0.3f, (time - (passivePeriod + circlePeriod)) / approachPeriod);
                }
                else
                {
                    dynamicSpeed = Mathf.Lerp(passiveSpeed, passiveSpeed + 0.03f, (time - (passivePeriod + circlePeriod)) / approachPeriod);
                }                

                movePos = Circling(circleDepth, dynamicRadius, dynamicSpeed, 0f);
                lookPos = Circling(circleDepth, dynamicRadius-0.001f, dynamicSpeed, 0.1f);

                gameObject.transform.position = movePos;
                transform.LookAt(lookPos);
            }
            else if (time >= (passivePeriod + circlePeriod + approachPeriod) && time < (passivePeriod + circlePeriod + approachPeriod + aggressivePeriod)) //Actively terrorising raft
            {
                //Will circle the boat closer, sometimes rise and cross under the boat
                if (!crossing)
                {

                    //Agressive circling
                    if (delayed)
                    {
                        movePos = Crossing(circleDepth, aggressiveRadius, aggressiveSpeed, 5f);
                        lookPos = Crossing(circleDepth, aggressiveRadius, aggressiveSpeed, 5.01f);
                    }
                    else
                    {
                        movePos = Crossing(circleDepth, aggressiveRadius, aggressiveSpeed, 1f);
                        lookPos = Crossing(circleDepth, aggressiveRadius, aggressiveSpeed, 1.01f);
                    }
                    

                    gameObject.transform.position = movePos;
                    transform.LookAt(lookPos);

                }
                if (Vector3.Distance(gameObject.transform.position, raft.transform.position) < 2)
                {
                    //audioPlaying = true;
                    Camera.main.transform.position = Camera.main.transform.position + Random.insideUnitSphere * cameraShakeLvl;
                    if (!audioSource.isPlaying)
                    {
                        audioSource.PlayOneShot(sharkUnderRaft);
                    }
                }
                else
                {
                    Camera.main.transform.localPosition = new Vector3(0,0,0);
                }

            }else if(time > (passivePeriod + circlePeriod + approachPeriod + approachPeriod)) //Reset to passive (lost a raft piece)
            {
                player.GetComponent<Pick>().ReduceLogs();
                player.GetComponent<SpearManager>().RefreshUI();
                audioSource.PlayOneShot(sharkTakesPlank);
                time = 0;
            }
            isNear = true;
        }
        else //Avoiding the raft
        {
            movePos = Circling(passiveDepth, fleeRadius, fleeSpeed, 0);
            lookPos = Circling(passiveDepth, fleeRadius, fleeSpeed, 0 + 0.1f);

            gameObject.transform.position = movePos;
            transform.LookAt(lookPos);
            isNear = false;
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
    private Vector3 Fleeing(float height, float radius, float speed, float offset)
    {
        Vector3 circlePos = new Vector3();
        circlePos.y = height;
        if (stabbedPos.eulerAngles.x > 0)
        {
            circlePos.x = gameObject.transform.position.x + (3 * ((time + offset) * speed));
        }
        else
        {
            circlePos.x = gameObject.transform.position.x + (-3 * ((time + offset) * speed));
        }
        
        circlePos.z = gameObject.transform.position.z + (((Mathf.Pow(time,2f) + offset) * speed));

        return circlePos;
    }

    public void Stabbed()
    {
        audioSource.PlayOneShot(sharkHit);
        time = 0;
        flee = true;
        aggressive = false;
        stabbedPos = gameObject.transform.rotation;
    }
}
