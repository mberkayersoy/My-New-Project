using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Threading.Tasks;

public class NetworkUIManager : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public static NetworkUIManager Instance;

    [Header("Login Panel")]
    public GameObject loginPanel;
    public TMP_InputField usernameInput;
    public GameObject UIBubblesPrefab;
    public GameObject UIBubblesObject;

    [Header("Register Panel")]

    public GameObject registerPanel;
    public TMP_InputField registerEmailInput;
    public TMP_InputField registerUsernameInput;
    public TMP_InputField registerPasswordInput;
    public TMP_InputField registerConfirmPasswordInput;
    public Button createAccountButton;

    [Header("Choice Panel")]
    public GameObject choicePanel;

    [Header("Right Panel")]
    public GameObject RightPanel;
    public GameObject FriendsPanel;
    public GameObject friendRowPrefab;
    public GameObject requestRowPrefab;
    public GameObject friendScrollview;
    public GameObject requestScrollview;
    public GameObject ExitConfirmPanel;
    public Transform friendContent;
    public Transform requestContent;
    public Button friendsPanelButton;
    public Button showFriendsButton;
    public Button showRequestButton;
    public Button sendRequestButton;
    public Button desktopButton;
    public TextMeshProUGUI feedbackText;
    public TMP_InputField newfriendUsernameInput;

    [Header("Create Room Panel")]
    public GameObject createRoomPanel;
    public TMP_InputField roomnameInput;
    public TMP_InputField maxPlayerInput;

    [Header("Random Room Join Panel")]
    public GameObject randomRoomJoinPanel;

    [Header("Room List Panel")]
    public GameObject roomlistPanel;
    public GameObject roomListInfo;
    public GameObject roomlistContent;
    public GameObject roomlistRowPrefab;

    [Header("Room Panel")]
    public GameObject insideRoomPanel;
    public Transform playersListTransform;
    public Button startgameButton;
    public GameObject insideRoomInfoPanel;
    public GameObject playerlistRowPrefab;

    private Dictionary<string, RoomInfo> roomCacheList;
    private Dictionary<string, GameObject> roomlistElements;
    private Dictionary<int, GameObject> playerlistElements;

    [Header("Loading Panel")]
    public GameObject LoadingPanel;
    public RawImage LoadingImage;
    public TextMeshProUGUI ConnectinStateText;

    [Header("CHAT System")]
    public GameObject ChatSistemi;
    public ChatGui chatgui;
    public bool continueFromChoicePanel = true;

    [Header("GamePanel")]
    public GameObject gamePanel;
    public GameManagerr gameManagerr;
    public Camera menuCamera;
    public GameObject controller;
    public GameObject playerManage;

    public void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;

        PhotonNetwork.AutomaticallySyncScene = true;
        roomCacheList = new Dictionary<string, RoomInfo>();
        roomlistElements = new Dictionary<string, GameObject>();
        UIBubblesObject = Instantiate(UIBubblesPrefab, new Vector3(-500, -275, 400), Quaternion.identity);
        requestScrollview.SetActive(false);
    }

    public void OnClickSendRequestButton()
    {
        if (string.IsNullOrEmpty(newfriendUsernameInput.text))
        {
            ShowFeedBackText("The username field cannot be left blank.");
            return;
        }
        FirebaseManager.Instance.SendRequest(newfriendUsernameInput.text);
    }

    public void ShowFeedBackText(string feedback)
    {
        feedbackText.text = feedback;
        Invoke(nameof(ClearFeedBackText), 2f);
    }
    public void ClearFeedBackText()
    {
        feedbackText.text = "";
    }
    
    public override void OnConnectedToMaster()
    {
        LoadingImage.transform.DOKill();
        LoadingPanel.SetActive(false);
        ConnectinStateText.text = "Connected";
        PhotonNetwork.AutomaticallySyncScene = true;
        SetActivePanel(choicePanel.name);
        RightPanel.SetActive(true);
    }

    public override void OnJoinedLobby()
    {
        roomCacheList.Clear();
        ClearRoomListView();
        chatgui.Connect();
        ConnectinStateText.text = "Connecting to Lobby";
       // Debug.Log("onjoinedlobby");
    }

    public override void OnLeftLobby()
    {
        roomCacheList.Clear();
        ClearRoomListView();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        SetActivePanel(choicePanel.name);
        RightPanel.SetActive(true);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        SetActivePanel(choicePanel.name);
        RightPanel.SetActive(true);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        string roomName = "Room " + Random.Range(100, 90000);

        RoomOptions options = new RoomOptions { MaxPlayers = 4 };
        PhotonNetwork.CreateRoom(roomName, options, null);
    }

    public override void OnJoinedRoom()
    {
        // Chat sistemine kullanýcý adý ve oda adýný gönderiyoruz ve iletiþimi baþlatýyoruz
        ChatSistemi.SetActive(true);
        chatgui = FindObjectOfType<ChatGui>();
        chatgui.UserNickName = PhotonNetwork.LocalPlayer.NickName;
        chatgui.RoomName = PhotonNetwork.CurrentRoom.Name;
        chatgui.Connect();

        roomCacheList.Clear();

        SetActivePanel(insideRoomPanel.name);
        RightPanel.SetActive(true);

        if (playerlistElements == null)
        {
            playerlistElements = new Dictionary<int, GameObject>();
        }

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject entry = Instantiate(playerlistRowPrefab, playersListTransform);
            entry.transform.localScale = Vector3.one;

            entry.GetComponent<UIPlayerListEntry>().Initialize(player.ActorNumber, player.NickName);

            UpdateInsideRoomInfos();

            if (player.CustomProperties.TryGetValue("IsPlayerReady", out object isPlayerReady))
            {
                entry.GetComponent<UIPlayerListEntry>().SetPlayerReady((bool) isPlayerReady);
            }

            if (player.CustomProperties.TryGetValue("SelectedTeamID", out object selectedTeamID))
            {
                entry.GetComponent<UIPlayerListEntry>().SetPlayerTeamColor((TeamID)selectedTeamID);
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
        ChatSistemi.SetActive(false);
        SetActivePanel(choicePanel.name);
        RightPanel.SetActive(true);
        foreach (GameObject entry in playerlistElements.Values)
        {
            Destroy(entry);
        }
        PhotonNetwork.LocalPlayer.SetTeamID((int)TeamID.Blue_Team);
        Debug.Log(PhotonNetwork.LocalPlayer.GetTeamID());
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
        UpdateInsideRoomInfos();

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(playerlistElements[otherPlayer.ActorNumber].gameObject);
        playerlistElements.Remove(otherPlayer.ActorNumber);
        startgameButton.gameObject.SetActive(CheckPlayersReady());
        UpdateInsideRoomInfos();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            startgameButton.gameObject.SetActive(CheckPlayersReady());
        }
        UpdateInsideRoomInfos();
    }
    private void UpdateInsideRoomInfos()
    {
        insideRoomInfoPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Room Name: " + PhotonNetwork.CurrentRoom.Name;
        insideRoomInfoPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Room Owner: " + PhotonNetwork.MasterClient.NickName;
        insideRoomInfoPanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = PhotonNetwork.CurrentRoom.PlayerCount.ToString() + "/" + PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
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
            if (changedProps.TryGetValue("SelectedTeamID", out object selectedTeamID))
            {
                entry.GetComponent<UIPlayerListEntry>().SetPlayerTeamColor((TeamID)selectedTeamID);
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
    
    public void OnFriendsPanelButtonClicked()
    {
       FriendsPanel.SetActive(!FriendsPanel.activeSelf);
    }

    public void OnFriendShowButtonClicked()
    {
        friendScrollview.SetActive(true);
        requestScrollview.SetActive(false);
    }

    public void OnRequestShowButtonClicked()
    {
        requestScrollview.SetActive(true);
        friendScrollview.SetActive(false);
    }

    public void OnBackButtonClicked()
    {
        //if (PhotonNetwork.InLobby)
        //{
        //    PhotonNetwork.LeaveLobby();
        //}
        SetActivePanel(choicePanel.name);
        RightPanel.SetActive(true);
    }
    public void OnBackButtonLogin()
    {
        SetActivePanel(loginPanel.name);
    }

    public void OnCreateRoomButtonClicked()
    {
        string roomName = roomnameInput.text;

        roomName = (roomName.Equals(string.Empty)) ? "Room " + Random.Range(100, 90000) : roomName;

        byte.TryParse(maxPlayerInput.text, out byte maxPlayer);
        maxPlayer = (byte)Mathf.Clamp(maxPlayer, 1, 8);

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
        ChatSistemi.SetActive(false);
        PhotonNetwork.LeaveRoom();
    }

    public void OnLoginButtonClicked(string username)
    {
        PhotonNetwork.LocalPlayer.NickName = username;
        PhotonNetwork.ConnectUsingSettings();
        LoadingPanel.SetActive(true);
        LoadingImage.transform.DORotate(new Vector3(0f, 0f, -360f), 0.5f, RotateMode.FastBeyond360).SetLoops(-1); ;
        ConnectinStateText.text = "Connecting...";
        //PhotonNetwork.JoinLobby();
        OnJoinedLobby();
    }

    public void OnRoomListButtonClicked()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
        SetActivePanel(roomlistPanel.name);
        RightPanel.SetActive(true);
        InvokeRepeating(nameof(UpdateRoomListInfos), 0, 2);
    }

    void UpdateRoomListInfos()
    {
        if (roomlistPanel.activeSelf)
        {
            roomListInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Total Room: " + PhotonNetwork.CountOfRooms.ToString();
            roomListInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Players In Room: " + PhotonNetwork.CountOfPlayersInRooms.ToString();
            roomListInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Players In Lobby: " + PhotonNetwork.CountOfPlayersOnMaster.ToString();
            roomListInfo.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Online Players: " + PhotonNetwork.CountOfPlayers.ToString();
        }
        else
        {
            CancelInvoke(nameof(UpdateRoomListInfos));
        }
    }

    public void OnStartGameButtonClicked()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        UIBubblesObject.SetActive(false);
        PhotonNetwork.InstantiateRoomObject("GameManagerr", Vector3.zero, Quaternion.identity);
        PhotonNetwork.InstantiateRoomObject("GamePanel", Vector3.zero, Quaternion.identity);
        PhotonNetwork.InstantiateRoomObject("ScoreManager", Vector3.zero, Quaternion.identity);
        GameManagerr.Instance.StartTheGame();
        RightPanel.SetActive(false);
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
            }
            else // if player has npt isPlayerReady feature.
            {
                return false;
            }
        }
        Debug.Log("Checkpalyersready: " + PhotonNetwork.IsMasterClient);
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
        registerPanel.SetActive(activePanel.Equals(registerPanel.name));
        //gamePanel.SetActive(activePanel.Equals(gamePanel.name));
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
        foreach (RoomInfo info in roomCacheList.Values)
        {
            GameObject entry = Instantiate(roomlistRowPrefab);//, roomlistContent.transform.localPosition + new Vector3(x, y, z), Quaternion.identity, roomlistContent.transform);
            entry.transform.SetParent(roomlistContent.transform);
            entry.transform.localScale = Vector3.one;

            entry.GetComponent<UIRoomListEntry>().Initialize(info.Name, (byte)info.PlayerCount, info.MaxPlayers, (info.MaxPlayers == info.PlayerCount));

            roomlistElements.Add(info.Name, entry);
        }
    }
    public void OnClickDesktopButton()
    {
        ExitConfirmPanel.SetActive(true);
    }
    public void OnClickYesButton()
    {
        Application.Quit();
    }
    public void OnClickNoButton()
    {
        ExitConfirmPanel.SetActive(false);
    }

}
