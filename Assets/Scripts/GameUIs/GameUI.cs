using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public List<TextMeshProUGUI> teamScoresText;

    void Start()
    {
        ScoreDisplay();
    }

    public void ScoreDisplay()
    {
        teamScoresText[0].text = "Blue Team: " + ScoreBoard.Instance.teamscores[TeamID.Blue_Team];
        teamScoresText[1].text = "Red Team: " + ScoreBoard.Instance.teamscores[TeamID.Red_Team];
        teamScoresText[2].text = "Green Team: " + ScoreBoard.Instance.teamscores[TeamID.Green_Team];
        teamScoresText[3].text = "Yellow Team: " + ScoreBoard.Instance.teamscores[TeamID.Yellow_Team];
        //GameEnd();
    }

    public void GameEnd()
    {
        if (GameManagerr.Instance.isGameEnd)
        {
            UIManager.Instance.GameEndUISection.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
