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

    public Image PlayerColorImage;
    public Image PlayerReadyImage;
    public Button PlayerReadyButton;
    public TMP_Dropdown TeamDropdown;
    public int maxSelections = 2;
    private Dictionary<string, int> selectionCounts = new Dictionary<string, int>();

    private int ownerId;
    private bool isPlayerReady;
    private TeamID selectedTeamID;

    #region UNITY

    public void OnEnable()
    {
        PlayerNumbering.OnPlayerNumberingChanged += OnPlayerNumberingChanged;
    }

    public void Start()
    {
        //Selection counts initialization
        foreach (var option in TeamDropdown.options)
        {
            selectionCounts.Add(option.text, 0);
        }
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

            TeamDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }

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
        SetPlayerTeamColor(selectedTeamID);
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
            PlayerReadyImage.enabled = playerReady;
        }
        else
        {
            PlayerReadyButton.image.color = Color.red;
            PlayerReadyImage.enabled = playerReady;
        }
    }

    public int SetPlayerTeamColor(TeamID teamID)
    {
        // Get the selected team from the dropdown
        int selectedTeam = TeamDropdown.value;


        // Get the corresponding team color
        Color teamColor = TeamColor.GetTeamColor(teamID);
        // Set the player color image to the team color
        PlayerColorImage.color = teamColor;
        return selectedTeam;
    }

    private void OnDropdownValueChanged(int selectionIndex = 0)
    {
        string selectionText = TeamDropdown.options[selectionIndex].text;

        // Check if the selection limit has been reached
        if (selectionCounts[selectionText] >= maxSelections)
        {
            // Deselect the current selection
            TeamDropdown.SetValueWithoutNotify(-1);
        }
        else
        {
            // Increase the selection count of the selected option
            selectionCounts[selectionText]++;
        }

        // Decrease the selection count of the previously selected option (if any)
        if (selectionIndex >= 0)
        {
            string previousSelectionText = TeamDropdown.options[selectionIndex].text;
            selectionCounts[previousSelectionText]--;
        }

        selectedTeamID = (TeamID)selectionIndex;
        PhotonNetwork.LocalPlayer.SetTeamID(selectionIndex);
        ExitGames.Client.Photon.Hashtable team = new() { { "SelectedTeamID", selectedTeamID } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(team);

    }
}
