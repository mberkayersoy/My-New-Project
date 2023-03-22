using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public int teamID;
    public float abilityTime;
    public GameObject abilityPrefab;


    public void GiveAbility(Ability ability)
    {

    }
    // Start is called before the first frame update


    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        GameManager.Instance.GiveAbilityToPlayer(other.gameObject);
    //        Debug.Log("Picked");
    //        Destroy(gameObject);
    //    }
    //}
}
