using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class NetworkManagement : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI logText;
    
    void Start()
    {
        /*Action priority
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.JoinRoom("oda ismi");
        PhotonNetwork.JoinRandomRoom();
        PhotonNetwork.CreateRoom("room name", new RoomOptions { MaxPlayers = 2, IsOpen = true, IsVisible = true }, TypedLobby.Default);
        PhotonNetwork.JoinOrCreateRoom("room name", new RoomOptions { MaxPlayers = 2, IsOpen = true, IsVisible = true }, TypedLobby.Default);
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.LeaveRoom();*/
        //Action priority

        PhotonNetwork.ConnectUsingSettings();
        //PhotonNetwork.ConnectToRegion("eu");
        //PhotonNetwork.ConnectToBestCloudServer();

        /*PhotonNetwork.JoinRoom("oda ismi");
        PhotonNetwork.JoinRandomRoom();
        PhotonNetwork.CreateRoom("room name", new RoomOptions { MaxPlayers = 2, IsOpen = true, IsVisible = true }, TypedLobby.Default);
        PhotonNetwork.JoinOrCreateRoom("room name", new RoomOptions { MaxPlayers = 2, IsOpen = true, IsVisible = true }, TypedLobby.Default);
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.LeaveRoom();*/
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PhotonNetwork.LeaveRoom();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PhotonNetwork.LeaveLobby();
        }

        //if (PhotonNetwork.IsConnected)
        //{
        //    Debug.Log("Connected");
        //}
        //else
        //{
        //    Debug.Log("Not connected");
        //}

        //if (PhotonNetwork.IsConnectedAndReady)
        //{
        //    Debug.Log("Connected and ready");
        //}
        //else
        //{
        //    Debug.Log("Not ready");
        //}

        //if (PhotonNetwork.InLobby)
        //{
        //    Debug.Log("Player is in the lobby.");
        //}
        //else
        //{
        //    Debug.Log("Player is not in the lobby.");
        //}

        //if (PhotonNetwork.IsMasterClient)
        //{
        //    Debug.Log("Player is the master client.");
        //}
        //else
        //{
        //    Debug.Log("Player is not the master client.");
        //}

        //if (PhotonNetwork.InRoom)
        //{
        //    Debug.Log("Player is in the room.");
        //}
        //else
        //{
        //    Debug.Log("Player is not in the room.");
        //}
    }

    /* public override void OnConnected() //Checks if the player is connected.
     {
         Debug.Log("Connected...");
     }*/

    public override void OnConnectedToMaster() //Checks if the player is ready for other actions after connecting.
    {
        GetLog("Player connected to server...");
        PhotonNetwork.JoinLobby(); //if player connected, join lobby.
    }
    //public override void OnDisconnected(DisconnectCause cause)
    //{
    //    GetLog("The connection is broken...");
    //    PhotonNetwork.ReconnectAndRejoin();
    //}

    public override void OnJoinedLobby()
    {
        GetLog("Player connected to lobby...");

        //PhotonNetwork.JoinRoom("room name");
        //PhotonNetwork.JoinRandomRoom();

        //PhotonNetwork.CreateRoom("oda ismi", new RoomOptions { MaxPlayers = 2, IsOpen = true, IsVisible = true }, TypedLobby.Default);
        PhotonNetwork.NickName = "Berkay";
        PhotonNetwork.JoinOrCreateRoom("room name", new RoomOptions { MaxPlayers = 2, IsOpen = true, IsVisible = true }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        GetLog("Player joined a room...");
        //Debug.Log(PhotonNetwork.LocalPlayer.NickName);
        //Debug.Log(PhotonNetwork.CountOfPlayers);
        //Debug.Log(PhotonNetwork.CountOfRooms);
        //Debug.Log(PhotonNetwork.CountOfPlayersInRooms);
        //Debug.Log(PhotonNetwork.CountOfPlayersOnMaster);

        Debug.Log(PhotonNetwork.CurrentRoom);
        Debug.Log(PhotonNetwork.CurrentLobby);
        Debug.Log(PhotonNetwork.CloudRegion);
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
        logText.text = text;
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
}
