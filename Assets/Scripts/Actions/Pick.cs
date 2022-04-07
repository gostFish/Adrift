using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pick : MonoBehaviour
{
    public int minLogs;
    private int logCount;
    private GameObject[] logs;

    private int frontCounter;
    private int backCounter;

    private bool takeFront;

    // Start is called before the first frame update
    void Start()
    {
        logs = GameObject.FindGameObjectsWithTag("Log");
        logCount = logs.Length;

        backCounter = 0;
        frontCounter = logCount-1;
        takeFront = true; 
    }

    public void PickLog()
    {
        if(logCount > minLogs && PlayerPrefs.GetInt("HasSpear") == 0)
        {
            gameObject.GetComponent<HoldSpear>().RefreshSpear();
            logCount--;
            PlayerPrefs.SetInt("HasSpear",1);

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
    }
}
