using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class GameEndUI : MonoBehaviourPunCallbacks
{
    private TextMeshProUGUI winnerDisplay;
    private void Start()
    {
        winnerDisplay = GetComponentInChildren<TextMeshProUGUI>();
        DisplayWinner();
    }

    void DisplayWinner()
    {
        Cursor.lockState = CursorLockMode.None;
        winnerDisplay.text = "WINNER" + "\n" + ScoreBoard.Instance.GetWinners().ToString();
        winnerDisplay.color = TeamColor.GetTeamColor(ScoreBoard.Instance.GetWinners());
        UpdatePlayerDatas();
    }

    void UpdatePlayerDatas()
    {
        int gainExperience = (int)ScoreBoard.Instance.teamscores[(TeamID)PhotonNetwork.LocalPlayer.GetTeamID()];
        FirebaseManager.Instance.GetComponent<PlayerData>().SetExperience(gainExperience);
        if (PhotonNetwork.LocalPlayer.GetTeamID() == (int)ScoreBoard.Instance.GetWinners())
        {
            FirebaseManager.Instance.GetComponent<PlayerData>().AddWin();
        }
        FirebaseManager.Instance.UpdatePlayerExperience();
        FirebaseManager.Instance.UpdatePlayerWinCount();
    }
    public void ReturnLobby()
    {
        //DestroyImmediate(GameManagerr.Instance.playerManage);
        PhotonNetwork.LeaveRoom();
        NetworkUIManager.Instance.menuCamera.gameObject.SetActive(true);
        NetworkUIManager.Instance.SetActivePanel(NetworkUIManager.Instance.choicePanel.name);
    }
}
