using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterManager : MonoBehaviour
{

    public GameObject water; //Empty object to hold the water chunks
    public float waterSize;
    public float viewDist;

    private GameObject player;
    private Vector3 playerPos;

    private GameObject middleChunk; //Player should be in this, otherwise, update the sorrounding
    private List<GameObject> waterChunks;
    private List<GameObject> freeChunks;
    private RaycastHit hit;
    private Vector3 rayPos;

    private float time;

    private float waterSizeCorrection;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");

        waterSizeCorrection = 20; //To test with final water prefab

        waterChunks = new List<GameObject>();
        freeChunks = new List<GameObject>();
        for (int i = -1, w = 0; i < 2; i++) //also iterates w (1d array of water)
        {            
            for (int j = -1; j < 2; j++,w++)
            {
                waterChunks.Add(Instantiate(water,new Vector3((waterSize/2) * i ,0, (waterSize / 2) * j),Quaternion.identity));
                waterChunks[w].transform.localScale = new Vector3(waterSize/ waterSizeCorrection, 1, waterSize/ waterSizeCorrection);                
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time += Time.deltaTime;

        if (time < 0.5) //Update every 0.5 seconds
        {
            //Check if water chunks are too far
            for (int i = 0; i < waterChunks.Count; i++) //also iterates w (1d array of water)
            {
                if (Vector3.Distance(waterChunks[i].transform.position, player.transform.position) > viewDist)
                {
                    freeChunks.Add(waterChunks[i]);
                    waterChunks.Remove(waterChunks[i]);
                }
            }
            if(freeChunks.Count > 0)
            {
                UpdateClusters();
            }
            time = 0;
        }
    }

    public void UpdateClusters() //Make sure there is water in range of the player
    {
        for (int i = -1, w = 0; i < 2; i++) //also iterates w (1d array of water)
        {
            for (int j = -1; j < 2; j++, w++)
            {
                rayPos = new Vector3((waterSize / 2) * i, Waves.instance.amplitude, (waterSize / 2) * j);

                //Raycast checks all corners around raft (with height = 2x wave amplitude)
                if (Physics.Raycast(rayPos, Vector3.down, out hit, Waves.instance.amplitude * 2) && hit.transform.tag == "Water")
                {
                    rayPos.y = 0;
                    freeChunks[freeChunks.Count - 1].transform.position = rayPos; //Move to new position

                    waterChunks.Add(freeChunks[freeChunks.Count-1]); //Change array again
                    freeChunks.Remove(freeChunks[freeChunks.Count - 1]);
                }                    
            }
        }
    }
}
