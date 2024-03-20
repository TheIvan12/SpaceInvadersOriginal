using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GGameOVer : MonoBehaviour
{
    public AudioClip ClickSFX;
    public AudioClip Music;
    // Start is called before the first frame update
    void Start()
    {
        GSoundManager.instance.playMusic(Music);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Retry()
    {
        GSoundManager.instance.stopMusic();
        GSoundManager.instance.playSFX(ClickSFX);
        SceneManager.LoadScene("SpaceInvaders");
       
    }

    public void Exit()
    {
        GSoundManager.instance.playSFX(ClickSFX);
        Application.Quit();
    }
}
