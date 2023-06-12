/*
 * BU DOSYA E��T�MDE YAPTI�IMIZ CHAT S�STEM�N�N SCR�PT DOSYASIDIR. BU �RNEK, T�M ��LEMLER�N B�RLE�T�R�LD��� B�R �RNEK OLDU�U ���N BU S�STEME ENTEGRE ED�LM��T�R.
 * Gerekli yerlerde i�lem a��klamalar� eklenmi�tir. Bu a��klamalar sana bilgi verecektir.
 * 2021 - OLCAY KALYONCUO�LU
 * */


using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun;
using TMPro;

public class GameNetworkManager : MonoBehaviourPunCallbacks
{
    public static GameNetworkManager Instance = null;

    //public Text InfoText;
    /*Point noktalar� oyuncular�n olu�turulacaklar� noktalard�r. Ben 4 adet belirledim, istersen daha fazla nokta belirleyebilirsin. Bu senin haritana g�re de�i�ir. 
     * */
    //public GameObject[] Pointnoktalari;
    //public GameObject GeriSayimSayacPaneli;
    //Coroutine OyununBitipBitmemesiniKontrolEdecek;

    //[Header("OYUN SONU OBJELER�")]
    //public GameObject OyunSonuPanel;
    //public TextMeshProUGUI OyunSonubilgi;

    //[Header("OYUNCU L�STES�")]
    //public GameObject Oyunculistesi;

    [Header("CHAT S�STEM�")]
    public GameObject ChatSistemi;
    public ChatGui chatgui;

    //[Header("VO�CE S�STEM�")]
    //public GameObject VoiceSistemi;
    //public voicesistemi voice;

    //PhotonView photonView;
    [Header("Benim Eklediklerim")]
    public GameObject controller;
    public void Awake()
    {
        Debug.Log("AWAKEE");
        PhotonNetwork.AutomaticallySyncScene = true;
        Instance = this;
        CreateController();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        //CountdownTimer.OnCountdownTimerHasExpired += OnCountdownTimerIsExpired;
        /* Oyuncular� belirlemi� oldu�um 4 noktada random olarak olu�turuyorum. Bu y�ntem ile bir noktaya birden fazla oyuncu denk gelebilir. E�er bunu istemiyorsan oyuncular� s�ra ile noktalarda olu�turabilirsin. B�ylelikle any� noktaya ayn� oyuncu gelme �ans� kalmaz. 
         * */
        //PhotonNetwork.Instantiate("Oyuncu", Pointnoktalari[Random.Range(0, 3)].transform.position, Quaternion.identity);
        //CreateController();

    }

    void CreateController()
    {
        switch (PhotonNetwork.LocalPlayer.GetTeamID())
        {
            case 0:
                controller = PhotonNetwork.Instantiate("CapsuleBlue", GameManagerr.Instance.GeneratePlayers(PhotonNetwork.LocalPlayer.GetTeamID()), Quaternion.identity, 0, new object[] { photonView.ViewID });
                break;
            case 1:
                controller = PhotonNetwork.Instantiate("CapsuleRed", GameManagerr.Instance.GeneratePlayers(PhotonNetwork.LocalPlayer.GetTeamID()), Quaternion.identity, 0, new object[] { photonView.ViewID });
                break;
            case 2:
                controller = PhotonNetwork.Instantiate("CapsuleGreen", GameManagerr.Instance.GeneratePlayers(PhotonNetwork.LocalPlayer.GetTeamID()), Quaternion.identity, 0, new object[] { photonView.ViewID });
                break;
            case 3:
                controller = PhotonNetwork.Instantiate("CapsuleYellow", GameManagerr.Instance.GeneratePlayers(PhotonNetwork.LocalPlayer.GetTeamID()), Quaternion.identity, 0, new object[] { photonView.ViewID });
                break;

        }
        //controller = PhotonNetwork.Instantiate("Capsule", Vector3.up, Quaternion.identity, 0, new object[] { photonView.ViewID });
        //controller.transform.position = GameManagerr.Instance.GeneratePlayers(controller);
        //GameManagerr.Instance.GeneratePlayers(controller);
    }

    public void CreateDiedController()
    {
        switch (PhotonNetwork.LocalPlayer.GetTeamID())
        {
            case 0:
                controller = PhotonNetwork.Instantiate("CapsuleBlue", GameManagerr.Instance.GeneratePlayers(PhotonNetwork.LocalPlayer.GetTeamID()), Quaternion.identity, 0, new object[] { photonView.ViewID });
                break;
            case 1:
                controller = PhotonNetwork.Instantiate("CapsuleRed", GameManagerr.Instance.GeneratePlayers(PhotonNetwork.LocalPlayer.GetTeamID()), Quaternion.identity, 0, new object[] { photonView.ViewID });
                break;
            case 2:
                controller = PhotonNetwork.Instantiate("CapsuleGreen", GameManagerr.Instance.GeneratePlayers(PhotonNetwork.LocalPlayer.GetTeamID()), Quaternion.identity, 0, new object[] { photonView.ViewID });
                break;
            case 3:
                controller = PhotonNetwork.Instantiate("CapsuleYellow", GameManagerr.Instance.GeneratePlayers(PhotonNetwork.LocalPlayer.GetTeamID()), Quaternion.identity, 0, new object[] { photonView.ViewID });
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

    public void Start()
    {
        Debug.Log("STARRRT");
        StartGame();
       // Hashtable props = new Hashtable
       //{

        //    {"PlayerLoadedLevel", true }

        //};
        // PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    public void Die()
    {
        PhotonNetwork.Destroy(controller);
        CreateDiedController();
    }

    #region PUN CALLBACKS

    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.LoadScene("Lobby");
    }
    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Odadan Oyuncu ��kt�");
    }

    #endregion

    /*private void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            Oyunculistesi.SetActive(true);
            Oyunculistesi.transform.Find("Text").GetComponent<Text>().text = "";

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                if (p.IsMasterClient)
                    Oyunculistesi.transform.Find("Text").GetComponent<Text>().text += "Oyuncu Ad� : " + p.NickName + " - Kurucu \n";
                else
                    Oyunculistesi.transform.Find("Text").GetComponent<Text>().text += "Oyuncu Ad� : " + p.NickName + " \n";
            }
        }
        else
        {
            Oyunculistesi.SetActive(false);
        }
    }*/

    public void LobbyeDon()
    {
        SceneManager.LoadScene("Lobby");
    }

    private void StartGame()
    {
        // Chat sistemine kullan�c� ad� ve oda ad�n� g�nderiyoruz ve ileti�imi ba�lat�yoruz
        ChatSistemi.SetActive(true);
        chatgui = FindObjectOfType<ChatGui>();
        chatgui.UserNickName = PhotonNetwork.LocalPlayer.NickName;
        chatgui.RoomName = PhotonNetwork.CurrentRoom.Name;
        chatgui.Connect();
        // Chat sistemine kullan�c� ad� ve oda ad�n� g�nderiyoruz ve ileti�imi ba�lat�yoruz

        // Ses sistemine kullan�c� ad� ve oda ad�n� g�nderiyoruz ve ileti�imi ba�lat�yoruz
        //VoiceSistemi.SetActive(true);
        //voice = FindObjectOfType<voicesistemi>();
        //voice.KullaniciAdi = PhotonNetwork.LocalPlayer.NickName;
        //voice.Odadi = PhotonNetwork.CurrentRoom.Name;
        // Ses sistemine kullan�c� ad� ve oda ad�n� g�nderiyoruz ve ileti�imi ba�lat�yoruz
        //GeriSayimSayacPaneli.SetActive(false);
    }
}
