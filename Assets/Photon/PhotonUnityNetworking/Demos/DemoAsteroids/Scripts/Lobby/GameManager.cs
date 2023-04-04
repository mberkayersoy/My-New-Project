using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


    public class GameManager : MonoBehaviourPunCallbacks
    {
        [Header("Giris Paneli")]
        public GameObject GirisPanel;
        public InputField Oyuncuİsimİnput;

        [Header("Secim Paneli")]
        public GameObject SecimPanel;

        [Header("Oda Olusturma Paneli")]
        public GameObject OdaOlusturPanel;
        public InputField OdaAdiİnput;
        public InputField MaksimumOyuncuİnput;

        [Header("Random Odaya Giris Paneli")]
        public GameObject RandomodayagirPanel;

        [Header("Oda listesi Paneli")]
        public GameObject OdalistePanel;
        public GameObject OdalistesiContent;
        public GameObject OdalistesiSatirPrefab;

        [Header("Oda içi Paneli")]
        public GameObject OdaiciPanel;

        public Button OyunaBaslaButon;
        public GameObject OyunculistesiSatirPrefab;

        private Dictionary<string, RoomInfo> OdaCacheList;
        private Dictionary<string, GameObject> OdaListeElemanlari;
        private Dictionary<int, GameObject> OyuncuListeElemanlari;

  
        public void Awake()
        {
            

          
        }

       
        public override void OnConnectedToMaster()
        {
            
        }

        public override void OnJoinedLobby()
        {
           
        }

        public override void OnLeftLobby()
        {
          
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
           
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            
        }

        public override void OnJoinedRoom()
        {
           
           
        }

        public override void OnLeftRoom()
        {
            
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
           
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
           
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
          
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            
        }
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
           
        }
       

      

        public void OnBackButtonClicked()
        {
           
        }

        public void OnCreateRoomButtonClicked()
        {
            
        }

        public void OnJoinRandomRoomButtonClicked()
        {
          
        }

        public void OnLeaveGameButtonClicked()
        {
           
        }

        public void OnLoginButtonClicked()
        {
           
        }

        public void OnRoomListButtonClicked()
        {
           
        }

        public void OnStartGameButtonClicked()
        {
            
        }

       
    /*
        private bool CheckPlayersReady()
        {
            
        }*/
        
        private void ClearRoomListView()
        {
            
        }

        public void LocalPlayerPropertiesUpdated()
        {
            
        }

        public void SetActivePanel(string activePanel)
        {
            GirisPanel.SetActive(activePanel.Equals(GirisPanel.name));
            SecimPanel.SetActive(activePanel.Equals(SecimPanel.name));
            OdaOlusturPanel.SetActive(activePanel.Equals(OdaOlusturPanel.name));
            RandomodayagirPanel.SetActive(activePanel.Equals(RandomodayagirPanel.name));
            OdalistePanel.SetActive(activePanel.Equals(OdalistePanel.name));  
            OdaiciPanel.SetActive(activePanel.Equals(OdaiciPanel.name));
        }

        private void UpdateCachedRoomList(List<RoomInfo> roomList)
        {
            
        }

        private void UpdateRoomListView()
        {
            
        }
    }
