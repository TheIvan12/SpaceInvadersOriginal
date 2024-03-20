using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPlayerBullet : MonoBehaviour
{

    public float speed = 3f; // velocidad de la bala 

    [HideInInspector]
    public SPlayer player;


    public bool canShoot = false;

    public GameObject bulletExplosion;
    //public bool canShoot = false;

  

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, speed, 0) * Time.deltaTime;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "SBorde") //Choca con un objeto con el tag SBarrier

        {
            Destroy(this.gameObject); // se destruye este objeto. this = este, gameObject = objeto 
            Instantiate(bulletExplosion, transform.position, Quaternion.identity); // crea efecto de explosion de bala a partir de prefab

        }
    

        else if (collision.tag == "SBarrier")
        {
            Destroy(gameObject);
            Destroy(collision.gameObject); // Destruyo barrera
            Instantiate(bulletExplosion, transform.position, Quaternion.identity); // crea efecto de explosion de bala a partir de prefab en la posicion del padre

        }

        else if (collision.tag == "SBarreraDisparoJugador")
        {
            Destroy(gameObject);
            Instantiate(bulletExplosion, transform.position, Quaternion.identity);
        }
    }

    private void OnDestroy()
   
    {
        SPlayer player = FindAnyObjectByType<SPlayer>();


        // El jugador puede volver a disparar 
        if (player != null)

        {
            player.canShoot = true;
           // Debug.Log("puede disparar");
        }

        //else Debug.Log("no funciona");
    }

   
   
}
