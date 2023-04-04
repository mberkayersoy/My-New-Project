using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Photon.Pun;

public class UIPlayerListEntry : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI PlayerNameText;

    //public Image PlayerColorImage;
    public Image PlayerReadyImage;
    public Button PlayerReadyButton;
    public TMP_Dropdown TeamDropdown;
    //public int maxSelections = 2;
    //private Dictionary<string, int> selectionCounts = new Dictionary<string, int>();

    private int ownerId;
    private bool isPlayerReady;

    #region UNITY

    public void OnEnable()
    {
        PlayerNumbering.OnPlayerNumberingChanged += OnPlayerNumberingChanged;
    }

    public void Start()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber != ownerId)
        {
            PlayerReadyButton.gameObject.SetActive(false);
            TeamDropdown.gameObject.SetActive(false);
        }
        else
        {
            ExitGames.Client.Photon.Hashtable props = new() { { "IsPlayerReady", isPlayerReady } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);

            PlayerReadyButton.onClick.AddListener(() =>
            {
                isPlayerReady = !isPlayerReady;
                SetPlayerReady(isPlayerReady);

                ExitGames.Client.Photon.Hashtable props2 = new() { { "IsPlayerReady", isPlayerReady } };
                PhotonNetwork.LocalPlayer.SetCustomProperties(props2);

                if (PhotonNetwork.IsMasterClient)
                {
                    FindObjectOfType<NetworkUIManager>().LocalPlayerPropertiesUpdated();
                }
            });
        }

        // Selection counts initialization
        //foreach (var option in TeamDropdown.options)
        //{
        //    selectionCounts.Add(option.text, 0);
        //}

        //// OnValueChanged event subscription
        //TeamDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    public void OnDisable()
    {
        PlayerNumbering.OnPlayerNumberingChanged -= OnPlayerNumberingChanged;
    }

    #endregion

    public void Initialize(int playerId, string playerName)
    {
        ownerId = playerId;
        PlayerNameText.text = playerName;

        //TeamDropdown.onValueChanged.AddListener(delegate
        //{
        //    SetPlayerTeamColor();
        //});
    }

    private void OnPlayerNumberingChanged()
    {

    }

    public void SetPlayerReady(bool playerReady)
    {
        PlayerReadyButton.GetComponentInChildren<TextMeshProUGUI>().text = playerReady ? "Ready !" : "Ready ?";

        if (playerReady) 
        {
            PlayerReadyButton.image.color = Color.green;
            PlayerReadyImage.enabled = true;
        }
        else
        {
            PlayerReadyButton.image.color = Color.red;
            PlayerReadyImage.enabled = false;
        }
    }

    //public void SetPlayerTeamColor()
    //{
    //    // Get the selected team from the dropdown
    //    int selectedTeam = TeamDropdown.value;

    //    // Convert the selected team ID to TeamID enum
    //    TeamID teamID = (TeamID)selectedTeam;

    //    // Get the corresponding team color
    //    Color teamColor = TeamColor.GetTeamColor(teamID);

    //    // Set the player color image to the team color
    //    PlayerColorImage.color = teamColor;
    //}

    //private void OnDropdownValueChanged(int selectionIndex)
    //{
    //    string selectionText = TeamDropdown.options[selectionIndex].text;

    //    // Check if the selection limit has been reached
    //    if (selectionCounts[selectionText] >= maxSelections)
    //    {
    //        // Deselect the current selection
    //        TeamDropdown.SetValueWithoutNotify(-1);
    //    }
    //    else
    //    {
    //        // Increase the selection count of the selected option
    //        selectionCounts[selectionText]++;
    //    }

    //    // Decrease the selection count of the previously selected option (if any)
    //    if (selectionIndex >= 0)
    //    {
    //        string previousSelectionText = TeamDropdown.options[selectionIndex].text;
    //        selectionCounts[previousSelectionText]--;
    //    }
    //}
}
