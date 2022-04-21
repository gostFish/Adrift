using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFish : MonoBehaviour
{

    private GameObject instanceManager;
    public GameObject fishSpot;
    private List<GameObject> fishList;

    private int currentFish;

    public int minFish;
    public int maxFish;
    public int maxTotal;
    public float spawntime;
    public float spawnProbability;

    public GameObject raft;

    private float time;

    private void Start()
    {
        minFish = 0; //Potentially no fish
        maxFish = 10; //3 fish at once
        maxTotal = 50;
        spawntime = 1f;
        spawnProbability = 30f;

        instanceManager = GameObject.FindGameObjectWithTag("InstanceManager");

        fishList = new List<GameObject>();
    }
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > spawntime)
        {
            if (Random.Range(0f, 100f) < spawnProbability)
            {
                if (currentFish < maxFish) //Spawn all the fish
                {
                    currentFish++;
                    GameObject newFish = Instantiate(fishSpot, new Vector3(raft.transform.position.x, 0.4f, raft.transform.position.z), Quaternion.identity);
                    newFish.transform.parent = instanceManager.transform;
                    fishList.Add(newFish);
                }
                else
                {
                    for (int i = 0; i < fishList.Count; i++)
                    {
                        if (!fishList[i].activeSelf) //Activate fish once all spawned
                        {
                            fishList[i].active = true;
                            break;
                        }
                    }
                }
            }
            time = 0f;
        }
    }
}
