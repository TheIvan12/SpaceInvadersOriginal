using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public enum InvaderType {SQUID,CRAB,OCTOPUS};

public class SInvander : MonoBehaviour
{
    public InvaderType tipo = InvaderType.SQUID;

    public GameObject ParticulaMuerte;
    public bool isQuiting = false;
    public SinvaderMovement padre;

    public GameObject invaderBullet;

    public float bulletSpawnYOffset = -0.5f;

    public static int  speed = 3;

    public int puntosGanados = 10;

    private Animator animator;

  
    void Start()
    {
        animator = GetComponent<Animator>();
       
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void Shoot()
   
    {      
        Vector3 aux = transform.position + new Vector3(0,bulletSpawnYOffset,0); 
        Instantiate(invaderBullet,aux, Quaternion.identity);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "SBorde") // Choca con borde de pantalla
        {
            // Llamar a switchDirection para que se gire el padre 
            padre.SwitchDirection();
        }

        else if (collision.tag == "SGameOverBarrier") 
        {
            SGameManager.instance.PlayerGameOver(); // fin del juego 
            Debug.Log("loser");
            
        }

        else if (collision.tag == "SPlayerBullet")
        {
           
            SGameManager.instance.AlienDestroyed();


            GameObject particula = Instantiate(ParticulaMuerte, transform.position, Quaternion.identity);

            // Stun a los aliens (Movimiento)
            padre.AlienDestroyedStun();



            //Suma puntos en el marcador

            SGameManager.instance.AddScore(puntosGanados);

            Destroy(collision.gameObject);
            Destroy(gameObject);
        }


    }

  

    public void MovementAnimation()
    {
        if (tipo == InvaderType.SQUID)
            animator.Play("ALIEN_1_IDLE");
        else if (tipo == InvaderType.CRAB)
            animator.Play("ALIEN_2_IDLE");
        else if (tipo == InvaderType.OCTOPUS)
            animator.Play("ALIEN_3_IDLE");
    }

    public void StunAnimation()
    {
        if (tipo == InvaderType.SQUID)
            animator.Play("ALIEN_1_STUN");
        else if (tipo == InvaderType.CRAB)
            animator.Play("ALIEN_2_STUN");
        else if (tipo == InvaderType.OCTOPUS)
            animator.Play("ALIEN_3_STUN");
    }
}
