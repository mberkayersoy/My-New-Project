using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginBalls : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    void Start()
    {
        var randomVector = Random.insideUnitSphere * 50f;
        rb = GetComponent<Rigidbody>();
        rb.AddForce(randomVector, ForceMode.Impulse);
    }

}
