using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class PersonalCanvas : MonoBehaviour
{
    [Header("UI Variables")]
    public GameObject deadSection;
    public GameObject abilityCountDownDisplay;
    public TextMeshProUGUI announceAbility;
    public TextMeshProUGUI deadSectionCountdown;
    public Image UITeamColor;
    public Image countdownImage;

    [Space]

    [Header("Other Variables")]
    public PlayerAttribute player;
    float abilityRemainingTime;
    float respawnRemainingTime = 5f;
    // Time stamp arastir.
    void Start()
    {
        player = GetComponentInParent<PlayerAttribute>();
        UITeamColor.color = player.teamColor;
        deadSection.SetActive(false);
        abilityCountDownDisplay.SetActive(false);
        announceAbility.transform.localScale = Vector3.zero;
        deadSectionCountdown = deadSection.GetComponentInChildren<TextMeshProUGUI>();
        deadSectionCountdown.color = player.teamColor;
        announceAbility.color = player.teamColor;
    }

    private void Update()
    {
        if (player.currentAbility != null)
        {
            abilityRemainingTime -= Time.deltaTime;
            float fillAmount = abilityRemainingTime / player.currentAbility.abilityTime;
            countdownImage.fillAmount = fillAmount;
            countdownImage.type = Image.Type.Filled;
            countdownImage.fillMethod = Image.FillMethod.Horizontal;
            countdownImage.fillOrigin = (int)Image.OriginHorizontal.Left;

            if (abilityRemainingTime <= 0)
            {
                Debug.Log("stopability");
                StopAbility();
            }
        }
        
        if (deadSection.activeSelf)
        {
            deadSection.GetComponentInChildren<TextMeshProUGUI>().text = "You Died" + "\n" + Mathf.FloorToInt(respawnRemainingTime).ToString();
            respawnRemainingTime -= Time.deltaTime;
            if (respawnRemainingTime <= 0)
            {
                respawnRemainingTime = 5f;
            }

        }
    }

    public void DisplayAbility(string abilitytype)
    {
        announceAbility.text = abilitytype;
        announceAbility.transform.DOScale(2f, 1.5f).SetEase(Ease.OutBounce)
            .OnComplete(() =>
            {
                announceAbility.transform.DOScale(0f, 1.5f).SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    announceAbility.text = "";
                });
            });
    }

    public void StopAbility()
    {
        player.SetAbility(null);
        abilityCountDownDisplay.SetActive(false);
    }

    public void StartAbilityCountDown(float abilityTime)
    {
        abilityCountDownDisplay.SetActive(true);
        abilityRemainingTime = abilityTime;
    }
    public void DeadSectionOn()
    {
        deadSection.SetActive(true);
        deadSectionCountdown.text = "You Died";
        deadSectionCountdown.transform.DOScale(4f, 4f).SetEase(Ease.OutBounce)
            .OnComplete(() =>
            {
                deadSectionCountdown.transform.DOScale(0f, 1f).SetEase(Ease.OutQuad);
            });

    }
    public void DeadSectionOff()
    {
        deadSection.SetActive(false);
    }
}
