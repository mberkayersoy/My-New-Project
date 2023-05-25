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
        winnerDisplay.text = "WINNER" + "\n" + ScoreBoard.Instance.GetWinners().ToString();
        winnerDisplay.color = TeamColor.GetTeamColor(ScoreBoard.Instance.GetWinners());
    }
    public void ReturnLobby()
    {
        //DestroyImmediate(GameManagerr.Instance.playerManage);
        PhotonNetwork.LeaveRoom();
        NetworkUIManager.Instance.menuCamera.gameObject.SetActive(true);
        NetworkUIManager.Instance.SetActivePanel(NetworkUIManager.Instance.choicePanel.name);
    }
}
