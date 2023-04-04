using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerr : MonoBehaviour
{
    public static GameManagerr Instance;
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public GameObject[] spawnPoints;
    public Camera playerCamera;
    public List<GameObject> playersList;
    public List<GameObject> abilities;
    public List<Ball> ballList;
    public Ball mainBall;
    public float respawnTime = 5f;
    public int abilityPercentage;
    public bool isGameEnd = false;

    public void StartTheGame()
    {
        mainBall.MoveTheBall();
        ballList.Add(mainBall);
        isGameEnd = false;
        GivePlayerFullAccess(playersList);
    }
    private void Start()
    {
        foreach (GameObject player in playersList)
        {
            GeneratePlayers(player);

        }
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            spawnPoints[i].GetComponent<MeshRenderer>().material.SetColor("_Color", TeamColor.GetTeamColor((TeamID)i));
        }
    }

    public void EditHitPlayers(GameObject player, TeamID ballTeamID)
    {
        // Give points to the team that has the teamID of the ball that hit the player.
        if (player.GetComponent<PlayerAttribute>().teamID != (int)ballTeamID)
        {
            ScoreBoard.Instance.SetScore(ballTeamID, 1);
        }
        // If the player is hit by the ball with his teamID, do nothing.
        else
        {
            Debug.Log("Kendi topun vurdu seni");
        }
        //player.GetComponent<FirstPersonMovement>().ResetAnimator();
         player.GetComponentInChildren<PersonalCanvas>().DeadSectionOn(); // Activate player dead canvas.
        GeneratePlayers(player);
        //player.transform.position = spawnPoints[player.GetComponent<Player>().teamID].transform.position + Vector3.up; // Relocate the player in Spawn.
        //player.GetComponent<Player>().isDead = true;
        StartCoroutine(Respawn(player)); 
    }

    // Allow dead characters to play again after waiting a respawnTime.
    IEnumerator Respawn(GameObject player)
    {
        yield return new WaitForSeconds(respawnTime);
        player.GetComponent<PlayerAttribute>().isDead = false;
        player.GetComponentInChildren<PersonalCanvas>().DeadSectionOff(); // Deactivate player dead canvas.
    }

    // Execute before the game start.
    void GeneratePlayers(GameObject player) // Everybody spawn their base and dont move until game start.
    {
        player.transform.position = spawnPoints[player.GetComponent<PlayerAttribute>().teamID].transform.position + Vector3.up * 2;
        player.GetComponent<PlayerAttribute>().isDead = true;
    }

    // Execute when the game start.
    void GivePlayerFullAccess(List<GameObject> playersList)
    {
        foreach (var player in playersList)
        {
            player.GetComponent<PlayerAttribute>().isDead = false;
        }
    }

    // Add newly created balls to the ball list.
    public void AddBallList(Ball ball1, Ball ball2)
    {
        ballList.Add(ball1);
        ballList.Add(ball2);
    }

    // The balls destroyed by the characters are removed from the ball list.
    public void RemoveBallList(Ball ball)
    {
        ballList.Remove(ball);
        GenerateAbility(ball.transform);
        CheckGameEnd();
    }

    // Create the ability balls in the game area.
    public void GenerateAbility(Transform ballTransform)
    {
        int abilityPossibility = Random.Range(0, 101);

        // Ability determines drop probability.
        if (abilityPossibility < abilityPercentage)
        {
            int randomNumber = Random.Range(0, abilities.Count);
            Instantiate(abilities[randomNumber], ballTransform.position, Quaternion.identity);
        }
    }

    public void CheckGameEnd()
    {
        if (ballList.Count <= 0)
        {
            isGameEnd = true;
        }
    }
}

public enum TeamID
{
    Blue_Team = 0,
    Red_Team = 1,
    Green_Team = 2,
    Yellow_Team = 3,
    NoTeam = 4
}
public class TeamColor
{
    public static Color GetTeamColor(TeamID teamID)
    {
        switch (teamID)
        {
            case 0:
                return Color.blue;
            case (TeamID)1:
                return Color.red;
            case (TeamID)2:
                return Color.green;
            case (TeamID)3:
                return Color.yellow;
            default:
                break;
        }
        return Color.white;
    }
}
