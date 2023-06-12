using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FriendRow : MonoBehaviour
{
    public string friendName;
    string status;
    public GameObject privateChatPanelPrefab;
    public GameObject currentChatPanel;
    public TextMeshProUGUI friendStatusText;
    public Button sendMessageButton;
    public Button removeFriendButton;

    void Start()
    {
        friendName = GetComponentInChildren<TextMeshProUGUI>().text;
        sendMessageButton.onClick.AddListener(OnClickSendMessageButton);
        removeFriendButton.onClick.AddListener(OnClickRemoveFriendButton);
    }

    public void OnClickSendMessageButton()
    {
        if (currentChatPanel == null) {

            currentChatPanel = Instantiate(privateChatPanelPrefab, GameObject.FindWithTag("RightPanel").transform);
            currentChatPanel.GetComponent<PrivateChat>().receiverUsername.text = friendName;
            return;
        }
        currentChatPanel.SetActive(!currentChatPanel.activeSelf);
    }

    public void OnClickRemoveFriendButton()
    {
        FirebaseManager.Instance.RemoveFriendAsync(friendName);
        Destroy(gameObject);
    }

    public void UpdateStatus(int newStatus)
    {
        status = newStatus switch
        {
            0 => "Offline",
            1 => "Online",
            2 => "In Match",
            _ => "default",
        };

        friendStatusText.text = status;
    }

}
