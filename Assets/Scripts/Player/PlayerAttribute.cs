using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttribute : MonoBehaviourPunCallbacks
{
    [Header("Numeric Variables")]
    public int teamID;
    [Tooltip("The move speed the player can reach")]
    public float moveSpeed = 2.0f;
    [Tooltip("The sprint speed the player can reach")]
    public float sprintSpeed = 5.335f;
    [Tooltip("The height the player can jump")]
    public float jumpHeight = 1.2f;


    [Space]

    [Header("Other Variables")]
    public Color teamColor;
    public bool isDead;
    public Ability currentAbility;
    public MeshRenderer meshMaterial;

    private void Awake()
    {
        SetTeamColor(teamID);
    }

    void SetTeamColor(int teamID)
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
        Debug.Log("viewID setability: " + GetComponent<PhotonView>().ViewID);
        //if (!GetComponent<PhotonView>().IsMine) return;
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
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Pickup"))
    //    {
    //       SetAbility(other.GetComponent<PickUp>().ability);
    //    }
    //}
}
