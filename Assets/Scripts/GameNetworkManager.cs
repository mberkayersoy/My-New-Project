/*
 * BU DOSYA EÐÝTÝMDE YAPTIÐIMIZ CHAT SÝSTEMÝNÝN SCRÝPT DOSYASIDIR. BU ÖRNEK, TÜM ÝÞLEMLERÝN BÝRLEÞTÝRÝLDÝÐÝ BÝR ÖRNEK OLDUÐU ÝÇÝN BU SÝSTEME ENTEGRE EDÝLMÝÞTÝR.
 * Gerekli yerlerde iþlem açýklamalarý eklenmiþtir. Bu açýklamalar sana bilgi verecektir.
 * 2021 - OLCAY KALYONCUOÐLU
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
    /*Point noktalarý oyuncularýn oluþturulacaklarý noktalardýr. Ben 4 adet belirledim, istersen daha fazla nokta belirleyebilirsin. Bu senin haritana göre deðiþir. 
     * */
    //public GameObject[] Pointnoktalari;
    //public GameObject GeriSayimSayacPaneli;
    //Coroutine OyununBitipBitmemesiniKontrolEdecek;

    //[Header("OYUN SONU OBJELERÝ")]
    //public GameObject OyunSonuPanel;
    //public TextMeshProUGUI OyunSonubilgi;

    //[Header("OYUNCU LÝSTESÝ")]
    //public GameObject Oyunculistesi;

    [Header("CHAT SÝSTEMÝ")]
    public GameObject ChatSistemi;
    public ChatGui chatgui;

    //[Header("VOÝCE SÝSTEMÝ")]
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
        /* Oyuncularý belirlemiþ olduðum 4 noktada random olarak oluþturuyorum. Bu yöntem ile bir noktaya birden fazla oyuncu denk gelebilir. Eðer bunu istemiyorsan oyuncularý sýra ile noktalarda oluþturabilirsin. Böylelikle anyý noktaya ayný oyuncu gelme þansý kalmaz. 
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
        Debug.Log("Odadan Oyuncu Çýktý");
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
                    Oyunculistesi.transform.Find("Text").GetComponent<Text>().text += "Oyuncu Adý : " + p.NickName + " - Kurucu \n";
                else
                    Oyunculistesi.transform.Find("Text").GetComponent<Text>().text += "Oyuncu Adý : " + p.NickName + " \n";
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
        // Chat sistemine kullanýcý adý ve oda adýný gönderiyoruz ve iletiþimi baþlatýyoruz
        ChatSistemi.SetActive(true);
        chatgui = FindObjectOfType<ChatGui>();
        chatgui.UserNickName = PhotonNetwork.LocalPlayer.NickName;
        chatgui.RoomName = PhotonNetwork.CurrentRoom.Name;
        chatgui.Connect();
        // Chat sistemine kullanýcý adý ve oda adýný gönderiyoruz ve iletiþimi baþlatýyoruz

        // Ses sistemine kullanýcý adý ve oda adýný gönderiyoruz ve iletiþimi baþlatýyoruz
        //VoiceSistemi.SetActive(true);
        //voice = FindObjectOfType<voicesistemi>();
        //voice.KullaniciAdi = PhotonNetwork.LocalPlayer.NickName;
        //voice.Odadi = PhotonNetwork.CurrentRoom.Name;
        // Ses sistemine kullanýcý adý ve oda adýný gönderiyoruz ve iletiþimi baþlatýyoruz
        //GeriSayimSayacPaneli.SetActive(false);
    }
}
