using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UsernameDisplay : MonoBehaviour
{
    /// <summary>
    /// Photon view instance for get username
    /// </summary>
    [SerializeField] PhotonView pw;

    /// <summary>
    /// Text component for display username
    /// </summary>
    [SerializeField] TextMeshProUGUI text;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time
    /// </summary>
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.SetText(pw.Owner.NickName);
        if (pw.IsMine) text.gameObject.SetActive(false);

    }
}
