using System.Collections;
using System.Collections.Generic;
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
    public float blueScore;
    public float redScore;
    public float yellowScore;
    public float greenScore;
    public float scoreMult;

    public void SetScore(int teamID, float ballScale)
    {
        switch (teamID)
        {
            case 0:
                blueScore += (int)(scoreMult * (1 / ballScale));
                break;

            case 1:
                redScore += (int)(scoreMult * (1 / ballScale));
                break;

            case 2:
                yellowScore += (int)(scoreMult * (1 / ballScale));
                break;

            case 3:
                greenScore += (int)(scoreMult * (1 / ballScale));
                break;
        }
    }
}
