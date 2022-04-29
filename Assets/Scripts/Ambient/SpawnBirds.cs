using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBirds : MonoBehaviour
{
    private GameObject instanceManager;
    public GameObject birdSpot;

    private int currentBirdsCount;

    public float spawntime;
    public float spawnProbability;

    public GameObject raft;
    private GameObject newBird;

    private float time;
    private float posX, posY, posZ;
    private int flockSize;

    private void Start()
    {
        spawntime = 1f;
        spawnProbability = 30f;
        currentBirdsCount = 0;
        newBird = GameObject.FindGameObjectWithTag("bird");
        instanceManager = GameObject.FindGameObjectWithTag("InstanceManager");
    }
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > spawntime)
        {
            if (Random.Range(0f, 100f) < spawnProbability)
            {
                    currentBirdsCount++;
                    SpawnBird();
            }
            time = 0f;
        }

    }

    public void SpawnBird()
    {
        posX = Random.Range(raft.transform.position.x, raft.transform.position.x + 50);
        posY = Random.Range(20f, 30f);
        posZ = Random.Range(raft.transform.position.z, raft.transform.position.z + 50);
        newBird = Instantiate(birdSpot, new Vector3(posX, posY, posZ), Quaternion.identity);
        newBird.transform.parent = instanceManager.transform;
        MakeFlock();
    }

    public void MakeFlock()
    {
        flockSize = Random.Range(1, 5);
        while (flockSize >= 0)
        {
            posX = Random.Range(newBird.transform.position.x + 3, newBird.transform.position.x - 3);
            posZ = Random.Range(newBird.transform.position.z - 3, newBird.transform.position.z - 6);
            newBird = Instantiate(birdSpot, new Vector3(posX, newBird.transform.position.y, posZ), Quaternion.identity);
            newBird.transform.parent = instanceManager.transform;
            flockSize--;
        }
    }
}
