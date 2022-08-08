using angulargame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testSeekingEnemy : MonoBehaviour
{
    // Target speed and damage tick
    public float speed = 10;
    public float damageTick = 1.0f;
    private Transform target;
    private Collider mainBody;

    //public Vector3 offset;
    //[Range(5, 20)] public float camSmoothness;


    void Start()
    {
        target = GameObject.Find("PlayerCube").transform;
        //mainBody = this.GetComponent<Collider>();

    }
    void Update()
    {
        if (target != null)
        {
            Follow();
        }
    }

    void Follow()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards( transform.position, target.position, step);
    }

    public void OnTriggerEnter(Collider other)
    {
        print("Collision Detected");
        //StartCoroutine(contactDamage(other));
        StartCoroutine(contactDamage(other));

        //if (other.tag == "Player")
        //{

        //    other.GetComponent<PlatformController>().reducePlayerHealth(1);
        //    yield return new WaitForSeconds(damageTick);
        //}
    }

    private IEnumerator contactDamage(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlatformController>().reducePlayerHealth(1);
            //yield return new WaitForSeconds(damageTick);
        }

        yield return null;
    }
}
