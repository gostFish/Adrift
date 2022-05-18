using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFish : MonoBehaviour
{

    private GameObject instanceManager;
    public GameObject fishPrefab, fishPrefab1, fishPrefab2, fishPrefab3, fishPrefab4, fishPrefab5, fishPrefab6;
    private List<GameObject> fishList;

    private int currentFish;

    public int minFish;
    public int maxFish;
    public float spawntime;
    public float spawnProbability;

    public GameObject raft;

    private float time;

    private void Start()
    {
        minFish = 0; //Potentially no fish
        maxFish = 7; 
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
                    int rand = Random.Range(1, 7);
                    ChooseFish(rand);
                    GameObject newFish = Instantiate(fishPrefab, new Vector3(raft.transform.position.x, 0.4f, raft.transform.position.z), Quaternion.identity);
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

    private void ChooseFish(int fishNr)
    {
        switch (fishNr)
        {
            case int prefabNr when prefabNr == 1:
                fishPrefab = fishPrefab1;
                break;
            case int prefabNr when prefabNr == 2:
                fishPrefab = fishPrefab2;
                break;
            case int prefabNr when prefabNr == 3:
                fishPrefab = fishPrefab3;
                break;
            case int prefabNr when prefabNr == 4:
                fishPrefab = fishPrefab4;
                break;
            case int prefabNr when prefabNr == 5:
                fishPrefab = fishPrefab5;
                break;
            case int prefabNr when prefabNr == 6:
                fishPrefab = fishPrefab6;
                break;
        }
    }
}
