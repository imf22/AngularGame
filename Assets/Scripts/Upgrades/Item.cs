using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace angulargame
{
    public abstract class Item : MonoBehaviour
    {
        string Name = "";
        public AudioSource audioSource;

        Coroutine Disappear;

        private void Start()
        {
            // Item will disappear after 1 minute
            Destroy(this.gameObject, 60f);
        }

        private void Update()
        {
            rotate();

        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag( "Player"))
            {
                this.GetComponent<Collider>().enabled = false;
                this.GetComponent<Renderer>().enabled = false;

                itemUpgrade(other.gameObject);

                Destroy(this.gameObject, 5f);
            }

        }

        public void playSFX() => audioSource.Play();

        public virtual void itemUpgrade(GameObject other)
        {
            throw new NotImplementedException();
        }

        public virtual void rotate()
        {
            transform.localRotation = Quaternion.Euler(0, Time.time * 100f, 0);
        }
        
    }
}
