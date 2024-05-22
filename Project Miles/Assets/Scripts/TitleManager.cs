using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public GameObject titleManager;
    public int sceneNumber;
    public Animator _startFade;
    public bool starting;
   // public bool titleCard;
   // public int titleCardSecs;
   // public GameObject credits;
   // public GameObject instructions;
    public AudioSource start;
    void Start()
    {
        start = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(startGame());
        }
            _startFade.SetBool("starting", starting);
    }
    public void loadMain()
    {
        
    }
    public void Quit()
    {
        Application.Quit();
    }
    IEnumerator startGame()
    {
        yield return new WaitForSeconds(0.5f);
        starting = true;
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneNumber);
    }


}
