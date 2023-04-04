using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Ability ability;

    private void Start()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.down * 20f, ForceMode.Impulse);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerAttribute>().SetAbility(ability);
            Destroy(gameObject);
        }
    }
}
