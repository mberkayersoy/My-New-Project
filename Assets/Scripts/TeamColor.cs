using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamColor : MonoBehaviour
{
    
    void Start()
    {
        int teamID = GetComponentInParent<ThirdPersonShooterController>().teamNumber;
        Material[] modelMaterials = gameObject.GetComponent<SkinnedMeshRenderer>().materials;

        foreach (var material in modelMaterials)
        {
            switch(teamID)
            {
                case 0:
                    material.SetColor("_Color", Color.blue);
                    break;
                case 1:
                    material.SetColor("_Color", Color.red);
                    break;
                case 2:
                    material.SetColor("_Color", Color.yellow);
                    break;
                case 3:
                    material.SetColor("_Color", Color.green);
                    break;
            }
        }
    }

    
    void Update()
    {
        
    }
}
