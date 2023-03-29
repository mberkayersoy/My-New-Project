using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JumpAbility : Ability
{
    float increaseJump = 10;

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
        owner.jumpForce += increaseJump;
    }

    public override void TakeAbility()
    {
        owner.jumpForce -= increaseJump;
        Debug.Log("Take");
    }
}
