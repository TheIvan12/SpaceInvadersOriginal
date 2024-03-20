using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOvni : MonoBehaviour
{
    public float speed = 3f; // Velocidad
    public int points = 100; // Puntos al derrotarlo
    public int dir = 1; // Direccion del ovni 1 derecha , -1 izquierda
    public float deathAnimTime = 1f; // Tiempo de la animacion de muerte

    public Animator animator;

   


    void Start()
    {
         animator = GetComponent<Animator>();
    }

    void Update()
    {
        transform.position += new Vector3(speed, 0, 0)* dir * Time.deltaTime;

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "SBorde") Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "SPlayerBullet")
        {
            Destroy(collision.gameObject);
            SGameManager.instance.AddScore(points); // Sumo puntos
            animator.Play("OVNI_Death");
            speed = 0;
            Destroy(gameObject,deathAnimTime); // Destruirlo
        }
    }


}
