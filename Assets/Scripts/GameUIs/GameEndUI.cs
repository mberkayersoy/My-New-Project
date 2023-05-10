using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

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
        SceneManager.LoadScene("Login");
    }
}
