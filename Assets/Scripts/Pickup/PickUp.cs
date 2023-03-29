using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityType
{
    Speed,
    Jump,
}
public class PickUp : MonoBehaviour
{
    public AbilityType type;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
            if (type == AbilityType.Speed)
            {
                other.GetComponent<Player>().SetAbility(new SpeedAbility());
            }
            else if (type == AbilityType.Jump)
            {
                other.GetComponent<Player>().SetAbility(new JumpAbility());
            }
        }
    }
}
