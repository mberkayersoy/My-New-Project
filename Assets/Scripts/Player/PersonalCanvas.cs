using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonalCanvas : MonoBehaviour
{
    public GameObject DeadSection;
    public Image UITeamColor;
    // Start is called before the first frame update
    void Start()
    {
        UITeamColor.color = GetComponentInParent<Player>().teamColor;
        DeadSection.SetActive(false);
    }
    public void DeadSectionOn()
    {
        DeadSection.SetActive(true);
        
    }
    public void DeadSectionOff()
    {
        DeadSection.SetActive(false);
    }
}
