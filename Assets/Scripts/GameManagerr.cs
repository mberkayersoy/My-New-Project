using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;

public class GameManagerr : MonoBehaviourPunCallbacks
{
    private static GameManagerr instance; // Tek örnek
    public static GameManagerr Instance // Eriþim için kullanýlan özellik
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManagerr>();
                if (instance == null)
                {
                    Debug.LogError("GameManagerr not found in the scene.");
                }
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    public GameObject[] spawnPoints;
    public List<GameObject> playersList;
    public List<GameObject> abilities;
    public List<int> ballList;
    public GameObject mainBall;
    public float respawnTime = 5f;
    public int abilityPercentage;
    public bool isGameEnd = false;
    public PhotonView pw;
    public GameObject playerManage;
    public void StartTheGame()
    {
        UIManager.Instance.PreGameUISection.StartCountDown();
        if (!PhotonNetwork.IsMasterClient) return; // Only MasterClient can instantiate first ball.

        mainBall = PhotonNetwork.InstantiateRoomObject("Ball", Vector3.up * 100, Quaternion.identity);
        mainBall.SetActive(true);
        //ballList.Add(mainBall.GetPhotonView().ViewID);
        //pw.RPC("AddBallList", RpcTarget.All, mainBall.GetPhotonView().ViewID);
        isGameEnd = false;
    }
    private void Start()
    {
        RoomManager.Instance.PlayerCreate();
        NetworkUIManager.Instance.menuCamera.gameObject.SetActive(false);
        NetworkUIManager.Instance.insideRoomPanel.gameObject.SetActive(false);
        //if (!PhotonNetwork.IsConnected) return;

        //if (PhotonNetwork.IsMasterClient)
        //{
        //    // MasterClient bu GameManager instance'ýný Photon aðý üzerinde oluþturur.
        //    PhotonNetwork.InstantiateRoomObject("GameManagerr", Vector3.zero, Quaternion.identity);
        //}

        //ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
        //{
        //    { "PlayerLoadedLevel", true}
        //};
        //PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        pw = GetComponent<PhotonView>();

        //for (int i = 0; i < spawnPoints.Length; i++)
        //{
        //    spawnPoints[i].GetComponent<MeshRenderer>().material.SetColor("_Color", TeamColor.GetTeamColor((TeamID)i));
        //}
    }

    //private bool CheckAllPlayerLoadedLevel()
    //{
    //    foreach (Player player in PhotonNetwork.PlayerList)
    //    {
    //        if (player.CustomProperties.TryGetValue("PlayerLoadedLevel", out object playerLoadedLevel))
    //        {
    //            if ((bool)playerLoadedLevel)
    //            {
    //                continue;
    //            }

    //            return false;
    //        }
    //    }

    //    return true;
    //}

    //public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    //{
    //    //if (!PhotonNetwork.IsMasterClient)
    //    //{
    //    //    return;
    //    //}

    //    if (changedProps.ContainsKey("PlayerLoadedLevel"))
    //    {
    //        if (CheckAllPlayerLoadedLevel())
    //        {
    //            UIManager.Instance.PreGameUISection.GetComponent<PreGameUI>().StartCountDown();
    //        }
    //        else
    //        {
    //            //UIManager.Instance.PreGameUISection.GetComponent<PreGameUI>().infoText.text = "Other players are waiting...";
    //        }
    //    }

    //}
    public void EditHitPlayers(GameObject player, TeamID ballTeamID)
    {
        // Give points to the team that has the teamID of the ball that hit the player.
        if (player.GetComponent<PlayerAttribute>().teamID != (int)ballTeamID)
        {
            //ScoreBoard.Instance.SetScore(ballTeamID, 1);
            //ScoreBoard.Instance.pw.RPC("SetScore", RpcTarget.AllBuffered, ballTeamID, 1);
            ScoreBoard.Instance.SetScore(ballTeamID, 1);
            
        }
        // If the player is hit by the ball with his teamID, do nothing.
        else
        {
            Debug.Log("Kendi topun vurdu seni");
        }
        player.GetComponent<FirstPersonMovement>().playerManage.Die();
        //player.GetComponent<FirstPersonMovement>().ResetAnimator();
        //player.GetComponentInChildren<PersonalCanvas>().DeadSectionOn(); // Activate player dead canvas.
        //GeneratePlayers(player);
        //player.transform.position = spawnPoints[player.GetComponent<Player>().teamID].transform.position + Vector3.up; // Relocate the player in Spawn.

        //StartCoroutine(Respawn(player)); 
    }

    // Execute before the game start.
    public Vector3 GeneratePlayers(int teamID) // Everybody spawn their base and dont move until game start.
    {
        Collider spawnCollider = spawnPoints[teamID].GetComponent<MeshCollider>();
        Vector3 randomPoint = new Vector3(Random.Range(spawnCollider.bounds.min.x, spawnCollider.bounds.max.x), 0f, Random.Range(spawnCollider.bounds.min.z, spawnCollider.bounds.max.z));
        Ray ray = new Ray(new Vector3(randomPoint.x, spawnCollider.bounds.max.y + 10f, randomPoint.z), Vector3.down);
        
        if (spawnCollider.Raycast(ray, out RaycastHit hit, 20f))
        {
            randomPoint = hit.point;
        }

        return randomPoint + Vector3.up / 2;
        //player.GetComponent<PlayerAttribute>().isDead = true;
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
    [PunRPC]
    public void AddBallList(int ball1)
    {
        ballList.Add(ball1);
    }

    // The balls destroyed by the characters are removed from the ball list.
    [PunRPC]
    public void RemoveBallList(int ball)
    {
        ballList.Remove(ball);
         //pw.RPC("GenerateAbility", RpcTarget.All, ball.transform);
         //GenerateAbility(ball.transform);
    }

    // Create the ability balls in the game area.
    [PunRPC]
    public void GenerateAbility(Vector3 ballTransform)
    {
        int abilityPossibility = Random.Range(0, 101);

        // Ability determines drop probability.
        if (abilityPossibility < abilityPercentage)
        {
            int randomNumber = Random.Range(0, abilities.Count);
            PhotonNetwork.InstantiateRoomObject(abilities[randomNumber].name, ballTransform, Quaternion.identity, 0);
        }
    }

    [PunRPC]
    public void CheckGameEnd()
    {
        if (ballList.Count <= 0)
        {
            isGameEnd = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            UIManager.Instance.GameUISection.gameObject.SetActive(false);
            UIManager.Instance.GameEndUISection.gameObject.SetActive(true);
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
