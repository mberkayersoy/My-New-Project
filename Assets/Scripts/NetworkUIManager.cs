using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NetworkUIManager : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    [Header("Login Panel")]
    public GameObject loginPanel;
    public TMP_InputField usernameInput;

    [Header("Choice Panel")]
    public GameObject choicePanel;

    [Header("Create Room Panel")]
    public GameObject createRoomPanel;
    public TMP_InputField roomnameInput;
    public TMP_InputField maxPlayerInput;

    [Header("Random Room Join Panel")]
    public GameObject randomRoomJoinPanel;

    [Header("Room List Panel")]
    public GameObject roomlistPanel;
    public GameObject roomlistContent;
    public GameObject roomlistRowPrefab;

    [Header("Room Panel")]
    public GameObject insideRoomPanel;
    public Transform playersListTransform;
    public Button startgameButton;
    public GameObject playerlistRowPrefab;

    private Dictionary<string, RoomInfo> roomCacheList;
    private Dictionary<string, GameObject> roomlistElements;
    private Dictionary<int, GameObject> playerlistElements;

    public void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        roomCacheList = new Dictionary<string, RoomInfo>();
        roomlistElements = new Dictionary<string, GameObject>();    
    }


    public override void OnConnectedToMaster()
    {
        SetActivePanel(choicePanel.name);
    }

    public override void OnJoinedLobby()
    {
        roomCacheList.Clear();
        ClearRoomListView();
    }

    public override void OnLeftLobby()
    {
        roomCacheList.Clear();
        ClearRoomListView();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        SetActivePanel(choicePanel.name);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        SetActivePanel(choicePanel.name);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        string roomName = "Room " + Random.Range(100, 90000);

        RoomOptions options = new RoomOptions { MaxPlayers = 4 };
        PhotonNetwork.CreateRoom(roomName, options, null);
    }

    public override void OnJoinedRoom()
    {
        roomCacheList.Clear();

        SetActivePanel(insideRoomPanel.name);

        if (playerlistElements == null)
        {
            playerlistElements = new Dictionary<int, GameObject>();
        }

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject entry = Instantiate(playerlistRowPrefab, playersListTransform);
            entry.transform.localScale = Vector3.one;

            entry.GetComponent<UIPlayerListEntry>().Initialize(player.ActorNumber, player.NickName);

            if (player.CustomProperties.TryGetValue("IsPlayerReady", out object isPlayerReady))
            {
                entry.GetComponent<UIPlayerListEntry>().SetPlayerReady((bool) isPlayerReady);
            }

            playerlistElements.Add(player.ActorNumber, entry);
        }

        startgameButton.gameObject.SetActive(CheckPlayersReady());

        Hashtable props = new Hashtable
        {
            {"PlayerLoadedLevel", false }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

    }

    public override void OnLeftRoom()
    {
        SetActivePanel(choicePanel.name);

        foreach (GameObject entry in playerlistElements.Values)
        {
            Destroy(entry);
        }

        playerlistElements.Clear();
        playerlistElements = null;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameObject entry = Instantiate(playerlistRowPrefab, playersListTransform);
        entry.transform.localScale = Vector3.one;

        entry.GetComponent<UIPlayerListEntry>().Initialize(newPlayer.ActorNumber, newPlayer.NickName);

        playerlistElements.Add(newPlayer.ActorNumber, entry);

        startgameButton.gameObject.SetActive(CheckPlayersReady());
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(playerlistElements[otherPlayer.ActorNumber].gameObject);
        playerlistElements.Remove(otherPlayer.ActorNumber);
        startgameButton.gameObject.SetActive(CheckPlayersReady());

    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            startgameButton.gameObject.SetActive(CheckPlayersReady());
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (playerlistElements == null)
        {
            playerlistElements = new Dictionary<int, GameObject>();
        }

        if (playerlistElements.TryGetValue(targetPlayer.ActorNumber, out GameObject entry))
        {
            if (changedProps.TryGetValue("IsPlayerReady", out object isPlayerReady))
            {
                entry.GetComponent<UIPlayerListEntry>().SetPlayerReady((bool)isPlayerReady);
            }
        }

        startgameButton.gameObject.SetActive(CheckPlayersReady());
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomListView();
        UpdateCachedRoomList(roomList);
        UpdateRoomListView();

    }

    public void OnBackButtonClicked()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
        SetActivePanel(choicePanel.name);
    }

    public void OnCreateRoomButtonClicked()
    {
        string roomName = roomnameInput.text;

        roomName = (roomName.Equals(string.Empty)) ? "Room " + Random.Range(100, 90000) : roomName;

        byte.TryParse(maxPlayerInput.text, out byte maxPlayer);
        maxPlayer = (byte)Mathf.Clamp(maxPlayer, 2, 8);

        RoomOptions options = new RoomOptions { MaxPlayers = maxPlayer };
        PhotonNetwork.CreateRoom(roomName, options, null);
        
    }

    public void OnJoinRandomRoomButtonClicked()
    {
        SetActivePanel(randomRoomJoinPanel.name);
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnLeaveGameButtonClicked()
    {
        PhotonNetwork.LeaveRoom();

    }

    public void OnLoginButtonClicked()
    {
        string playerName = usernameInput.text;

        if (!playerName.Equals(""))
        {
            PhotonNetwork.LocalPlayer.NickName = playerName;
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            Debug.LogError("Username not suitable");
        }
    }

    public void OnRoomListButtonClicked()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
        SetActivePanel(roomlistPanel.name);
    }

    public void OnStartGameButtonClicked()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.LoadLevel("GameScene");
    }

    private bool CheckPlayersReady()
    {
        if (!PhotonNetwork.IsMasterClient) 
        {
            return false;
        }

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.TryGetValue("IsPlayerReady", out object isPlayerReady))
            {
                if (!(bool) isPlayerReady)
                {
                    return false;
                }
                else
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void ClearRoomListView()
    {
        foreach (GameObject entry in roomlistElements.Values)
        {
            Destroy(entry);
        }

        roomlistElements.Clear();
    }

    public void LocalPlayerPropertiesUpdated()
    {
        startgameButton.gameObject.SetActive(CheckPlayersReady());
    }

    public void SetActivePanel(string activePanel)
    { 
        loginPanel.SetActive(activePanel.Equals(loginPanel.name));
        choicePanel.SetActive(activePanel.Equals(choicePanel.name));
        createRoomPanel.SetActive(activePanel.Equals(createRoomPanel.name));
        randomRoomJoinPanel.SetActive(activePanel.Equals(randomRoomJoinPanel.name));
        roomlistPanel.SetActive(activePanel.Equals(roomlistPanel.name));
        insideRoomPanel.SetActive(activePanel.Equals(insideRoomPanel.name));
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList) 
            {
                if (roomCacheList.ContainsKey(info.Name))
                {
                    roomCacheList.Remove(info.Name);
                }

                continue; // continue with next room
            }

            if (roomCacheList.ContainsKey(info.Name))
            {
                roomCacheList[info.Name] = info;
            }
            else
            {
                roomCacheList.Add(info.Name, info);
            }
        }
    }

    private void UpdateRoomListView()
    {
        //float x = 0;
        //float y = 345;
        //float z = 0;
        //float spacing = 120;
        foreach (RoomInfo info in roomCacheList.Values)
        {
            GameObject entry = Instantiate(roomlistRowPrefab);//, roomlistContent.transform.localPosition + new Vector3(x, y, z), Quaternion.identity, roomlistContent.transform);
            entry.transform.SetParent(roomlistContent.transform);
            entry.transform.localScale = Vector3.one;

            entry.GetComponent<UIRoomListEntry>().Initialize(info.Name, (byte)info.PlayerCount, info.MaxPlayers);

            roomlistElements.Add(info.Name, entry);

            //y -= spacing;
        }
    }
}
