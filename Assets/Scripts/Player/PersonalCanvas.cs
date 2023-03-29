using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonalCanvas : MonoBehaviour
{
    public GameObject deadSection;
    public Player player;
    public Image UITeamColor;
    public GameObject abilityCountDownDisplay;
    public Image countdownImage;
    float remainingTime;
    // Time stamp arastir.
    void Start()
    {
        player = GetComponentInParent<Player>();
        UITeamColor.color = player.teamColor;
        deadSection.SetActive(false);
        abilityCountDownDisplay.SetActive(false);
    }

    private void Update()
    {
        if (player.currentAbility != null)
        {
            remainingTime -= Time.deltaTime;
            float fillAmount = remainingTime / player.currentAbility.abilityTime;
            countdownImage.fillAmount = fillAmount;
            countdownImage.type = Image.Type.Filled;
            countdownImage.fillMethod = Image.FillMethod.Horizontal;
            countdownImage.fillOrigin = (int)Image.OriginHorizontal.Left;

            if (remainingTime <= 0)
            {
                Debug.Log("stopability");
                StopAbility();
            }
        }   
    }
    public void StopAbility()
    {
        player.SetAbility();
        abilityCountDownDisplay.SetActive(false);
    }

    public void StartAbilityCountDown(float abilityTime)
    {
        Debug.Log("abilityTime: " + abilityTime);
        abilityCountDownDisplay.SetActive(true);
        remainingTime = abilityTime;
    }
    public void DeadSectionOn()
    {
        deadSection.SetActive(true);
        
    }
    public void DeadSectionOff()
    {
        deadSection.SetActive(false);
    }
}
