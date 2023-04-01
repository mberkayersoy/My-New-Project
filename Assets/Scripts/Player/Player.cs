using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Numeric Variables")]
    public int teamID;
    [Tooltip("The move speed the player can reach")]
    public float moveSpeed = 2.0f;
    [Tooltip("The sprint speed the player can reach")]
    public float sprintSpeed = 5.335f;
    [Tooltip("The height the player can jump")]
    public float jumpHeight = 1.2f;
    [Tooltip("The height the player can jump")]
    public SkinnedMeshRenderer meshMaterial;

    [Space]

    [Header("Other Variables")]
    public Color teamColor;
    public bool isDead;
    public Ability currentAbility;

    private void Awake()
    {
        teamColor = TeamColor.GetTeamColor((TeamID)teamID);
        foreach (var material in meshMaterial.materials)
        {
            material.color = teamColor;
        }

    }

    public Ability GetAbility()
    {
        return currentAbility;
    }
    public void SetAbility(Ability newAbility)
    {
        if (currentAbility != null)
        {
            currentAbility.TakeAbility(this);
        }
        
        currentAbility = newAbility;

        if (currentAbility != null)
        {
            currentAbility.GiveAbility(this);
        }
    }
}
