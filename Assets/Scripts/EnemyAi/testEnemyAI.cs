using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class testEnemyAI : MonoBehaviour
{
    public Transform muzzelpoint;
    public GameObject projectile;
    public int currentWeapon = 0 ;

    public int numberOrigins = 1;
    public float shotPower = 200f;
    public float firerate = 0.5f;
    public float projectileLifetime = 1.0f;
    public float projectileScale = 2;


    //public Transform playerCheck;
    public float enemySightRange = 10f;
    public LayerMask playerMask;
    public bool _detectPlayer;


    float shotcountdown;  

    // Start is called before the first frame update
    void Start()
    {
        shotcountdown = 0f;
        
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
        if (Physics.OverlapBox(this.transform.position, new Vector3(1, 1, enemySightRange), this.transform.rotation, playerMask).Length > 0)
        {
;            //(groundCheck.position, groundCheckRadius, groundMask).Length > 0)
            _detectPlayer = true;
        }
        else
        {
            _detectPlayer = false;
        }

    }

    // Handles the swapping of current weapon using right click

    // Controlls creation of new projectiles on each shot
    private void fireProjectile()
    {
        shotcountdown -= Time.deltaTime;    

        // Checks if mouse down and cooldown is refreshed
        if (_detectPlayer && shotcountdown <= 0f)
        {
            // Reduces cooldown per frame

            for (int i = 0; i < numberOrigins; i++)
            {
                float offsetFloat = 360 / numberOrigins * i ;
                print(offsetFloat);
                Quaternion offset = Quaternion.Euler(offsetFloat , 0, 0);

                // Instantiate object
                //GameObject currentProjectile = (GameObject)Instantiate(projectile, this.transform.position + Vector3.forward , muzzelpoint.rotation * offset );
                GameObject currentProjectile = (GameObject)Instantiate(projectile, muzzelpoint.position, muzzelpoint.rotation);

                // Set scale
                currentProjectile.transform.localScale = currentProjectile.transform.localScale * projectileScale;

                // Add force to projectile
                currentProjectile.GetComponent<Rigidbody>().AddForce(muzzelpoint.up * shotPower * 10);

                // Destroy Projectile at end of its lifetime
                Destroy(currentProjectile, projectileLifetime);

            }

            // Reset shotcountdown
            shotcountdown = firerate;
        }
    }
}
