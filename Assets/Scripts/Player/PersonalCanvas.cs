using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Photon.Pun;

public class PersonalCanvas : MonoBehaviour
{
    [Header("UI Variables")]
    public GameObject deadSection;
    public GameObject abilityCountDownDisplay;
    public TextMeshProUGUI announceAbility;
    public TextMeshProUGUI deadSectionCountdown;
    public Image UITeamColor;
    public Image countdownImage;

    [Header("Escape Panel")]
    public GameObject ESCPanel;
    public GameObject ConfirmPanel;
    public Button returnMenuButton;
    public Button yesButton;
    public Button noButton;

    [Space(5)]

    [Header("Other Variables")]
    public PlayerAttribute player;
    float abilityRemainingTime;
    public float respawnRemainingTime = 3f;

    void Start()
    {
        player = GetComponentInParent<PlayerAttribute>();
        UITeamColor.color = player.teamColor;
        abilityCountDownDisplay.SetActive(false);
        announceAbility.transform.localScale = Vector3.zero;
        deadSectionCountdown = deadSection.GetComponentInChildren<TextMeshProUGUI>();
        deadSectionCountdown.color = player.teamColor;
        announceAbility.color = player.teamColor;
        ESCPanel.SetActive(false);
        ConfirmPanel.SetActive(false);
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

        EscapePanelUI();
    }

    

    public void EscapePanelUI()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ESCPanel.SetActive(!ESCPanel.activeSelf);

            if (ESCPanel.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
                NetworkUIManager.Instance.RightPanel.SetActive(true);
            }
            else
            {
                NetworkUIManager.Instance.RightPanel.SetActive(false);
                ConfirmPanel.SetActive(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

        
    public void OnClickReturnMenu()
    {
        ConfirmPanel.SetActive(true);
    }

    public void OnClickNoButton()
    {
        ConfirmPanel.SetActive(false);
    }    
    public void OnClickYesButton()
    {
        PhotonNetwork.LeaveRoom();
        NetworkUIManager.Instance.menuCamera.gameObject.SetActive(true);
        NetworkUIManager.Instance.SetActivePanel(NetworkUIManager.Instance.choicePanel.name);
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
