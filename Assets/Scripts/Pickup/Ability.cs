using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AbilityType
{
    Speed,
    Jump,
    Jetpack
}

[CreateAssetMenu(fileName = "Ability", menuName = "Abilities/Make New Ability", order = 0)]

public class Ability : ScriptableObject
{

    public GameObject abilityPrefab;
    private GameObject currentEffect;
    public AbilityType abilityType;
    public float abilityTime = 45f;
    private const float speedForce = 2;
    private const float jumpForce = 10;

    public void GiveAbility(PlayerAttribute player)
    {
        PersonalCanvas playerPersonvalCanvas = player.GetComponentInChildren<PersonalCanvas>();

        if (abilityType == AbilityType.Speed)
        {
            playerPersonvalCanvas.StartAbilityCountDown(abilityTime);
            playerPersonvalCanvas.DisplayAbility("Speed Force Updated");
            player.moveSpeed *= speedForce;
            player.sprintSpeed *= speedForce;
            currentEffect = Instantiate(abilityPrefab, player.transform.position + new Vector3(0, 0.1f, 0), Quaternion.Euler(-90, 0, 0), player.transform);

        }
        else if (abilityType == AbilityType.Jump)
        {
            playerPersonvalCanvas.StartAbilityCountDown(abilityTime);
            player.jumpHeight *= jumpForce;
            playerPersonvalCanvas.DisplayAbility("Jump Force Updated");
            currentEffect = Instantiate(abilityPrefab, player.transform.position + new Vector3(0, 0.1f, 0), Quaternion.Euler(-90, 0, 0), player.transform);

        }
        else if (abilityType == AbilityType.Jetpack)
        {
            playerPersonvalCanvas.StartAbilityCountDown(abilityTime);

            playerPersonvalCanvas.DisplayAbility("Jump Force Updated");
            currentEffect = Instantiate(abilityPrefab, player.transform.localPosition + new Vector3(0.02f, 1.25f, -0.205f), Quaternion.identity, player.transform);
        }

    }
    public void TakeAbility(PlayerAttribute player)
    {
        PersonalCanvas playerPersonvalCanvas = player.GetComponentInChildren<PersonalCanvas>();
        if (abilityType == AbilityType.Speed)
        {
            playerPersonvalCanvas.StartAbilityCountDown(abilityTime);
            player.moveSpeed /= speedForce;
            player.sprintSpeed /= speedForce;
        }
        else if (abilityType == AbilityType.Jump)
        {
            playerPersonvalCanvas.StartAbilityCountDown(abilityTime);
            player.jumpHeight /= jumpForce;
        }
        Destroy(currentEffect);
    }

}
