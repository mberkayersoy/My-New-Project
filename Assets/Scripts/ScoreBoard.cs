using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    public static ScoreBoard Instance;
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public float scoreMult;
    public Dictionary<TeamID, float> teamscores = new Dictionary<TeamID, float>
    {
        { TeamID.Blue_Team, 0 },
        { TeamID.Red_Team, 0 },
        { TeamID.Green_Team, 0 },
        { TeamID.Yellow_Team, 0 },
    };

    public void SetScore(TeamID teamID, float ballScale)
    {
        teamscores[teamID] += (int)(scoreMult * (1 / ballScale));
        UIManager.Instance.GameUISection.ScoreDisplay();

        if (GameManagerr.Instance.isGameEnd)
        {
            GetWinners();
        }

    }

    public TeamID GetWinners()
    {
        TeamID winningTeam = TeamID.Blue_Team;
        float highestScore = teamscores[TeamID.Blue_Team];

        foreach (KeyValuePair<TeamID, float> teamScore in teamscores)
        {
            if (teamScore.Value > highestScore)
            {
                highestScore = teamScore.Value;
                winningTeam = teamScore.Key;
            }
        }

        return winningTeam;
    }
}
