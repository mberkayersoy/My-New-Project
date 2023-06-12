using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using System;
using Photon.Realtime;
using TMPro;

public class PlayerManage : MonoBehaviour
{
    //public PhotonView photonView;
    public GameObject controller;
    public TextMeshProUGUI InfoText;

    //[Header("OYUNCU LÝSTESÝ")]
    //public GameObject PlayersListPanel;
    //public TextMeshProUGUI PlayerListText;

    //[Header("CHAT SÝSTEMÝ")]
    //public GameObject ChatSistemi;
    //public ChatGui chatgui;


    public void Die()
    {
        PhotonNetwork.Destroy(controller);
        CreateDiedController();
    }

    void Awake()
    {
        //photonView = GetComponent<PhotonView>();

    }

    private void Update()
    {
        //if (Input.GetKey(KeyCode.Tab))
        //{
        //    PlayersListPanel.SetActive(true);
        //    PlayerListText.text = "";

        //    foreach (Player p in PhotonNetwork.PlayerList)
        //    {
        //        if (p.IsMasterClient)
        //            PlayerListText.text += p.NickName + " - Owner \n";
        //        else
        //            PlayerListText.text += p.NickName + " \n";
        //    }
        //}
        //else
        //{
        //    PlayersListPanel.SetActive(false);
        //}



        //if (chatgui.isChatPanelOpen)
        //{
        //    controller.GetComponent<PlayerAttribute>().isDead = true;
        //}
        //else
        //{
        //    controller.GetComponent<PlayerAttribute>().isDead = false;
        //}
    }

    //private void StartGame()
    //{
    //    // Chat sistemine kullanýcý adý ve oda adýný gönderiyoruz ve iletiþimi baþlatýyoruz

    //    ChatSistemi.SetActive(true);
    //    chatgui.UserNickName = PhotonNetwork.LocalPlayer.NickName;
    //    chatgui.RoomName = PhotonNetwork.CurrentRoom.Name;
    //    chatgui.Connect();
    //    // Chat sistemine kullanýcý adý ve oda adýný gönderiyoruz ve iletiþimi baþlatýyoruz

    //    // Ses sistemine kullanýcý adý ve oda adýný gönderiyoruz ve iletiþimi baþlatýyoruz
    //    //VoiceSistemi.SetActive(true);
    //    //voice = FindObjectOfType<voicesistemi>();
    //    //voice.KullaniciAdi = PhotonNetwork.LocalPlayer.NickName;
    //    //voice.Odadi = PhotonNetwork.CurrentRoom.Name;
    //    // Ses sistemine kullanýcý adý ve oda adýný gönderiyoruz ve iletiþimi baþlatýyoruz
    //    //GeriSayimSayacPaneli.SetActive(false);
    //}
    void Start()
    {
        //if (photonView.IsMine)
        //{
        //    CreateController();
        //    StartGame();
        //}
        CreateController();
        //StartGame();
    }
    void CreateController()
    {
        switch(PhotonNetwork.LocalPlayer.GetTeamID())
        {
            case 0:
                controller = PhotonNetwork.Instantiate("CapsuleBlue", GameManagerr.Instance.GeneratePlayers(PhotonNetwork.LocalPlayer.GetTeamID()), Quaternion.identity, 0, new object[] { PhotonNetwork.LocalPlayer.GetTeamID() });
                break;
            case 1:
                controller = PhotonNetwork.Instantiate("CapsuleRed", GameManagerr.Instance.GeneratePlayers(PhotonNetwork.LocalPlayer.GetTeamID()), Quaternion.identity, 0, new object[] { PhotonNetwork.LocalPlayer.GetTeamID() });
                break;
            case 2:
                controller = PhotonNetwork.Instantiate("CapsuleGreen", GameManagerr.Instance.GeneratePlayers(PhotonNetwork.LocalPlayer.GetTeamID()), Quaternion.identity, 0, new object[] { PhotonNetwork.LocalPlayer.GetTeamID() });
                break;
            case 3:
                controller = PhotonNetwork.Instantiate("CapsuleYellow", GameManagerr.Instance.GeneratePlayers(PhotonNetwork.LocalPlayer.GetTeamID()), Quaternion.identity, 0, new object[] { PhotonNetwork.LocalPlayer.GetTeamID() });
                break;

        }
        controller.GetComponent<PlayerAttribute>().isDead = true;
        StartCoroutine(Respawn(controller));
        //controller = PhotonNetwork.Instantiate("Capsule", Vector3.up, Quaternion.identity, 0, new object[] { photonView.ViewID });
        //controller.transform.position = GameManagerr.Instance.GeneratePlayers(controller);
        //GameManagerr.Instance.GeneratePlayers(controller);
    }
    public void CreateDiedController()
    {
        switch (PhotonNetwork.LocalPlayer.GetTeamID())
        {
            case 0:
                controller = PhotonNetwork.Instantiate("CapsuleBlue", GameManagerr.Instance.GeneratePlayers(PhotonNetwork.LocalPlayer.GetTeamID()), Quaternion.identity, 0, new object[] { PhotonNetwork.LocalPlayer.GetTeamID() });
                break;
            case 1:
                controller = PhotonNetwork.Instantiate("CapsuleRed", GameManagerr.Instance.GeneratePlayers(PhotonNetwork.LocalPlayer.GetTeamID()), Quaternion.identity, 0, new object[] { PhotonNetwork.LocalPlayer.GetTeamID() });
                break;
            case 2:
                controller = PhotonNetwork.Instantiate("CapsuleGreen", GameManagerr.Instance.GeneratePlayers(PhotonNetwork.LocalPlayer.GetTeamID()), Quaternion.identity, 0, new object[] { PhotonNetwork.LocalPlayer.GetTeamID() });
                break;
            case 3:
                controller = PhotonNetwork.Instantiate("CapsuleYellow", GameManagerr.Instance.GeneratePlayers(PhotonNetwork.LocalPlayer.GetTeamID()), Quaternion.identity, 0, new object[] { PhotonNetwork.LocalPlayer.GetTeamID() });
                break;
        }
        //controller = PhotonNetwork.Instantiate("Capsule", GameManagerr.Instance.GeneratePlayers(controller), Quaternion.identity, 0, new object[] { photonView.ViewID });
        controller.GetComponent<PlayerAttribute>().isDead = true;
        controller.GetComponentInChildren<PersonalCanvas>().DeadSectionOn();
        StartCoroutine(Respawn(controller));
    }
    IEnumerator Respawn(GameObject controller)
    {
        yield return new WaitForSeconds(controller.GetComponentInChildren<PersonalCanvas>().respawnRemainingTime);
        controller.GetComponent<PlayerAttribute>().isDead = false;
        controller.GetComponentInChildren<PersonalCanvas>().DeadSectionOff(); // Deactivate player dead canvas.
    }
}
