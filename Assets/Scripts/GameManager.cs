using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public float respawnTime = 5f;
    public GameObject[] spawnPoints;
    public GameObject[] playersList;
    public List<GameObject> abilities;
    public List<Ball> ballList;
    public Ball mainBall;

    public void StartTheGame()
    {
        GivePlayerFullAccess(playersList);
        mainBall.MoveTheBall();
        ballList.Add(mainBall);
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

    public void RespawnPlayer(GameObject player, Color ballColor)
    {
        player.GetComponentInChildren<PersonalCanvas>().DeadSectionOn();
        player.transform.position = spawnPoints[player.GetComponent<Player>().teamID].transform.position;
        player.GetComponent<Rigidbody>().isKinematic = true;
        player.GetComponent<Player>().isDead = true;
        StartCoroutine(Respawn(player)); 
    }

    IEnumerator Respawn(GameObject player)
    {
        yield return new WaitForSeconds(respawnTime);
        player.GetComponent<Rigidbody>().isKinematic = false;
        player.GetComponent<Player>().isDead = false;
        player.GetComponentInChildren<PersonalCanvas>().DeadSectionOff();
    }

    void GeneratePlayers(GameObject player) // Everybody spawn their base and dont move until game start.
    {
        player.transform.position = spawnPoints[player.GetComponent<Player>().teamID].transform.position;
        player.GetComponent<Rigidbody>().isKinematic = true;
        player.GetComponent<Player>().isDead = true;
    }
    void GivePlayerFullAccess(GameObject[] playersList)
    {
        foreach (var player in playersList)
        {
            player.GetComponent<Rigidbody>().isKinematic = false;
            player.GetComponent<Player>().isDead = false;
        }
    }

    public void AddBallList(Ball ball1, Ball ball2)
    {
        ballList.Add(ball1);
        ballList.Add(ball2);
    }
    public void RemoveBallList(Ball ball)
    {
        ballList.Remove(ball);
        GenerateAbility(ball.transform);

    }
    public void GenerateAbility(Transform ballTransform)
    {
        Instantiate(abilities[0], Vector3.up, Quaternion.identity);
    }
    public void GiveAbilityToPlayer(GameObject player)
    {
        player.GetComponent<FirstPersonMovement>().speed += 50 ; 

    }

}

public enum TeamID
{
    BlueTeam = 0,
    RedTeam = 1,
    GreenTeam = 2,
    YellowTeam = 3
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
        return Color.black;
    }
}
