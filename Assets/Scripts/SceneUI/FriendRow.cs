using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FriendRow : MonoBehaviour
{
    public string friendName;
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

    }
    public void OnClickRemoveFriendButton()
    {
        FirebaseManager.Instance.RemoveFriendAsync(friendName);
        Destroy(gameObject);
    }

}
