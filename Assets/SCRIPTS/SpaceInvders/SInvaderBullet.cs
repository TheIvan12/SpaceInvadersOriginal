using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SInvaderBullet : MonoBehaviour
{
    public float speed = 3;

    public GameObject bulletExplosion;



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, -speed, 0) * Time.deltaTime;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    
    {
        if (collision.tag == "Ssuelo") //Choca con un objeto con el tag SSuelo

        {
            Destroy(this.gameObject); // se destruye este objeto. this = este, gameObject = objeto 
            Instantiate(bulletExplosion, transform.position, Quaternion.identity); // crea efecto de explosion de bala a partir de prefab

        }
        else if (collision.tag == "SPlayer")
        {
            SGameManager.instance.DamagePlayer();
            Destroy(gameObject);
            //Destroy(collision.gameObject);
            Instantiate(bulletExplosion, transform.position, Quaternion.identity); // crea efecto de explosion de bala a partir de prefab

        }

        else if (collision.tag == "SPlayerBullet")
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
            Instantiate(bulletExplosion,transform.position, Quaternion.identity);
        }

        else if (collision.tag == "SBarrier")
        {
            Destroy(gameObject);
            Destroy(collision.gameObject); // Destruyo barrera
            Instantiate(bulletExplosion, transform.position, Quaternion.identity); // crea efecto de explosion de bala a partir de prefab
        }
    }




}
