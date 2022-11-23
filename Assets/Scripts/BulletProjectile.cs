using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    private Rigidbody bulletRigidbody;
    [SerializeField] private Transform vfxHitGreen;
    [SerializeField] private Transform vfxHitRed;
    public float speed = 250f;

    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        bulletRigidbody.velocity = transform.forward * speed;
        //bulletRigidbody.AddForce(new Vector3(0,0,1) * speed - bulletRigidbody.velocity, ForceMode.VelocityChange);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        //if (other.gameObject.tag == "Ball")
        //{
        //    other.GetComponent<Ball>().Split();
        //}
        //Destroy(gameObject);
        if (other.gameObject.tag == "Ball")
        {
            // hit target
            other.GetComponent<Ball>().Split(1);
            Instantiate(vfxHitGreen, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else
        {
            Instantiate(vfxHitRed, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log(collision.gameObject.name);
    //    if (collision.gameObject.tag == "Ball")
    //    {
    //        //hit target
    //        collision.gameObject.GetComponent<Ball>().Split(1);
    //        Instantiate(vfxHitGreen, transform.position, Quaternion.identity);
    //        Destroy(gameObject);
    //    }
    //    else
    //    {
    //        Instantiate(vfxHitGreen, transform.position, Quaternion.identity);
    //        Destroy(gameObject);
    //    }
    //}
}
