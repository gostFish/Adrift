using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroAnimation : MonoBehaviour
{
    public GameObject fadeObj;
    public CanvasGroup fadeUI;

    public GameObject sceneManager;
    public GameObject mainCamera;
    public Texture[] frames;
    public float changeTiming;
    public float fadeTiming;
    public float time;
    //public float fadeTime;

    private AudioSource audioSource;
    public AudioClip thunder;
    private bool played;

    public int lerpVal; //For testing only

    void Start()
    {
        played = false;
        time = 2;
        //fadeTime = 0;
        audioSource = mainCamera.GetComponent<AudioSource>();
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
        {
            Skip();
        }
    }

    void FixedUpdate()
    {
        time = time + Time.deltaTime;
        //fadeTime = fadeTime + Time.deltaTime / (fadeTiming);
        
        //Debug.Log("time: " + time + " fadeTime: " + fadeTime + " Lerp: " + Mathf.Lerp(0, 255, time/fadeTiming));
        switch (time)
        {
            case float time when time < changeTiming:
                //Debug.Log("Here");
                //fadeObj.SetActive(true);

                GetComponent<RawImage>().texture = frames[0];

                if (time > (changeTiming - fadeTiming))
                {
                    fadeUI.alpha = Mathf.Lerp(0, 1, (time - (changeTiming - fadeTiming)) / fadeTiming);
                }
                break;
            case float time when time < (changeTiming * 2):
                if (time < (changeTiming + fadeTiming))
                {
                    fadeUI.alpha = Mathf.Lerp(1, 0, ((time - (changeTiming)) / fadeTiming));
                }
                GetComponent<RawImage>().texture = frames[1];
                if (time > ((changeTiming*2) - fadeTiming))
                {
                    fadeUI.alpha = Mathf.Lerp(0, 1, (time - ((changeTiming*2) - fadeTiming)) / fadeTiming);
                }
                break;
            case float time when time < (changeTiming * 3):
                if (time < ((changeTiming*2) + fadeTiming))
                {
                    fadeUI.alpha = Mathf.Lerp(1, 0, ((time - (changeTiming * 2)) / fadeTiming));
                }
                GetComponent<RawImage>().texture = frames[2];
                if (time > ((changeTiming * 3) - fadeTiming))
                {
                    fadeUI.alpha = Mathf.Lerp(0, 1, (time - ((changeTiming * 3) - fadeTiming)) / fadeTiming);
                }
                break;
            case float time when time < (changeTiming * 4):
                if (time < ((changeTiming * 3) + fadeTiming))
                {
                    fadeUI.alpha = Mathf.Lerp(1, 0, ((time - (changeTiming * 3)) / fadeTiming));
                }
                GetComponent<RawImage>().texture = frames[3];
                if (time > ((changeTiming * 4) - fadeTiming))
                {
                    time = time + 2;
                }
                break;
            case float time when time < (changeTiming * 5):
                
                switch (time)
                {
                    case float time2 when time < (changeTiming * 4+0.03):
                        fadeUI.alpha = 0;
                        if (!played)
                        {
                            audioSource.PlayOneShot(thunder);
                            played = true;
                        }
                        GetComponent<RawImage>().texture = frames[7];
                        break;
                    case float time2 when time < (changeTiming * 4) + 0.3:
                        GetComponent<RawImage>().texture = frames[4];
                        break;
                    case float time2 when time < (changeTiming * 4) + 0.6:
                        GetComponent<RawImage>().texture = frames[7];
                        break;
                    case float time2 when time < (changeTiming * 4) + 0.9:
                        GetComponent<RawImage>().texture = frames[4];
                        break;
                    case float time2 when time < (changeTiming * 4) + 1.2:
                        GetComponent<RawImage>().texture = frames[7];
                        break;
                    case float time2 when time < (changeTiming * 4) + 1.5:
                        GetComponent<RawImage>().texture = frames[4];
                        break;
                }
                if (time > ((changeTiming * 5) - fadeTiming))
                {
                    fadeUI.alpha = Mathf.Lerp(0, 1, (time - ((changeTiming * 5) - fadeTiming)) / fadeTiming);
                }
                break;
            case float time when time < (changeTiming * 6):
                if (time < ((changeTiming * 5) + fadeTiming))
                {
                    fadeUI.alpha = Mathf.Lerp(1, 0, ((time - (changeTiming * 5)) / fadeTiming));
                }
                GetComponent<RawImage>().texture = frames[5];
                if (time > ((changeTiming * 6) - fadeTiming))
                {
                    fadeUI.alpha = Mathf.Lerp(0, 1, (time - ((changeTiming * 6) - fadeTiming)) / fadeTiming);
                }
                break;
            case float time when time < (changeTiming * 7):
                if (time < ((changeTiming * 6) + fadeTiming))
                {
                    fadeUI.alpha = Mathf.Lerp(1, 0, ((time - (changeTiming * 6)) / fadeTiming));
                }
                GetComponent<RawImage>().texture = frames[6];
                if (time > ((changeTiming * 7) - fadeTiming))
                {
                    fadeUI.alpha = Mathf.Lerp(0, 1, (time - ((changeTiming * 7) - fadeTiming)) / fadeTiming);
                }
                break;
            case float time when time< (changeTiming* 9):
                //fadeObj.SetActive(false);
                SceneManager.LoadScene("Level");
                break;

        }
    }
    public void StartCutscene()
    {
        time = 2;
    }
    public void Skip()
    {
        gameObject.SetActive(false);
        SceneManager.LoadScene("Level");
    }
}
