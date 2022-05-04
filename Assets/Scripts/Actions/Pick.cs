using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pick : MonoBehaviour
{

    //Variables

    public int minLogs;
    private int logCount;

    private int frontCounter;
    private int backCounter;

    private bool dead = false;
    private bool takeFront;

    private float deathAnimTime;

    //GameObjects
    private GameObject player;
    private GameObject[] logs;

    public GameObject playerUI;
    public GameObject deathScreen;

    private Camera mainCam;

    private Vector3 startPos;
    private Vector3 sinkPos1;
    private Vector3 sinkPos2;
    private Vector3 sinkPos3;
    private Vector3 sinkPos4;

    private Quaternion startAngle;
    private Quaternion sinkSwayRightAngle;
    private Quaternion sinkSwayLeftAngle;
    private Quaternion sinkAngle;

    // Start is called before the first frame update
    void Start()
    {
        logs = GameObject.FindGameObjectsWithTag("Log");
        logCount = logs.Length;

        backCounter = 0;
        frontCounter = logCount-1;
        takeFront = true;

        dead = false;
        deathAnimTime = 0;

        player = GameObject.FindGameObjectWithTag("Player");
        mainCam = Camera.main;
        startPos = player.transform.position;
        startAngle = player.transform.rotation;

        sinkPos1 = startPos;
        sinkPos1.y -= 6;
        sinkPos2 = startPos;
        sinkPos2.y -= 8;
        sinkPos3 = startPos;
        sinkPos3.y -= 10;
        sinkPos4 = startPos;
        sinkPos4.y -= 14;

        sinkAngle = Quaternion.Euler(-90, 0, 0);
        sinkSwayRightAngle = Quaternion.Euler(-90, 0, -25);
        sinkSwayLeftAngle = Quaternion.Euler(-90, 0, 25);
    }

    private void FixedUpdate()
    {        
        if (dead)
        {            
            if (deathAnimTime < 1) //draw back
            {
                deathAnimTime += Time.deltaTime / 2f;
                player.transform.localPosition = Vector3.Lerp(startPos, sinkPos1, deathAnimTime);
                mainCam.transform.localRotation = Quaternion.Lerp(startAngle, sinkAngle, deathAnimTime);
                
            }
            else if (deathAnimTime <= 2.1) //stab
            {
                deathAnimTime += Time.deltaTime / 4f;
                player.transform.localPosition = Vector3.Lerp(sinkPos1, sinkPos2, deathAnimTime - 1.1f);
                mainCam.transform.localRotation = Quaternion.Lerp(sinkAngle, sinkSwayRightAngle, deathAnimTime -1.1f);
            }
            else if (deathAnimTime <= 3.2) //stab
            {
                deathAnimTime += Time.deltaTime / 4f;
                player.transform.localPosition = Vector3.Lerp(sinkPos2, sinkPos3, deathAnimTime - 2.2f);
                mainCam.transform.localRotation = Quaternion.Lerp(sinkSwayRightAngle, sinkSwayLeftAngle, deathAnimTime -2.2f);
            }
            else if (deathAnimTime <= 4.3) //stab
            {
                deathAnimTime += Time.deltaTime / 14f;
                player.transform.localPosition = Vector3.Lerp(sinkPos3, sinkPos4, deathAnimTime - 3.3f);
                mainCam.transform.localRotation = Quaternion.Lerp(sinkSwayLeftAngle, sinkAngle, deathAnimTime -3.3f);
            }
        }                
    }

    public void PickLog()
    {
        if(PlayerPrefs.GetInt("HasSpear") == 0)
        {
            gameObject.GetComponent<SpearManager>().RefreshSpear();
            PlayerPrefs.SetInt("HasSpear", 1);
            ReduceLogs();
        }
        
        if(logCount <= minLogs)
        {
            DeathScreen();
            foreach(GameObject log in logs) //All logs disables with death
            {
                log.SetActive(false);
            }
        }
    }

    public void ReduceLogs()
    {
        logCount--;
        if (takeFront)
        {
            logs[frontCounter].SetActive(false);
            frontCounter--;
            takeFront = false;
        }
        else
        {
            logs[backCounter].SetActive(false);
            backCounter++;
            takeFront = true;
        }
    }

    public void DeathScreen()
    {
        playerUI.SetActive(false);
        player.GetComponent<PlayerView1stPerson>().enabled = false;
        dead = true;
        StartCoroutine(DrownAnim());
    }

    IEnumerator DrownAnim()
    {
        yield return new WaitForSeconds(3f);
        deathScreen.SetActive(true);
    }
}
