using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class testTurrentEnemy : MonoBehaviour
{
    public Transform[] muzzelpoint;
    public GameObject projectile;

    public float shotPower = 200f;
    public float firerate = 0.5f;
    public float projectileLifetime = 1.0f;
    public float projectileScale = 2;
    
    public bool ShootLeft = true;
    public bool ShootRight = true;
    public bool ShootUp = true;
    public bool ShootDown = true;

    float shotcountdown;

    //public Transform playerCheck;
    public float enemySightRange = 10f;
    public LayerMask playerMask;
    public bool _detectPlayer;

    //Private
    private bool[] ULDRFiring; // UPDATE IDEA MAKE GAMEOBJECT ARRAY TO ALLOW FOR CONTROLL OVER CHILD AND OTHER ASPECTS


    // Start is called before the first frame update
    void Start()
    {
        shotcountdown = 0f;

        // Set Turrent Fring directions
        ULDRFiring = new bool[4];
        ULDRFiring[0] = ShootUp ;
        ULDRFiring[1] = ShootLeft;
        ULDRFiring[2] = ShootDown;
        ULDRFiring[3] = ShootRight;
    }

    // Update is called once per frame
    void Update()
    {
        DetectPlayer();
        fireProjectile();
    }

    private void DetectPlayer()
    {
        // Determine if character is on ground for jump
        if (Physics.OverlapBox(this.transform.position, new Vector3(1, 1, enemySightRange), this.transform.rotation, playerMask).Length > 0 || 
            Physics.OverlapBox(this.transform.position, new Vector3(1, enemySightRange, 1), this.transform.rotation, playerMask).Length > 0 )
        {
            _detectPlayer = true;
        }
        else
        {
            _detectPlayer = false;
        }

    }


    // Controlls creation of new projectiles on each shot
    private void fireProjectile()
    {
        // Reduces cooldown per frame
        shotcountdown -= Time.deltaTime;

        // Checks if mouse down and cooldown is refreshed
        if (_detectPlayer && shotcountdown <= 0f)
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
            // Reset shotcountdown
            shotcountdown = firerate;
        }
    }
}
