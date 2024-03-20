using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinvaderMovement : MonoBehaviour
{                              
    public float speed = 2f;   // velocidad de movimiento
    private int dir = 1;

    public float despAbajo = 0.5f; // distancia que baja al cambiar de direccion

    public bool canSwitch = true; // bool que indica si puede girarse 
    public float switchDelay = 0.5f;  // tiempo que debe pasar despues de girar, para poder volver a hacerlo 

    private SGameManager gm;

    public bool canMove = true; // Bool que indica si puede moverse

    public float moveStunTime = 0.5f;

    [HideInInspector]
    public float originalSpeed = 3f; // Velocidad inicial de los aliens

    /*
     * 1 - Despues de girar, pongo can switch a false
     * 2 - Crear una funcion que ponga canSwitch a true 
     * 3 - a la vez que pongo canSwitch a false, hago invoke del metodo que cree antes, con tiempo switchDelay 
     */


    void Start()
    {
        gm = SGameManager.instance;
        originalSpeed = speed;
    }

   
    void Update()
    {
        if (!gm.gameOver && canMove)

        {
            Movement();
        }
    }

    private void Movement()
    {
        transform.position += new Vector3(speed, 0, 0) * dir * Time.deltaTime;
    }

    public void SwitchDirection()
   
    {

        if (canSwitch == true)
        {
            dir = dir *= -1; // invierto la direccion (1 y -1)

            // Desplazarme hacia abajo 
            transform.position += new Vector3(0, -despAbajo, 0);
            canSwitch = false;
            Invoke("EnableSwitch",switchDelay);
        }

        /* else if (canSwitch == false)
        {
            //Invoke("EnableSwitch", switchDelay);     <-------   CODIGO ERRONEO 
        }
        */

    }


    public void EnableSwitch()
  
    {
       
        canSwitch = true;   
    }

    public void EnableMovement()
    {
        canMove = true; // Activar movimiento
        gm.SetInvadersAnim(true); // poner animacion de movimiento
    }

    public void AlienDestroyedStun() // Metodo que se llama cuando se destruye un alien y que para su movimiento un tiempo
    { 

        canMove = false; // Paramos el movimiento
        gm.SetInvadersAnim(false); // desactivar animacion de movimiento o en su defecto de stun.
        Invoke("EnableMovement" , moveStunTime);// Reactivamos el movimiento tras un tiempo

    }
}
