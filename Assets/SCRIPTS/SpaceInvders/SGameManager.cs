using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SGameManager : MonoBehaviour
{
    // lista doble (matriz) de SInvaders - DECLARACION
    public SInvander[,] matrizAliens;

    // nº de filas de invaders,alto
    public int nFilas = 5;
    // nº de columnas de invaders, ancho
    public int nColumnas = 11;

    // Prefab de alien 
    public GameObject alien1Prefab;
    public GameObject alien2Prefab;
    public GameObject alien3Prefab;

    // game object padre de los aliens
    public SinvaderMovement padreAliens;

    //Distancia entre aliens al spawnearlos
    public float distanciaAliens = 1f;

    // Timepo entre disparos de los aliens
    public float tiempoEntreDisparos = 2f;


    // CICLO DEL JUEGO
    public bool gameOver = false; // bool de fin de la partida 

    // Vidas actuales del jugador 
    public int vidas = 3;

    // Puntuacion actual del jugador
    public int score = 0;

    // nº de aliens derrotados 
    public int defeatedAliens = 0;

    // Tiempo que dura la animacion 
    public float playerDamageDelay = 1.5f;

    // Multiplicador que aumenta la velocidad de los aliens 
    public float incVel = 3f;

    //SINGLETONE
    public static SGameManager instance = null;

    //Interfaz

    public TextMeshPro scoreText;  // Texto de la puntuacion
    public TextMeshPro lifesText;  //Texto con x vidas
    public TextMeshPro highScoreText; // texto puntuacion maxima

    public GameObject spritevida3; // Sprites de las vidas del jugador
    public GameObject spritevida2; // Sprites de las vidas del jugador

    //REFERENCIA AL JUGADOR 
    private SPlayer player;

    // Ovnis Variables

    public GameObject PrefabOVNI;
    public Transform spawnIzqOvni; //spawn izquieda
    public Transform spawnDerOvni; //spawn Derecha
    public float spawnOVNITime = 15f; // Tiempo que tarda un OVNI en aparecer

    public int highScore = 0;

    public AudioClip AlienDestroyedSFX;
    public AudioClip OvniSFX;
    public AudioClip Music;


    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

    }


    void Start()
    {
        SoundManager.instance.playMusic(Music);
        // Busco un objeto del tipo SPlayer
        player = FindObjectOfType<SPlayer>();   

        // Decidimos que matrizAliensa es una nueva matris de SInvaders de nColumnas x nFilas
        // - INICIALIZACIÓN
        matrizAliens = new SInvander[nColumnas, nFilas];
        SpawnAliens();

        InvokeRepeating("SelectAlienShoot", tiempoEntreDisparos, tiempoEntreDisparos);
        InvokeRepeating("SpawnOVNI", spawnOVNITime, spawnOVNITime);

        // Saco la puntuacion maxima guardada en el archivo PlayerPrefbs
        highScore = PlayerPrefs.GetInt("HIGH-SCORE");
        highScoreText.text = "HI-SCORE \n" + highScore.ToString();

    }

   
    void Update()
    {
        
    }

    void SpawnAliens()
    {

        // Doble bucle (anidado) que recorre la matriz de (11 x 5, rangos 0-10 y 0-4)
        for (int i = 0; i < nColumnas; i++)
        {
            for (int j = 0; j < nFilas; j++)
            {
                GameObject Prefab; // prefab del alien que spawneamos 

                if (j == nFilas - 1) Prefab = alien1Prefab;  // la ultima fila
                else if (j < 2) Prefab = alien3Prefab; // dos primeras filas
                else Prefab = alien2Prefab; // resto de filas


                // Dentro de los dos bucles, instanciamos un alien
                SInvander auxAlien = Instantiate(Prefab, padreAliens.gameObject.transform).GetComponent<SInvander>();

                // Lo guardamos en la posición de la matriz apropiada
                matrizAliens[i,j] = auxAlien;

                // Colocamos el alien 
                auxAlien.transform.position += new Vector3(i-nColumnas/2,j - nFilas /2,0) * distanciaAliens;

                auxAlien.padre = padreAliens;

            }
        }
       
    }

    public void SpawnOVNI()
    {
        int random = Random.Range(0 , 2);
        SoundManager.instance.playSFX(OvniSFX);

        if(random == 0 ) 
        Instantiate(PrefabOVNI, spawnIzqOvni).GetComponent<SOvni>().dir = 1;

        else if(random == 1) 
        Instantiate(PrefabOVNI,spawnDerOvni).GetComponent <SOvni>().dir = -1;

        
    }

    private void SelectAlienShoot()

    {
        bool encontrado = false; // variable tipo bool que controla si se ha encontrado a un alien o no

        while(!encontrado) // Se repite con columnas aleatorias 
        {
            // 1 - Elegi una columna aleatoria que no esté vacía 
            int randomCol = Random.Range(0 , nColumnas); // Columna aleatoria

            // 2 - Buscar al alien más cercano al jugador en esa columna 
            // En este for, tenemos dos condiciones, que j > -1 y que encontrado == false
            // Como usamos el operador && entre ellas, deben cumplirse las dos o salimos del bucle for 
            for (int j = 0; j < nFilas && !encontrado; j++)
            {
                if (matrizAliens[randomCol,j] != null) // si la casilla no esta vacía (null), el alien sigue vivo
                {
                    // Si encuentro un alien vivo, es el más cercano de la columna al jugador 
                    // porque la estoy recorriendo de abajo a arriba 
                    matrizAliens[randomCol, j].Shoot(); // Alien dispara 
                    encontrado = true; // He acabado la busqueda 
                }
            }

        }
    }

    // Metodo que se llama cuando perdemos la partida ( 0 vidas  o los alien llegan a abajo)
    public void PlayerGameOver()
    {
        UpdateHighScore();
        SoundManager.instance.stopMusic();
        gameOver = true;
        //Debug.Log("Has Perdido");
        CancelInvoke(); // Se interrumpen todos los invokes de este script
        SetInvadersAnim(false);

        SceneManager.LoadScene("GameOver");

    }

    // Metodo que se llama cuando ganamos la partida
    public void PlayerWin()
    {
        UpdateHighScore();
        SoundManager.instance.stopMusic();
        gameOver = true;
       //Debug.Log("Has Ganado");
        CancelInvoke();
        SceneManager.LoadScene("WinScreen");



    }

    public void DamagePlayer()
    {
        if (!gameOver && player.GetCanMove())
        {


            vidas--;
            UpdateLifeUI();

            //animacion jugador, daño
          
            player.PlayerDamaged();
            padreAliens.canMove = false; // Bloqueo los aliens
           
            Invoke("UnlockDamagePlayer", playerDamageDelay);

            if (vidas <= 0)
            {
                PlayerGameOver();
            }

        }

    }

    private void UnlockDamagePlayer()
    {
        player.PlayerReset(); // Desbloqueo al jugador
        padreAliens.canMove = true; // Desbloqueo a los aliens 
        SetInvadersAnim(true);
    }

    private void UpdateLifeUI()
    {
        // Actualiza el texto

        lifesText.text = vidas.ToString();

        //Actualiza los sprites de las vidas

        spritevida3.SetActive(vidas >= 2); //Se Activa si vidas >=2
        spritevida2.SetActive(vidas >= 3); //Se activa si vidas >=3

    }

    public void AlienDestroyed()
    {
        SoundManager.instance.playSFX(AlienDestroyedSFX);

        defeatedAliens++; // Aumento de la cuenta de aliens derrotados 

        //Actualizar velocidad de los aliens segun cuantos quedan 

        // (1 + (alienDerrotados / alienTotales) * (incVelocidad - 1) * speedAliens

        // Suma incrVelocidad / alienTotales
        padreAliens.speed += ( incVel / (float)(nFilas * nColumnas)) * incVel;


        if(defeatedAliens >= nFilas*nColumnas)
        {
            PlayerWin();  // El jugador gana
        }

    }

    public void ResetGame()
    {
        UpdateHighScore();
        SceneManager.LoadScene("SpaceInvaders");
    }
    public void GameOver()
    {
        UpdateHighScore();
        
        SoundManager.instance.stopMusic();
        SceneManager.LoadScene("GameOver");
    }

    public void PlayerWinGame()
    {
        UpdateHighScore();
        SoundManager.instance.stopMusic();
        SceneManager.LoadScene("WinScreen");
    }

  


    public void UpdateHighScore()
    {
        if (score > highScore) // si mi puntuacion es mayor que la maxima 
        {

            PlayerPrefs.SetInt("HIGH-SCORE", score); // la guardo 

        }
    }

    private void OnApplicationQuit()
    {
        UpdateHighScore(); // Si cerramos la aplicacion guardamos la puntuación
    }

    //suma puntos a la puntiacion
    public void AddScore(int points)
    {
        score += points;

        // Actualizar texto puntos 
        scoreText.text = "SCORE\n" + score.ToString();
        //SetInvadersAnim(true); // Duda
       // SetInvadersAnim(false); // Duda
    }


    // Recorre la lista de aliens, y les pone la animacion indicada
    public void SetInvadersAnim(bool movement)

    {
        for(int i = nColumnas; i < 0; i++)
        
        {
           for (int j = nFilas; j < 0; j++)
           
            {
                if (matrizAliens[i, j] != null)  // recorremos la matriz , si un alien da valor de null, no hacemos los if 
                {

                    //Asignamos la animacion que toque 
                    if (movement)
                        matrizAliens[i, j].MovementAnimation();

                    else
                        matrizAliens[i, j].MovementAnimation();
                }
            }
        
        }
    }

}
