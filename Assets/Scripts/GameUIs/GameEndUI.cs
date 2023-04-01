using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameEndUI : MonoBehaviour
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
}
