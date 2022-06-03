using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarryMusic : MonoBehaviour
{
    /*  void Start()
      {
          

      }*/
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
              
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        GameObject[] musicObj = GameObject.FindGameObjectsWithTag("MainMenuMusic");
        if (musicObj.Length > 1)
        {
            for (int i = 1; i < musicObj.Length; i++)
            {
                Destroy(musicObj[i]);
            }
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Loaded scene " + scene.name);
        //Debug.Log(mode);
        if (scene.name == "Level")
        {
            Destroy(this.gameObject);            
        }
    }
}
