using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RequestRow : MonoBehaviour
{
    public string senderName;
    public Button acceptRequestButton;
    public Button RejectRequestButton;
    // Start is called before the first frame update
    void Start()
    {
        senderName = GetComponentInChildren<TextMeshProUGUI>().text;
        acceptRequestButton.onClick.AddListener(OnClickAcceptButton);
        RejectRequestButton.onClick.AddListener(OnClickRejectButton);
    }

    public void OnClickAcceptButton()
    {
        FirebaseManager.Instance.AcceptRequestAsync(senderName);
        Destroy(gameObject);
    }
    public void OnClickRejectButton()
    {
        FirebaseManager.Instance.RejectRequestAsync(senderName);
        Destroy(gameObject);
    }
}
