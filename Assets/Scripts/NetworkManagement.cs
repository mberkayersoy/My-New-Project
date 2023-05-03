using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManagement : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() //Checks if the player is ready for other actions after connecting.
    {
        GetLog("Player connected to server...");
        PhotonNetwork.JoinLobby(); //if player connected, join lobby.
    }

    public override void OnJoinedLobby()
    {
        GetLog("Player connected to lobby...");
        //PhotonNetwork.JoinOrCreateRoom("room name", new RoomOptions { MaxPlayers = 2, IsOpen = true, IsVisible = true }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        GetLog("Player joined a room...");

        //GameObject player = PhotonNetwork.Instantiate("Capsule", Vector3.up, Quaternion.identity);
        //GameManagerr.Instance.GiveCamera(player);
        //GameManagerr.Instance.playersList.Add(player);
        //GameManagerr.Instance.GeneratePlayers(player);
        //GameManagerr.Instance.gameObject.SetActive(true);
    }

    public override void OnLeftLobby()
    {
        GetLog("Exited the lobby...");
    }

    public override void OnLeftRoom()
    {
        GetLog("Exited the room....");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        //GetLog("Couldn't join a room..." + message + " - " + returnCode);
        Debug.Log("Couldn't join a room..." + message + " - " + returnCode);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //GetLog("Couldn't join a random room..." + message + " - " + returnCode);
        Debug.Log("Couldn't join a random room..." + message + " - " + returnCode);
    }
    
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        //GetLog("Couldn't create a room..." + message + " - " + returnCode);
        Debug.Log("Couldn't create a room..." + message + " - " + returnCode);
    }

    void GetLog(string text)
    {
        //logText.text = text;
        Debug.Log(text);
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }
    
    public void Reconnect()
    {
        PhotonNetwork.Reconnect(); 
    }

    public void StatisticsReport()
    {
        PhotonNetwork.NetworkStatisticsEnabled = true;
        Debug.Log(PhotonNetwork.NetworkStatisticsToString());
    }

    public void StatisticsReset()
    {
        PhotonNetwork.NetworkStatisticsReset();
    }

    public void GetPing()
    {
        GetLog(PhotonNetwork.GetPing().ToString());
    }

    //public void LogIn()
    //{
    //    PhotonNetwork.NickName = emailInput.text;

    //    if (emailInput.text == emaill && passwordInput.text == pass)
    //    {
    //        SceneManager.LoadScene("Home");
    //    }
        
    //}
}
