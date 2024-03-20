using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPlayer : MonoBehaviour
{
    [Tooltip("Prefab del la bala")]
    public GameObject prefabBullet; // Prefab de bala 

    [Tooltip("Velocidad del jugador en unidades de unity / segundo")]
    public float speed = 2f; // Velocidaad Del jugador

    // Teclas para input configurable 
    public KeyCode shootKey = KeyCode.Space;
    public KeyCode moveLeftKey = KeyCode.A;
    public KeyCode moveRightKey = KeyCode.D;

    public Transform PosDisparo;

    public bool canShoot = true; //booleano de si puede o no puede disparar
    private bool canMove = true;

    // Animator del jugador 
    private Animator pAnimator;
    private Vector3 posInicial;

    // valores limites del desplazamiento lateral
    private float LimiteIZq = -7.8f;
    private float LimiteDerch = 7.7f;

    public AudioClip ShootSFX;
    public AudioClip DeathSFX;
   


    // Start is called before the first frame update
    void Start()
    {
        posInicial = transform.position;
        pAnimator = GetComponent<Animator>();
       
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            InputPlayer();
        }
    }

    private void InputPlayer()
    {
        if (canShoot && Input.GetKeyDown(shootKey))
        {
            // Disparo
            Shoot();
        }

        else if (Input.GetKey(moveLeftKey))
        {
            // voy a  la izquierda 
            transform.position += new Vector3(-speed, 0, 0) * Time.deltaTime;
           
            if(transform.position.x < LimiteIZq)
            {
                Vector3 aux = transform.position;
                aux.x = LimiteIZq;
                transform.position = aux;
            }
        }

        else if (Input.GetKey(moveRightKey))
        {
            // voy a la derecha
            transform.position += new Vector3(speed, 0, 0) * Time.deltaTime;

            if (transform.position.x > LimiteDerch)
            {
                Vector3 aux = transform.position;
                aux.x = LimiteDerch;
                transform.position = aux;
            }
        }
    }

    private void Shoot()
    {
        GameObject aux = Instantiate(prefabBullet, PosDisparo.position, Quaternion.identity); // invocamos una bala y le asignamos su posicion y rotacion
        SPlayerBullet bullet = aux.GetComponent<SPlayerBullet>();
        bullet.player = this;

        Debug.Log("Dispara");

        SoundManager.instance.playSFX(ShootSFX);

        canShoot = false;
    }

    public void PlayerDamaged()
    {
        SoundManager.instance.playSFX(DeathSFX);
        pAnimator.Play("PLAYER_DEATH");
        canMove = false;
    }

    public void PlayerReset()
    {
        pAnimator.Play("PLAYER_IDLE");
        canMove = true;
        transform.position = posInicial;     
    }

    public bool GetCanMove()
    {
        return canMove;
    }

    public void SetCanMove(bool b)
    { canMove = b; }
}
