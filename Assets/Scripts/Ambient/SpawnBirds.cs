using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBirds : MonoBehaviour
{

    //Game Objects
    private GameObject instanceManager;
    private GameObject sceneManager;

    private List<GameObject> allFlocks;
    private GameObject flock;
    public GameObject birdSpot;

    public GameObject raft;
    private GameObject leaderBird;
    private GameObject newBird;

    //Variables

    //Public
    public float spawntime;
    public float spawnProbability;

    public int minFlockSize;
    public int maxFlockSize;

    public int maxBirds;
    private int birdCount;

    //Private
    private float time;
    private float posX, posY, posZ;
    private int flockSize;
    private int nextRecycle;


    private void Start()
    {            
        //spawntime = 1f;
        //spawnProbability = 7f;

        nextRecycle = 0;
        birdCount = 1;
        
        instanceManager = GameObject.FindGameObjectWithTag("InstanceManager");
        sceneManager = GameObject.FindGameObjectWithTag("SceneManager");
                
        allFlocks = new List<GameObject>();

        SpawnBird(); //Starter bird
    }
    
    void Update()
    {
        time += Time.deltaTime;

        if (time > spawntime)
        {
            if (sceneManager.GetComponent<DayCycle>().isDay==1)
            {
                if (Random.Range(0f, 100f) < spawnProbability)
                {
                    if (birdCount < maxBirds) //Instantiate necessary birds
                    {                    
                        SpawnBird();
                        birdCount++;
                    }
                    else //Recycle birds
                    {
                        //Place an old flock in a new position
                        PlaceBird(allFlocks[nextRecycle]);

                        //Recycle counter loops through all birds
                        if(nextRecycle < maxBirds - 1)
                        {
                            nextRecycle++;
                        }
                        else
                        {
                            //Max counter reach, start from 0 again
                            nextRecycle = 0;
                        }
                    }
                }
                time = 0f;
            }            
        } 
    }

    public void SpawnBird()
    {
        posX = raft.transform.position.x + 250;
        posY = Random.Range(20f, 30f);
        posZ = Random.Range(raft.transform.position.z-25, raft.transform.position.z+25);

        //Make leader bird
        leaderBird = Instantiate(birdSpot, new Vector3(posX, posY, posZ), Quaternion.identity);
        leaderBird.transform.parent = instanceManager.transform;
        leaderBird.name = "FlockLeader";
        MakeFlock();
    }

    public void MakeFlock()
    {
        flockSize = Random.Range(minFlockSize, maxFlockSize);

        //Sort birds as a pack in a game object
        flock = new GameObject();
        flock.name = "Bird Flock";
        flock.transform.parent = instanceManager.transform;

        allFlocks.Add(flock);
        leaderBird.transform.parent = flock.transform;

        //Make follower birds
        int rowCount = 1;        
        while (flockSize >= 0)
        {
            posX = leaderBird.transform.position.x;
            posZ = leaderBird.transform.position.z;

            //Spawn new bird on the right            
            newBird = Instantiate(birdSpot, new Vector3(posX + (1 * rowCount), leaderBird.transform.position.y, posZ + (3*rowCount)), Quaternion.identity);
            newBird.transform.parent = flock.transform; //Bird is child of flock

            //Spawn a new bird on the left
            newBird = Instantiate(birdSpot, new Vector3(posX + (1 * rowCount), leaderBird.transform.position.y, posZ - (3 * rowCount)), Quaternion.identity);
            newBird.transform.parent = flock.transform; //Bird is child of flock
            flockSize -= 2;
            rowCount++;
        }
    }

    public void PlaceBird(GameObject bird)
    {
        
        Vector3 newPos = bird.transform.position;
        newPos.x = newPos.x + Vector3.Distance(bird.transform.GetChild(0).transform.position, raft.transform.position) + 250;
        bird.transform.position = newPos;
    }
}
