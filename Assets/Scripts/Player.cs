using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    


}

