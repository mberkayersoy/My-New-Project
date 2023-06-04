using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class PrivateChat : MonoBehaviour
{
    public Button closeTabButton;

    [Header("UI ELEMENTS")]
    public TextMeshProUGUI receiverUsername;
    public RectTransform InputPanel;
    public InputField MessageWritingInput;
    public Text WrittenTexts;

    void Start()
    {
        closeTabButton.onClick.AddListener(OnClickCloseTabButton);
        FirebaseManager.Instance.ListenChat(FirebaseManager.Instance.GetComponent<PlayerData>().GetUsername(), receiverUsername.text, this);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!string.IsNullOrEmpty(MessageWritingInput.text))
            {
                OnEnterSend();
                MessageWritingInput.Select();
                MessageWritingInput.ActivateInputField();
            }
        }
    }

    public void OnEnterSend()
    {
        SendChatMessage(MessageWritingInput.text);
        MessageWritingInput.text = "";
        MessageWritingInput.Select();
        MessageWritingInput.ActivateInputField();
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
    private void SendChatMessage(string newMessage)
    {
        if (string.IsNullOrEmpty(newMessage))
        {
            return;
        }

        FirebaseManager.Instance.UpdateChatAsync(FirebaseManager.Instance.GetComponent<PlayerData>().GetUsername(), receiverUsername.text, FirebaseManager.Instance.GetComponent<PlayerData>().GetUsername() + ": " + newMessage);
        FirebaseManager.Instance.ListenChat(FirebaseManager.Instance.GetComponent<PlayerData>().GetUsername(), receiverUsername.text, this);
    }

    public void WriteAllMessagesUI(List<string> messageData)
    {
        WrittenTexts.text = "";

        foreach (string message in messageData)
        {
            WrittenTexts.text += message + "\n";
        }
    }
    public void OnClickCloseTabButton()
    {
        gameObject.SetActive(false);
    }
}
