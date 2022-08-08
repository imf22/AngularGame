using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlankTurrentAI : MonoBehaviour
{
    // Player detection components
    public float enemySightRange = 10f;
    public LayerMask playerMask;
    public bool _detectPlayer; // Player dection set to private

    // Default Blaster Components
    public Transform muzzelpoint;
    public GameObject projectile;
    public float cooldown;

    float actionCountdown;

    // Start is called before the first frame update
    void Start()
    {
        actionCountdown = 0f;
        
    }

    // Update is called once per frame
    void Update()
    {
        DetectPlayer();
        OnDetect();
    }

    private void OnDetect()
    {
         // Reduces cooldown per frame
        actionCountdown -= Time.deltaTime;

        // Checks if mouse down and cooldown is refreshed
        if (_detectPlayer && actionCountdown <= 0f)
        {
            // Perform Action on Detect
            EnemyAction();

            // Reset shotcountdown
            actionCountdown = cooldown;
        }
    }

    private void EnemyAction()
    {
        // Enemy specific action
        throw new NotImplementedException();
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
}
