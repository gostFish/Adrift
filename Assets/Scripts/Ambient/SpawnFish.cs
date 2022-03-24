using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFish : MonoBehaviour
{
    public GameObject fishSpot; //

    private int currentFish;

    public int minFish;
    public int maxFish;
    public float spawntime;
    public float spawnProbability;

    private float time;

    private void Start()
    {
        minFish = 0; //Potentially no fish
        maxFish = 3; //3 fish at once
        spawntime = 2f;
        spawnProbability = 30f;
    }
    // Update is called once per frame
    void Update()
    {
        if(currentFish < maxFish)
        {
            time += Time.deltaTime;
            if (time > spawntime)
            {            
                if (Random.Range(0f,100f) < spawnProbability)
                {
                    currentFish++;
                    time = 0f;
                    Instantiate(fishSpot,new Vector3(100f,100f,100f),Quaternion.identity);
                }
                else
                {
                    time = 0f;
                }
                Debug.Log("There are currently " + currentFish + " fish nearby");
            }
        }
    }
}
