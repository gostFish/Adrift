using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public float time;
    public float scrollSpeed;
    public float startTime;
    public float stopTime;
    public GameObject cutscene;
    public GameObject credits;

    public GameObject skip1;
    public GameObject skip2;

    void Start()
    {
        time = 0;
        skip1.SetActive(true);
        skip2.SetActive(false);
    }
    private void FixedUpdate()
    {
        time = time + Time.deltaTime;
        if(time > startTime && time < stopTime)
        {
            cutscene.GetComponent<Animator>().enabled = false;
            credits.transform.position += new Vector3(0,(scrollSpeed * Time.deltaTime),0);
            skip1.SetActive(false);
            skip2.SetActive(true);
        }
        if(time > (stopTime + 3))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
    
    public void Skip1()
    {
        time = startTime;

    }
    public void Skip2()
    {
        time = stopTime + 3;
    }
}
