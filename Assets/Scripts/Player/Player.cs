using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int teamID;
    public Color teamColor;
    public bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        teamColor = TeamColor.GetTeamColor((TeamID)teamID);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
