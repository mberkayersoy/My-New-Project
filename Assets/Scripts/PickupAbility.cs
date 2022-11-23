using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAbility : MonoBehaviour
{
    [SerializeField] Ability ability = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //other.GetComponent<>().EquipAbility();
            Destroy(gameObject);
        }
    }
    
}
