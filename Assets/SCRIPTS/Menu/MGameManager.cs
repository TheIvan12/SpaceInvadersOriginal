using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MGameManager : MonoBehaviour
{

    public AudioClip ClickSFX;
    public AudioClip Music;
    // Start is called before the first frame update
    void Start()
    {
        MSoundManage.instance.playMusic(Music);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EscenaJuego()
    {
        MSoundManage.instance.stopMusic();
        MSoundManage.instance.playSFX(ClickSFX);
        SceneManager.LoadScene("SpaceInvaders");

       
    }

    public void SalirJuego()
    {
        MSoundManage.instance.playSFX(ClickSFX);
       // Debug.Log("Saliendo del juego");
        Application.Quit();
    }

  
}
