using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpeedAbility : Ability
{
    float increaseSpeed = 30;
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        Destroy(gameObject);
    //    }
    //}
    public override void GiveAbility(Player player)
    {
        owner = player;
        owner.GetComponentInChildren<PersonalCanvas>().StartAbilityCountDown(abilityTime);
        player.speedForce += increaseSpeed;
    }

    public override void TakeAbility()
    {
        owner.speedForce -= increaseSpeed;
        Debug.Log("Take");
    }
}
