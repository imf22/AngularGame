using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    public GameObject Explosion;
    public AudioClip DestroySFX;
    
    private AudioSource _audioSource;
    private Rigidbody _rb;


    // Start is called before the first frame update
    void Start()
    {
        _rb = this.GetComponent<Rigidbody>();
        GameObject GlobalAudio = GameObject.Find("GlobalAudioSource0");
        _audioSource = GlobalAudio.GetComponent<AudioSource>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            GameObject explosion = Instantiate(Explosion) as GameObject;
            other.GetComponent<Collider>().enabled = false;
            explosion.transform.position = transform.position;
            _audioSource.PlayOneShot(DestroySFX);

            // Destroy Enemy, create Xplosion, and destory projectile
            other.transform.parent.gameObject.GetComponent<BasicEnemyAI>().destorySelfScoring();
            Destroy(explosion, 2.0f);
            Destroy(this.gameObject);
        }
        if (other.tag == "EProjectile")
        {
            GameObject explosion = Instantiate(Explosion) as GameObject;
            explosion.transform.position = transform.position;
            explosion.transform.localScale = explosion.transform.localScale / 2;
            _audioSource.PlayOneShot(DestroySFX);

            Destroy(other.gameObject);
            Destroy(explosion, 2.0f);
            Destroy(this.gameObject);
        }
        else if (!other.CompareTag("Player"))
        {
           if (!other.CompareTag("PlayerFiller"))
            {
                if (!other.CompareTag("Projectile"))
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
