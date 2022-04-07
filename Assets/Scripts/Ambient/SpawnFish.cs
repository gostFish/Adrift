using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFish : MonoBehaviour
{
    public GameObject fishSpot; //
    private List<GameObject> fishList;

    private int currentFish;

    public int minFish;
    public int maxFish;
    public int maxTotal;
    public float spawntime;
    public float spawnProbability;

    private float time;

    private void Start()
    {
        minFish = 0; //Potentially no fish
        maxFish = 3; //3 fish at once
        maxTotal = 50;
        spawntime = 1f;
        spawnProbability = 30f;

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
                    GameObject newFish = Instantiate(fishSpot, new Vector3(100f, 100f, 100f), Quaternion.identity); 
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
            Debug.Log("There are currently " + currentFish + " fish nearby");
        }
    }
}
