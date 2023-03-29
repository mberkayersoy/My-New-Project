using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int teamID;
    public float speedForce;
    public float jumpForce;
    public Color teamColor;
    public bool isDead;
    public bool hasAbility;
    public Ability currentAbility;

    private void Awake()
    {
        speedForce = 25;
        jumpForce = 5;
        teamColor = TeamColor.GetTeamColor((TeamID)teamID);
    }


    public Ability GetAbility()
    {
        return currentAbility;
    }
    public void SetAbility(Ability newAbility = null)
    {
        if (currentAbility != null)
        {
            currentAbility.TakeAbility();
        }
        
        currentAbility = newAbility;
        
        if (currentAbility != null)
        {
            currentAbility.GiveAbility(this);
        }
    }

}
