using angulargame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyAI : MonoBehaviour
{
    // A basic enemy AI that can chase and shoot the player
    // Contact damage is enabled by default

    // Target speed and damage tick
    private GameObject target;
    public float speed = 5;
    public float damageTick = 1.0f;

    // Player detection components
    public float enemySightRange = 10f;
    public LayerMask playerMask;
    private bool _detectPlayer; // Player dection set to private

    // Default Blaster Components: Fires all directions
    public Transform[] muzzelpoint;
    public GameObject projectile;
    public float cooldown;

    public float shotPower = 100f;
    public float firerate = 1.0f;
    public float projectileLifetime = 1.0f;
    public float projectileScale = 2;

    float actionCountdown;

    // Start is called before the first frame update
    void Start()
    {
        // Set action ready
        actionCountdown = 0f;
        initialPlayerSearch();

        //target = GameObject.Find("PlayerCube");
        //DetectPlayer();
        //StartCoroutine(StartEnemyAI());

    }

    //Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            DetectPlayer();
            if (!_detectPlayer)
            {
                Follow();
            }

            // Will auto fire once detected
            OnDetect();

        }
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
            actionCountdown = firerate;
        }
    }

    private void EnemyAction()
    {
        // Fire projectiles

        for (int i = 0; i < muzzelpoint.Length; i++)
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
        // Reset shotcountdown
        actionCountdown = firerate;
    }

    private void DetectPlayer()
    {
        // Determine if player is in range
        if (Physics.OverlapSphere(this.transform.position, enemySightRange, playerMask).Length > 0)
        {
            _detectPlayer = true;
        }
        else
        {
            _detectPlayer = false;
        }

    }

    void Follow()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
    }

    public void OnTriggerEnter(Collider other)
    {
        StartCoroutine(contactDamage(other));
    }

    private IEnumerator contactDamage(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlatformController>().reducePlayerHealth(1);
            //TODO : Add SFX and VFX
            yield return new WaitForSeconds(damageTick);
        }

        yield return null;
    }

    private void initialPlayerSearch()
    {
        // Searches for player and if found sets as AI target;
        Collider[] initSearch = Physics.OverlapSphere(this.transform.position, 100, playerMask);
        if (initSearch.Length > 0) target = initSearch[0].gameObject;
    }
    private IEnumerator StartEnemyAI()
    {
        // Initial Player Search
        initialPlayerSearch();

        while(target != null)
        {
            DetectPlayer();
            if (!_detectPlayer)
            {
                Follow();
            }

            // Will auto fire once detected
            OnDetect();
        }

        yield return new WaitForSeconds(0.1f);
    }
}
