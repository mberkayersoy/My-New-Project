using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Chat;
using Photon.Realtime;
using AuthenticationValues = Photon.Chat.AuthenticationValues;
using Photon.Pun;



public class ChatGui : MonoBehaviour, IChatClientListener
{
	[Header("-----TEKNİK AYARLAR")]
	public string[] Baglanilabilecek_kanallar; 
	public int GecmisMesajlariGorebilmeTercihi;
	public string KullaniciAdi { get; set; }
	public ChatClient chatClient;
	#if !PHOTON_UNITY_NETWORKING
		[SerializeField]
	#endif
	protected internal ChatAppSettings chatAppSettings;
	private string SeciliKanal; 
	private readonly Dictionary<string, Toggle> channelToggles = new Dictionary<string, Toggle>();


	[Header("-----PANELLER VE DİĞER OBJELER")]
	public GameObject Baslik;
	public Text KullaniciAdiText; 
	public GameObject BaglantiDurumLabel;	
	public RectTransform ChatPanel;     
	public GameObject KullaniciGirisPaneli;
	public InputField MesajYazmaInputu;  
	public Text YazilanYazilarinTexti;    	
	

	public void Start()
	{
				       
	}

	public void Connect()
	{
		
	}

	public void OnConnected()
	{
		
	}

	public void OnDisconnected()
	{
		
		
	}


	public void OnDestroy()
    {
		
    }
    public void OnApplicationQuit()
	{
		
	}

	public void Update()
	{
		

	}

	public void OnEnterSend()
	{
		
	}

	public void OnClickSend()
	{
		
	}	

	private void SendChatMessage(string inputLine)
	{
		

	}

	public void OnGetMessages(string channelName, string[] senders, object[] messages)
	{
		
	}

	public void ShowChannel(string channelName)
	{
		
	}


	public void OnPrivateMessage(string sender, object message, string channelName)
	{
		
	}


	public void DebugReturn(ExitGames.Client.Photon.DebugLevel level, string message)
	{
	if (level == ExitGames.Client.Photon.DebugLevel.ERROR)
		{
			Debug.LogError(message);
		}
		else if (level == ExitGames.Client.Photon.DebugLevel.WARNING)
		{
			Debug.LogWarning(message);
		}
		else
		{
			Debug.Log(message);
		}
	}

	
	public void OnChatStateChange(ChatState state)
	{
		// sohbet durumu değiştiğinde buradan yakalanabilir.		
			
	}

	public void OnSubscribed(string[] channels, bool[] results)
	{
		

		Debug.Log("Bir Kanala Bağlanıldı : " + string.Join(", ", channels));
		
	    ShowChannel(channels[0]);
	}


    public void OnSubscribed(string channel, string[] users, Dictionary<object, object> properties)
    {
        Debug.LogFormat("Kanal : {0}, Kişi sayısı: {1} Kanal Özellikleri: {2}.", channel, users.Length, properties.ToStringFull());
    }

  
	public void OnUnsubscribed(string[] channels)
	{
	
		
		foreach (string channelName in channels)
		{

			Debug.Log("Kanaldan Ayrıldın :" + channelName + "'.");
			
		}
	}
	
		
	public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
	{
		/// Arkadaş listemizde ki kişilerin durumlarla ilgili güncellenmelerini alabiliriz.

			
	}

    public void OnUserSubscribed(string channel, string user)
    {
        Debug.LogFormat("Kanala Giren kişiler : channel=\"{0}\" userId=\"{1}\"", channel, user);
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        Debug.LogFormat("Kanaldan Çıkan kişiler : channel=\"{0}\" userId=\"{1}\"", channel, user);
    }

    
    public void OnChannelPropertiesChanged(string channel, string userId, Dictionary<object, object> properties)
    {
        Debug.LogFormat("Kanal Özellikleri Değiştirildi : {0} by {1}. Props: {2}.", channel, userId, Extensions.ToStringFull(properties));
    }

    public void OnUserPropertiesChanged(string channel, string targetUserId, string senderUserId, Dictionary<object, object> properties)
    {
        Debug.LogFormat("Kullanıcı Özellikleri Değiştirildi : (channel:{0} user:{1}) by {2}. Props: {3}.", channel, targetUserId, senderUserId, Extensions.ToStringFull(properties));
    }

  
    public void OnErrorInfo(string channel, string error, object data)
    {
        Debug.LogFormat("OnErrorInfo for channel {0}. Error: {1} Data: {2}", channel, error, data);
    }

  	
}