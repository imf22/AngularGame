using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace angulargame
{
    public class FireProjectile : MonoBehaviour
    {
        public Transform[] muzzelpoint;
        public GameObject projectile;
        public AudioSource firingSound;

        public float shotPower = 400f;
        public float firerate = 0.5f;
        public float projectileLifetime = 5.0f;
        public float projectileScale = 1;

        private bool ShootLeft = true;
        private bool ShootRight = true;
        private bool ShootUp = true;
        private bool ShootDown = true;
        


        float shotcountdown;

        public bool[] ULDRFiring;

        // Start is called before the first frame update
        void Start()
        {
            shotcountdown = 0f;
        }

        // Update is called once per frame
        void Update()
        {
            fireProjectile();

            // Set Fring directions
            ULDRFiring = new bool[4];
            ULDRFiring[0] = ShootUp;
            ULDRFiring[1] = ShootLeft;
            ULDRFiring[2] = ShootDown;
            ULDRFiring[3] = ShootRight;
        }


        // Controlls creation of new projectiles on each shot
        private void fireProjectile()
        {
            // Reduces cooldown per frame
            shotcountdown -= Time.deltaTime;

            // Checks if mouse down and cooldown is refreshed
            if (VirtualInputManager.Instance.Shoot && shotcountdown <= 0f)
            {
                for (int i = 0; i < muzzelpoint.Length; i++)
                {
                    if (ULDRFiring[i])
                    {
                        // Instantiate projectile
                        GameObject currentProjectile = (GameObject)Instantiate(projectile, muzzelpoint[i].position, muzzelpoint[i].rotation);

                        // Set scale
                        currentProjectile.transform.localScale = currentProjectile.transform.localScale * projectileScale;

                        // Add force to projectile
                        currentProjectile.GetComponent<Rigidbody>().AddForce(muzzelpoint[i].up * shotPower * 10);

                        // Destroy Projectile at end of its lifetime
                        Destroy(currentProjectile, projectileLifetime);
                    }

                    
                    
                }

                firingSound.Play();

                // Reset shotcountdown
                shotcountdown = firerate;
            }
        }
    }
}
