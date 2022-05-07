using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroAnimation : MonoBehaviour
{
    public GameObject sceneManager;
    public GameObject mainCamera;
    public Texture[] frames;
    public float changeTiming;
    public float fadeTiming;
    public float time;
    public float fadeTime;

    private AudioSource audioSource;
    public AudioClip thunder;
    private bool played;

    public int lerpVal; //For testing only

    void Start()
    {
        played = false;
        time = 0;
        fadeTime = 0;
        audioSource = mainCamera.GetComponent<AudioSource>();
    }
    void FixedUpdate()
    {
        time = time + Time.deltaTime;
        fadeTime = fadeTime + Time.deltaTime / (fadeTiming);
        
        //Debug.Log("time: " + time + " fadeTime: " + fadeTime + " Lerp: " + Mathf.Lerp(0, 255, time/fadeTiming));
        switch (time)
        {
            case float time when time < changeTiming:
                
                
                if(time < changeTiming - fadeTiming)
                {
                    GetComponent<RawImage>().texture = frames[0];
                }
                else
                {                    
                    //lerpVal = (int)(Mathf.Lerp(255, 0, (time - fadeTiming) / fadeTiming));
                    //Debug.Log("Lerp = " + lerpVal);
                    //GetComponent<RawImage>().color = new Color(255, 255, 255, lerpVal);
                }                
                    //GetComponent<RawImage>().color = new Color(255, 255, 255, Mathf.Lerp(255, 0, time  /  (changeTiming - fadeTime)));    
                break;
            case float time when time < (changeTiming * 2):
                GetComponent<RawImage>().texture = frames[1];
                break;
            case float time when time < (changeTiming * 3):
                GetComponent<RawImage>().texture = frames[2];
                break;
            case float time when time < (changeTiming * 4):
                GetComponent<RawImage>().texture = frames[3];
                break;
            case float time when time < (changeTiming * 5):
                
                switch (time)
                {
                    case float time2 when time < (changeTiming * 4+0.03):
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
                break;
            case float time when time < (changeTiming * 6):
                GetComponent<RawImage>().texture = frames[5];
                break;
            case float time when time < (changeTiming * 7):
                GetComponent<RawImage>().texture = frames[6];
                break;
            case float time when time< (changeTiming* 9):
                SceneManager.LoadScene("Level");
                break;

        }
    }
    public void StartCutscene()
    {
        time = 0;
    }
    public void Skip()
    {
        gameObject.SetActive(false);
        SceneManager.LoadScene("Level");
    }
}
