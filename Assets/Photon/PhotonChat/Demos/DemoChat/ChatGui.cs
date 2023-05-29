/*
 * BU DOSYA EĞİTİMDE YAPTIĞIMIZ CHAT SİSTEMİNİN SCRİPT DOSYASIDIR. BU ÖRNEK, TÜM İŞLEMLERİN BİRLEŞTİRİLDİĞİ BİR ÖRNEK OLDUĞU İÇİN BU SİSTEME ENTEGRE EDİLMİŞTİR.
 * Gerekli yerlerde işlem açıklamaları eklenmiştir. Bu açıklamalar sana bilgi verecektir.
 * 2021 - OLCAY KALYONCUOĞLU
 * */

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Chat;
using Photon.Realtime;
using AuthenticationValues = Photon.Chat.AuthenticationValues;
using Photon.Pun;
using TMPro;

public class ChatGui : MonoBehaviour, IChatClientListener
{
    [Header("TECHNICAL SETTINGS")]
    public string[] Connectable_channels;
    public int PreferencetoSeePastMessages;
    public string UserNickName { get; set; }
    public string RoomName { get; set; }
    public ChatClient chatClient;
#if !PHOTON_UNITY_NETWORKING
		[SerializeField]
#endif
    protected internal ChatAppSettings chatAppSettings;
    private string SelectedChannel;

    [Header("UI ELEMENTS")]
    public RectTransform ChatPanel;
    public RectTransform InputPanel;
    public InputField MessageWritingInput;
    public Text WrittenTexts;
    public bool isChatPanelOpen;

    public void Start()
    {
        isChatPanelOpen = false;
    }

    public void Connect()
    {
        chatAppSettings = PhotonNetwork.PhotonServerSettings.AppSettings.GetChatSettings();
        Connectable_channels[0] = RoomName;
        chatClient = new ChatClient(this);
#if !UNITY_WEBGL
        chatClient.UseBackgroundWorkerForSending = true;
#endif
        chatClient.AuthValues = new AuthenticationValues(UserNickName);
        chatClient.ConnectUsingSettings(chatAppSettings);
    }
    public void OnConnected()
    {
        if (Connectable_channels != null && Connectable_channels.Length > 0)
        {
            chatClient.Subscribe(Connectable_channels, PreferencetoSeePastMessages);

        }
        ChatPanel.gameObject.SetActive(true);
        InputPanel.gameObject.SetActive(false);
        MessageWritingInput.Select();
        MessageWritingInput.ActivateInputField();
    }
    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        // Arkadaşlaşlık sistemi kullanılacaksa arkadaşların durumlarını buradan görebilirsin. Bunun nasıl olduğunu chat sistemi bölümünde anlattım.
    }

    public void OnDestroy()
    {
        if (ChatPanel != null)
        {
            if (chatClient != null)
            {
                ChatPanel.gameObject.SetActive(true); // true
                chatClient.Disconnect();
            }
        }

    }
    private void OnDisable()
    {
        if (ChatPanel != null)
        {
            if (chatClient != null)
            {
                ChatPanel.gameObject.SetActive(false);
                chatClient.Disconnect();
            }
        }
        else
        {
            chatClient.Disconnect();
        }

    }
    public void OnApplicationQuit()
    {
        if (chatClient != null)
        {
            ChatPanel.gameObject.SetActive(true);
            chatClient.Disconnect();
        }
        else
        {
            chatClient.Disconnect();
        }
    }

    public void Update()
    {
        if (chatClient != null)
        {
            chatClient.Service();
        }

        //if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        //{
        //    if (MessageWritingInput.isFocused)
        //    {
        //        // InputField odaklanmış durumda
        //        // Yazma işlemlerini burada yapabilirsin
        //    }
        //    else
        //    {
        //        // InputField odaklanmamış durumda
        //        // Başka işlemleri burada yapabilirsin
        //    }
        //}
        Debug.Log(MessageWritingInput.isFocused);
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (isChatPanelOpen && string.IsNullOrEmpty(MessageWritingInput.text))
            {
                isChatPanelOpen = false;
                InputPanel.gameObject.SetActive(isChatPanelOpen);
            }
            else
            {
                isChatPanelOpen = true;
                OnEnterSend();
                InputPanel.gameObject.SetActive(isChatPanelOpen);
                MessageWritingInput.Select();
                MessageWritingInput.ActivateInputField();
            }
        }
    }
    public void OnEnterSend()
    {
        Debug.Log("OnEnterSend");
        //if (Input.GetKeyDown(KeyCode.Return))
        //{
            SendChatMessage(MessageWritingInput.text);
            MessageWritingInput.text = "";
            MessageWritingInput.Select();
            MessageWritingInput.ActivateInputField();
        //}
        if (isChatPanelOpen)
        {
            MessageWritingInput.Select();
            MessageWritingInput.ActivateInputField();
        }
    }

    public void OnClickSend()
    {
        if (!string.IsNullOrEmpty(MessageWritingInput.text))
        {
            SendChatMessage(MessageWritingInput.text);
            MessageWritingInput.text = "";
            MessageWritingInput.Select();
            MessageWritingInput.ActivateInputField();
        }   
    }
    private void SendChatMessage(string gelenmesaj)
    {
        if (string.IsNullOrEmpty(gelenmesaj))
        {
            return;
        }
        chatClient.PublishMessage(SelectedChannel, gelenmesaj);
    }
    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        // Arkadaşlar arasında özel mesaj atılacaksa buradan karşılanacak- Bunun nasıl olduğunu chat sistemi bölümünde anlattım.
    }
    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        if (channelName.Equals(SelectedChannel))
        {
            ShowChannel(SelectedChannel);
        }
    }
    public void ShowChannel(string channelName)
    {
        if (string.IsNullOrEmpty(channelName))
        {
            return;
        }

        bool varmi = chatClient.TryGetChannel(channelName, out ChatChannel channel);

        if (!varmi)
        {
            Debug.Log("Kanal bulunamadı mesajlar alınamadı " + channelName);
            return;
        }

        SelectedChannel = channelName;

        WrittenTexts.text = channel.ToStringMessages();
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
        ShowChannel(channels[0]);
    }
    public void OnDisconnected()
    {
        // Bağlantı koparsa işlem yapılabilir.
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