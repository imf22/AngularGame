using angulargame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private Rigidbody _rb;
    public GameObject Explosion;
    
    
    //: "PlatformController;

    // Start is called before the first frame update
    void Start()
    {
        _rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            GameObject explosion = Instantiate(Explosion) as GameObject;
            explosion.transform.position = transform.position;

            other.GetComponent<PlatformController>().reducePlayerHealth(1);

            Destroy(explosion);
            Destroy(this.gameObject);
        }
        if (other.tag == "Projectile")
        {
            GameObject explosion = Instantiate(Explosion) as GameObject;
            explosion.transform.position = transform.position;
            explosion.transform.localScale = explosion.transform.localScale / 4;
            Destroy(explosion);
            Destroy(this.gameObject);
        }
        
        else if(other.tag != "Enemy")
        {
            if (!other.CompareTag("PlayerFiller"))
            {
                Destroy(this.gameObject);
            }
        }

    }
}
