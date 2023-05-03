using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class UIRoomListEntry : MonoBehaviour
{
    public TextMeshProUGUI RoomNameText;
    public TextMeshProUGUI RoomPlayersText;
    public Button JoinRoomButton;

    private string roomName;
    private bool roomStatus;

    public void Start()
    {
        if (!roomStatus)
        {
            JoinRoomButton.interactable = true;

            JoinRoomButton.onClick.AddListener(() =>
            {
                if (PhotonNetwork.InLobby)
                {
                    PhotonNetwork.LeaveLobby();
                }

                PhotonNetwork.JoinRoom(roomName);
            });
        }
        else
        {
            JoinRoomButton.interactable = false;
        }
    }

    public void Initialize(string name, byte currentPlayers, byte maxPlayers, bool incomingStatus)
    {
        roomStatus = incomingStatus;
        roomName = name;
        RoomNameText.text = name;
        RoomPlayersText.text =  "Number of Player: " + currentPlayers + " / " + maxPlayers;
    }
}
